namespace OrderService.Application.Interfaces
{
    public interface IAppMapper
    {
        T Map<T>(object entity);
    }
}
