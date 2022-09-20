using AutoMapper;
using Entities;
using Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<Developer, DeveloperMap>().ReverseMap();
        }
    }
}
