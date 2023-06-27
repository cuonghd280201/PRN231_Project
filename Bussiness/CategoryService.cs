using AutoMapper;
using Common.ExceptionHandler.Exceptions;
using DataAccess;
using DataAccess.Models;

namespace Bussiness
{
    public class CategoryService
    {
        private readonly IGenericRep<Category> _categoryRep;
        private readonly IMapper _mapper;
        public CategoryService(IGenericRep<Category> categoryRep, IMapper mapper)
        {
            this._categoryRep = categoryRep;
            this._mapper = mapper;
        }

        public Category Read(int id)
        {
            var categories = _categoryRep.All;
            if (categories == null)
            {
                throw new BadRequestException("Category Not Found!");
            }
            var response = categories.FirstOrDefault(it => it.CategoryId == id);
            if (response == null)
            {
                throw new BadRequestException("Category Not Found!");
            }
            return response;
        }

        public List<Category> ReadAll()
        {
            var categories = _categoryRep.All;
            if (categories == null)
            {
                return new List<Category>();
            }
            return categories.ToList();
        }

        public void Create(Category category)
        {
            _categoryRep.Create(category);
        }

        public void Update(int id, Category category)
        {
            var categories = _categoryRep.All;
            if (categories == null)
            {
                throw new BadRequestException("Category Not Found!");
            }
            var response = categories.FirstOrDefault(it => it.CategoryId == id);
            if (response == null)
            {
                throw new BadRequestException("Category Not Found!");
            }
            _mapper.Map(category, response);
            _categoryRep.Update(response);
        }

        public void Delete(int id)
        {
            var categories = _categoryRep.All;
            if (categories == null)
            {
                throw new BadRequestException("Category Not Found!");
            }
            var response = categories.FirstOrDefault(it => it.CategoryId == id);
            if (response == null)
            {
                throw new BadRequestException("Category Not Found!");
            }
            _categoryRep.Delete(response);
        }
    }
}
