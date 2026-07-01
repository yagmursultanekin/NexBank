using AutoMapper;
using NexBank.Application.DTOs;
using NexBank.Domain.Entities;

namespace NexBank.Application.Mappings;
    public class MappingProfile : Profile
    {
        public MappingProfile()
    {
        CreateMap<Account, AccountDto>();
        CreateMap<Transaction,TransactionDto>();
    }

    }
