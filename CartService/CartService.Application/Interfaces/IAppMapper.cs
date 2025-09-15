namespace CartService.Application.Interfaces
{
    public interface IAppMapper
    {
        T Map<T>(object entity);
    }
}
