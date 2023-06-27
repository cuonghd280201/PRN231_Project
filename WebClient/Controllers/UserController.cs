using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using WebClient.Models;

namespace WebClient.Controllers
{
	public class UserController : Controller
	{

		private readonly HttpClient _httpClient;
		public UserController(IHttpClientFactory httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient();
		}

		public async Task<IActionResult> Order()
		{
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetCookie("Token"));
			var url = string.Format("http://localhost:5291/api/orders/customers/{0}", GetCookie("UserId"));
			var response = await _httpClient.GetAsync(url);
			var viewModel = new ListOrderModel();
			string jsonResponse = await response.Content.ReadAsStringAsync();
			List<Order> orders = JsonConvert.DeserializeObject<List<Order>>(jsonResponse);
			viewModel.Orders = orders;
			return View(viewModel);
		}

		public async Task<IActionResult> OrderDetail(int orderId)
		{
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetCookie("Token"));
			var url = string.Format("http://localhost:5291/api/orders/{0}", orderId);
			var response = await _httpClient.GetAsync(url);
			var viewModel = new ListOrderDetail();
			string jsonResponse = await response.Content.ReadAsStringAsync();
			List<OrderDetail> orderDetails = JsonConvert.DeserializeObject<List<OrderDetail>>(jsonResponse);
			viewModel.OrderDetails = orderDetails;
			return View(viewModel);
		}

		public async Task<IActionResult> Flower()
		{
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetCookie("Token"));
			var response = await _httpClient.GetAsync("http://localhost:5291/api/flowers");
			var viewModel = new ListFlower();
			string jsonResponse = await response.Content.ReadAsStringAsync();
            viewModel.Flowers = JsonConvert.DeserializeObject<List<FlowerBouquet>>(jsonResponse);
			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> AddToOrder(int id)
		{
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetCookie("Token"));
			var url = string.Format("http://localhost:5291/customers/{0}/flower/{1}", GetCookie("UserId"), id);
			var response = await _httpClient.GetAsync(url);
			return RedirectToAction("Flower", "User");
		}

		public async Task<IActionResult> OrderDelete(int orderId)
		{
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetCookie("Token"));
			var url = string.Format("http://localhost:5291/api/orders/{0}", orderId);
			var response = await _httpClient.DeleteAsync(url);
			return RedirectToAction("Order", "User");
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
