using System;
using AutoMapper;
using MES.BLL.DTO;
using MES.DAL.Entities;

namespace MES.BLL.Infrastructure
{
    public static class AutmapperConfigBLL
    {
        public static readonly Action<IMapperConfigurationExpression> Configure = cfg =>
        {
            cfg.CreateMap<UserDTO, ApplicationUser>().ReverseMap();
            cfg.CreateMap<Detail, DetailDTO>().ReverseMap();
            cfg.CreateMap<ArrivalOfDetail, ArrivalOfDetailDTO>().ReverseMap();

        };
    }
}