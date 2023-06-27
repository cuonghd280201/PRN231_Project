using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebClient.Models;

namespace WebClient.Controllers
{
	public class AdminController : Controller
	{

		private readonly HttpClient _httpClient;
		public AdminController(IHttpClientFactory httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient();
		}

		public async Task<IActionResult> Order()
		{
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetCookie("Token"));
			var response = await _httpClient.GetAsync("http://localhost:5291/api/orders");
			var ListOrder = new ListOrderModel();
			string jsonResponse = await response.Content.ReadAsStringAsync();
			List<Order> orders = JsonConvert.DeserializeObject<List<Order>>(jsonResponse);
			ListOrder.Orders = orders;
			return View(ListOrder);
		}

		public async Task<IActionResult> MakeDone(int orderId)
		{
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetCookie("Token"));
			await _httpClient.PutAsync(string.Format("http://localhost:5291/api/orders/make-done/{0}", orderId), null);
			return RedirectToAction("Order", "Admin");
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

		public async Task<IActionResult> FlowerUpdate(int flowerId)
		{
			var url = string.Format("http://localhost:5291/api/flowers/{0}", flowerId);
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetCookie("Token"));
			var response = await _httpClient.GetAsync(url);
			var viewModel = new ListFlower();
			string jsonFlower = await response.Content.ReadAsStringAsync();
			var FlowerBouquet = JsonConvert.DeserializeObject<FlowerBouquet>(jsonFlower);

			response = await _httpClient.GetAsync("http://localhost:5291/api/categories");
			string jsonCategory = await response.Content.ReadAsStringAsync();
			var categories = JsonConvert.DeserializeObject<List<Category>>(jsonCategory);
			ViewBag.Categories = categories!.Select(c => new SelectListItem
			{
				Value = c.CategoryId.ToString(),
				Text = c.CategoryName
			}).ToList();
			var supplierresponse = await _httpClient.GetAsync("http://localhost:5291/api/suppliers");
			string jsosupplier = await supplierresponse.Content.ReadAsStringAsync();
			var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(jsosupplier);
			ViewBag.Suppliers = suppliers!.Select(c => new SelectListItem
			{
				Value = c.SupplierId.ToString(),
				Text = c.SupplierName
			}).ToList();
			return View(FlowerBouquet);
		}

		[HttpPost]
		public async Task<IActionResult> FlowerUpdate(FlowerBouquet request)
		{
			if (!ModelState.IsValid)
			{
				var categoryresponse = await _httpClient.GetAsync("http://localhost:5291/api/categories");
				string jsonCategory = await categoryresponse.Content.ReadAsStringAsync();
				var categories = JsonConvert.DeserializeObject<List<Category>>(jsonCategory);
				ViewBag.Categories = categories!.Select(c => new SelectListItem
				{
					Value = c.CategoryId.ToString(),
					Text = c.CategoryName
				}).ToList();

				var supplierresponse = await _httpClient.GetAsync("http://localhost:5291/api/suppliers");
				string jsosupplier = await supplierresponse.Content.ReadAsStringAsync();
				var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(jsosupplier);
				ViewBag.Suppliers = suppliers!.Select(c => new SelectListItem
				{
					Value = c.SupplierId.ToString(),
					Text = c.SupplierName
				}).ToList();
				return View(request);
			}
			var url = "http://localhost:5291/api/flowers";
			var json = System.Text.Json.JsonSerializer.Serialize(request);
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetCookie("Token"));
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = await _httpClient.PutAsync(url, content);
			return RedirectToAction("Flower", "Admin");
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
