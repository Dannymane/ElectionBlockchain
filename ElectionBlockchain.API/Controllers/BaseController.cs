using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ElectionBlockchain.API.Controllers
{
   public abstract class BaseController : Controller
   {
      protected readonly IMapper Mapper;
      public BaseController(IMapper mapper)
      {
         Mapper = mapper;
      }

   }
}
