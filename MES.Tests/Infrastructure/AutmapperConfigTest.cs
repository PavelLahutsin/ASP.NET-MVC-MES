using AutoMapper;
using MES.BLL.DTO;
using MES.DAL.Entities;
using System;

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
