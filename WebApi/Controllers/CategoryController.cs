using Bussiness;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            return Ok(_categoryService.ReadAll());
        }

        [HttpPost]
        public void Create([FromBody] Category category)
        {
            _categoryService.Create(category);
        }

        [HttpDelete("{categoryId}")]
        public void Delete([FromRoute] int categoryId)
        {
            _categoryService.Delete(categoryId);
        }
    }
}
