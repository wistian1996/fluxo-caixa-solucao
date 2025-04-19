using AutoMapper;
using GestaoFluxo.Application.Dtos;
using GestaoFluxo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFluxo.Application.Mappers
{
    internal class LancamentoMapperConfig : Profile
    {
        public LancamentoMapperConfig()
        {
            CreateMap<Lancamento, LancamentoDto>();
        }
    }
}
