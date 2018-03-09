using AutoMapper;
using MES.BLL.DTO;
using MES.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Tests.Infrastructure
{
    class AutmapperConfigTest
    {
        public static readonly Action<IMapperConfigurationExpression> Configure = cfg =>
        {

            cfg.CreateMap<Detail, DetailDTO>().ReverseMap();
           
        };
    }
}
