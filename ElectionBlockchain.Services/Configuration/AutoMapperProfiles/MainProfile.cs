using AutoMapper;
using ElectionBlockchain.Model.DataModels;
using System.Text.RegularExpressions;

namespace ElectionBlockchain.Services.Configuration.AutoMapperProfiles;
public class MainProfile : Profile
{
   public MainProfile()
   {
      CreateMap<VoteQueue, Vote>(); // map from VoteQueue(src) to Vote(dst)

   }
}
