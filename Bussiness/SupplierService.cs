using AutoMapper;
using DataAccess;
using DataAccess.Models;

namespace Bussiness
{
	public class SupplierService
	{
		private readonly IGenericRep<Supplier> _supplier;
		private readonly IMapper _mapper;
		public SupplierService(IGenericRep<Supplier> Supplier, IMapper mapper)
		{
			this._supplier = Supplier;
			this._mapper = mapper;
		}
		public void Create(Supplier supplier)
		{
			_supplier.Create(supplier);
		}

		public List<Supplier> ReadAll()
		{
			return _supplier.All.ToList();
		}
	}
}
