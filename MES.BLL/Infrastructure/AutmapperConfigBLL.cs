using System;
using AutoMapper;
using MES.BLL.DTO;
using MES.DAL.Entities;

namespace MES.BLL.Infrastructure
{
    public static class AutmapperConfigBll
    {
        public static readonly Action<IMapperConfigurationExpression> Configure = cfg =>
        {
            
            cfg.CreateMap<Detail, DetailDTO>().ReverseMap();
            cfg.CreateMap<ArrivalOfDetail, ArrivalOfDetailDto>().ReverseMap();
            cfg.CreateMap<DefectDetail, DefectDetailDto>().ReverseMap();
            cfg.CreateMap<Soldering, SolderingDto>().ReverseMap();
            cfg.CreateMap<CheckJmt, CheckJmtDto>().ReverseMap();
            cfg.CreateMap<ProductState, ProductStateDto>().ReverseMap();
            cfg.CreateMap<Boxing, BoxingDto>().ReverseMap();
        };
    }
}