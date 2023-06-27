using Bussiness;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[Route("api/suppliers")]
	[ApiController]
	public class SupplierController : ControllerBase
	{
		private readonly SupplierService _supplierService;
		public SupplierController(SupplierService supplierService) {
			this._supplierService = supplierService;
		}
		[HttpGet]
		public List<Supplier> GetAll()
		{
			return _supplierService.ReadAll();
		}

		[HttpPost]
		public void Create([FromBody] Supplier request)
		{
			_supplierService.Create(request);
		}
	}
}
