using AutoMapper;
using DataAccess.Models;

namespace Bussiness.Config
{
	public class MappingProfile :  Profile
	{
		public MappingProfile()
		{
			CreateMap<Order, Order>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
			CreateMap<FlowerBouquet, FlowerBouquet>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
		}
	}
}
