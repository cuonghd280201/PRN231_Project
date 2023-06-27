using Bussiness;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[Route("api/flowers")]
	[ApiController]
	[Authorize]
	public class FlowerController : ControllerBase
	{
		private readonly FlowerService _flowerService;
		public FlowerController(FlowerService flowerService)
		{
			this._flowerService = flowerService;
		}

		[HttpGet]
		public List<FlowerBouquet> GetAll()
		{
			return _flowerService.ReadAll();
		}

		[HttpGet("{flowerId}")]
		public FlowerBouquet GetById([FromRoute] int flowerId)
		{
			return _flowerService.Read(flowerId);
		}

		[HttpPut]
		[Authorize(Roles = "Admin")]
		public void Update([FromBody] FlowerBouquet flowerBouquet)
		{
			 _flowerService.Update(flowerBouquet);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public void Create([FromBody] FlowerBouquet flowerBouquet)
		{
			_flowerService.Create(flowerBouquet);
		}
	}
}
