using System;
using AutoMapper;
using MES.BLL.DTO;
using MES.WEB.Models;

namespace MES.WEB
{
    public static class AutoMapperConfigWeb
    {
        public static readonly Action<IMapperConfigurationExpression> Config = cfg =>
        {
            //cfg.CreateMap<DetailDTO, DetailVM>().ReverseMap();
            cfg.CreateMap<RegisterVm, UserDTO>().ReverseMap();
            cfg.CreateMap<LoginVm, UserDTO>().ReverseMap();
            //cfg.CreateMap<ArrivalOfDetailVM, ArrivalOfDetailDTO>().ReverseMap();

        };
    }
}