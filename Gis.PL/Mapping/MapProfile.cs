using AutoMapper;
using Gis.DAL.Models;
using Gis.PL.Dtos;

namespace Gis.PL.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<MosqueDto,Mosque>().ReverseMap();

            CreateMap<RestaurantDto, Restaurant>().ReverseMap();
            CreateMap<PharmacyDto, Pharmacy>().ReverseMap();
            CreateMap<MarketDto, Market>().ReverseMap();
            CreateMap<StudentHousingDto, StudentHousing>().ReverseMap();




        }
    }
}
