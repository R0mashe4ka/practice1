using AutoMapper;
using practice1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace practice1.Helpers
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<Person, PersonDTO>().ReverseMap();
            //CreateMap<PersonDTO, Person>();
        }

    }
}
