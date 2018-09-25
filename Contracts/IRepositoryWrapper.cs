namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }

        ICollectionRepository Collection { get; }

        ICollectionFeedRepository CollectionFeed { get; }

        IFeedRepository Feed { get; }

        INewsRepository News { get; }
    }
}