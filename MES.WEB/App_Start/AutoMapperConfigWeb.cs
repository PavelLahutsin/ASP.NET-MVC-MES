using System;
using AutoMapper;
using MES.BLL.DTO;
using MES.DAL.Entities;
using MES.WEB.Models;

namespace MES.WEB
{
    public static class AutoMapperConfigWeb
    {
        public static readonly Action<IMapperConfigurationExpression> Config = cfg =>
        {
            cfg.CreateMap<DetailDTO, DetailVm>().ReverseMap();
            cfg.CreateMap<Detail, DetailVm>().ReverseMap();
            cfg.CreateMap<RegisterVm, UserDTO>().ReverseMap();
            cfg.CreateMap<LoginVm, UserDTO>().ReverseMap();
            cfg.CreateMap<ArrivalOfDetailVm, ArrivalOfDetailDto>().ReverseMap();
        };
    }
}