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
            cfg.CreateMap<ArrivalOfDetailVm, ArrivalOfDetail>().ReverseMap();
            cfg.CreateMap<DisplayArrivalOfDetailVm, DisplayArrivalOfDetailDto>().ReverseMap();
            cfg.CreateMap<DefectDetailVm, DefectDetailDto>().ReverseMap();
            cfg.CreateMap<DefectDetailVm, DefectDetail>().ReverseMap();
            cfg.CreateMap<DefectDetailDisplayVm, DefectDetailDisplayDto>().ReverseMap();
            cfg.CreateMap<SolderingVm, SolderingDto>().ReverseMap();
            cfg.CreateMap<ProductVm, Product>().ReverseMap();
            cfg.CreateMap<SolderingCountVm, SolderingCountDto>().ReverseMap();
            cfg.CreateMap<ProductState, ProductStateVm>().ReverseMap();
            cfg.CreateMap<CheckJmtForListDto, CheckJmtForListVm>().ReverseMap();
            cfg.CreateMap<CheckJmtDto, CheckJmtVm>().ReverseMap();
        };
    }
}