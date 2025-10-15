using Mapster;
using OrderService.Application.Interfaces;
using System.Reflection;

namespace OrderService.Infrastructure.Adapters
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
