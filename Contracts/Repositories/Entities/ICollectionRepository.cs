using Entities;

namespace Contracts.Repositories.Entities
{
    public interface ICollectionRepository : IRepositoryBase<Collection>
    {
        Collection GetCollectionById(int id);

        void CreateCollection(Collection collection);
    }
}