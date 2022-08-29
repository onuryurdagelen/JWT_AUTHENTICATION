using AuthServer.Core.DTO;
using AuthServer.Core.Entity;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service
{
   class DtoMapper : Profile
    {
        public DtoMapper()
        {
            CreateMap<ProductDto, Product>().ReverseMap(); // Tam tersi de olabilecegini belirtmek icin ReverseMap() kullanilir.
            CreateMap<UserAppDto, UserApp>().ReverseMap();
        }
    }
}
