using AutoMapper;
using ShopBridge.Data.Entities;

namespace ShopBridge.Mapper
{
    public class ItemMapperProfile : Profile
    {
        public ItemMapperProfile()
        {
            CreateMap<Models.ItemModel, ItemEntity>();
            CreateMap<ItemEntity, Models.ItemModel>();
        }
    }
}
