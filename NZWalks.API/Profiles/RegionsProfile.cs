using AutoMapper;

namespace NZWalks.API.Profiles
{
    public class RegionsProfile :Profile
    {
        public RegionsProfile()
        {
            //Learn Automapper
            CreateMap<Models.Domain.Region, Models.DTO.Region>()
                .ReverseMap();
        }
    }
}
