using Contracts;
using Entities;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly RepositoryContext _repoContext;
        private IUserRepository _user;
        private ICollectionRepository _collection;
        private ICollectionFeedRepository _collectionFeedRepository;
        private IFeedRepository _feedRepository;
        private INewsRepository _newsRepository;

        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public IUserRepository User => _user ?? (_user = new UserRepository(_repoContext));

        public ICollectionRepository Collection => _collection ?? (_collection = new CollectionRepository(_repoContext));

        public ICollectionFeedRepository CollectionFeed => _collectionFeedRepository ?? (_collectionFeedRepository = new CollectionFeedRepository(_repoContext));

        public IFeedRepository Feed => _feedRepository ?? (_feedRepository = new FeedRepository(_repoContext));

        public INewsRepository News => _newsRepository ?? (_newsRepository = new NewsRepository(_repoContext));
    }
}