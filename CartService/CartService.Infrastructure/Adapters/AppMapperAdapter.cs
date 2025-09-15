using CartService.Application.Interfaces;
using Mapster;
using System.Reflection;

namespace CartService.Infrastructure.Adapters
{
    public class AppMapperAdapter : IAppMapper
    {
        public AppMapperAdapter()
        {
            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
        }

        public T Map<T>(object entity)
        {
            return entity.Adapt<T>();
        }
    }
}
