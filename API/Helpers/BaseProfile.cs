using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;

namespace API.Helpers
{
    public class BaseProfile : Profile
    {
    public IGenericRepository<ProductBrand> BrandRepo { get; }
    public IGenericRepository<ProductType> TypeRepo { get; }

    public BaseProfile(IGenericRepository<ProductBrand> brandRepo, IGenericRepository<ProductType> typeRepo)
    {
        BrandRepo = brandRepo;
        TypeRepo = typeRepo;
    }

        public BaseProfile()
        {
        }
    }
}