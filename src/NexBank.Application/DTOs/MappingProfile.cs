using AutoMapper;
using NexBank.Domain.Entities;

namespace NexBank.Application.DTOs;
    public class MappingProfile : Profile
    {
        public MappingProfile()
    {
        CreateMap<Account, AccountDto>();
        CreateMap<Transaction,TransactionDto>();
    }

    }
