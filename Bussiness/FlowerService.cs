using AutoMapper;
using Common.ExceptionHandler.Exceptions;
using DataAccess;
using DataAccess.Models;

namespace Bussiness
{
    public class FlowerService
    {
        private readonly IGenericRep<FlowerBouquet> _flowerRep;
        private readonly IMapper _mapper;
        public FlowerService(IGenericRep<FlowerBouquet> _flowerRep, IMapper mapper)
        {
            this._flowerRep = _flowerRep;
            this._mapper = mapper;
        }

        public FlowerBouquet Read(int id)
        {
            var flowers = _flowerRep.All;
            if (flowers == null)
            {
                throw new BadRequestException("Flower Not Found!");
            }
            var response = flowers.FirstOrDefault(it => it.FlowerBouquetId == id);
            if (response == null)
            {
                throw new BadRequestException("Flower Not Found!");
            }
            return response;
        }

        public List<FlowerBouquet> ReadAll()
        {
            var flowers = _flowerRep.All;
            if (flowers == null)
            {
                return new List<FlowerBouquet>();
            }
            return flowers.ToList();
        }

        public void Create(FlowerBouquet flower)
        {
            _flowerRep.Create(flower);
        }

        public void Update(int id, FlowerBouquet flower)
        {
            var flowers = _flowerRep.All;
            if (flowers == null)
            {
                throw new BadRequestException("Flower Not Found!");
            }
            var response = flowers.FirstOrDefault(it => it.FlowerBouquetId == id);
            if (response == null)
            {
                throw new BadRequestException("Flower Not Found!");
            }
            _mapper.Map(flower, response);
            _flowerRep.Update(response);
        }

        public void Delete(int id)
        {
            var flowers = _flowerRep.All;
            if (flowers == null)
            {
                throw new BadRequestException("Flower Not Found!");
            }
            var response = flowers.FirstOrDefault(it => it.FlowerBouquetId == id);
            if (response == null)
            {
                throw new BadRequestException("Flower Not Found!");
            }
            _flowerRep.Delete(response);
        }

		public void Update(FlowerBouquet flowerBouquet)
		{
            var flowers = _flowerRep.All;
            if (flowers == null) {
                return;
            }
            var oldFlower = flowers.FirstOrDefault(it => it.FlowerBouquetId == flowerBouquet.FlowerBouquetId);
            if(oldFlower == null) {
                return;
            }
            _mapper.Map(flowerBouquet, oldFlower);
            _flowerRep.Update(oldFlower);
		}
	}
}
