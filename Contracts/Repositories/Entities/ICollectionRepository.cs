using MyFeedlyServer.Entities.Entities;

namespace MyFeedlyServer.Contracts.Repositories.Entities
{
    public interface ICollectionRepository : IRepositoryBase<Collection>
    {
        Collection GetCollectionById(int id);

        Collection GetCollectionByIdAndUserId(int id, int userId);

        void CreateCollection(Collection collection);
    }
}