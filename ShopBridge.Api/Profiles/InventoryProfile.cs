using AutoMapper;
using ShopBridge.Api.Dtos;
using ShopBridge.Api.Models;

namespace ShopBridge.Api.Profiles
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {
            CreateMap<InventoryItem, InventoryItemReadDto>();
            CreateMap<InventoryItemCreateDto, InventoryItem>();
            CreateMap<InventoryItemUpdateDto, InventoryItem>();
            CreateMap<InventoryItem, InventoryItemUpdateDto>();
        }
    }
}