﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using WebApp.Models.Token;
using WebClient.Models;
using WebClient.Models.Request;

namespace WebClient.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		private readonly HttpClient _httpClient;
		public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient();
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		public ActionResult Login()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> Login(LoginRequest loginRequest)
		{

			var url = "http://localhost:5291/api/customers/login";

			var json = JsonSerializer.Serialize(loginRequest);

			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync(url, content);

			if (response.IsSuccessStatusCode)
			{
				var responseContent = await response.Content.ReadAsStringAsync();
				var jsonDocument = JsonDocument.Parse(responseContent);
				var tokenValue = jsonDocument.RootElement.GetProperty("token").GetString();
				var tokenInfor = JwtUtils.Decode(tokenValue);
				CreateCookie("Role", tokenInfor.Role);
				CreateCookie("UserName", tokenInfor.Name);
				CreateCookie("UserId", tokenInfor.UserId);
				CreateCookie("Token", tokenValue);
				if (tokenInfor.Role.Equals("Admin"))
				{
					return RedirectToAction("Flower", "Admin");
				}
				else
				{
					return RedirectToAction("Flower", "User");
				}
			}
			else
			{
				ViewBag.Msg = "Thông tin tài khoản không chính xác";
				return View();
			}
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		private void CreateCookie(String name, String value)
		{
			Response.Cookies.Append(name, value, new CookieOptions
			{
				Expires = DateTime.Now.AddDays(1)
			});
		}

		public IActionResult Logout()
		{
			ClearCookie();
			return RedirectToAction("Login", "Home");
		}

		private void ClearCookie()
		{
			HttpContext.Response.Cookies.Delete("Token");
			HttpContext.Response.Cookies.Delete("Role");
			HttpContext.Response.Cookies.Delete("UserId");
			HttpContext.Response.Cookies.Delete("UserName");
		}

		public String GetCookie(String name)
		{
			if (Request.Cookies[name] != null)
			{
				return Request.Cookies[name];
			}
			return null;
		}
	}
}