using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionBlockchain.Services.ConcreteServices
{
   public class BaseService
   {
      protected readonly ApplicationDbContext DbContext = null!;
      protected readonly IMapper Mapper = null!;
      public BaseService(ApplicationDbContext dbContext, IMapper mapper)
      {
         DbContext = dbContext;
         Mapper = mapper;

      }
   }
}