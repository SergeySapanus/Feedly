using Entities.Concrete;

namespace Contracts
{
    public interface ICollectionRepository : IRepositoryBase<Collection>
    {
        Collection GetCollectionById(int id);

        void CreateCollection(Collection collection);
    }
}