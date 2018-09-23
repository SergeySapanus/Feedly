namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        ICollectionRepository Collection { get; }
    }
}