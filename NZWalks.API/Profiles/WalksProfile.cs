using AutoMapper;

namespace NZWalks.API.Profiles
{
    public class WalksProfile:Profile
    {
        public WalksProfile()
        {
            //Learn Automapper
            CreateMap<Models.Domain.Walk, Models.DTO.Walk>()
                .ReverseMap();

            CreateMap<Models.Domain.WalkDifficulty, Models.DTO.WalkDifficulty>()
                .ReverseMap();
        }
    }
}
