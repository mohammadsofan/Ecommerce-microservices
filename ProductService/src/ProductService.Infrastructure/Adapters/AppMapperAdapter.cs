using Mapster;
using ProductService.Application.Interfaces;

namespace ProductService.Infrastructure.Adapters
{
    public class AppMapperAdapter : IAppMapper
    {
        public T Map<T>(object entity)
        {
            return entity.Adapt<T>();
        }
    }
}
