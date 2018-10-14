using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MyFeedlyServer.Contracts;
using MyFeedlyServer.Contracts.Repositories;
using MyFeedlyServer.Contracts.Repositories.Entities;
using MyFeedlyServer.Controllers;
using MyFeedlyServer.Entities.Entities;
using MyFeedlyServer.Models;
using Xunit;

namespace MyFeedlyServer.Tests
{
    public class CollectionControllerTests : IClassFixture<CollectionControllerFixture>
    {
        private readonly CollectionControllerFixture _fixture;

        public CollectionControllerTests(CollectionControllerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void GetCollectionById_WithValidModel_ShouldOkResult()
        {
            // arrange
            var authorizedUser = _fixture.Fixture.Create<User>();
            _fixture.SetAuthorizedUserId(authorizedUser.Id);

            var collection = _fixture.Fixture.Create<Collection>();

            _fixture.CollectionRepository.Setup(r => r.GetCollectionByIdAndUserId(collection.Id, authorizedUser.Id)).Returns(collection).Verifiable();

            // act
            var act = _fixture.Controller.GetCollectionById(collection.Id);

            // assert
            Assert.IsType<OkObjectResult>(act);
            Assert.Equal((int)HttpStatusCode.OK, ((OkObjectResult)act).StatusCode);

            _fixture.CollectionRepository.VerifyAll();
        }

        [Fact]
        public void CreateCollection_WithValidModel_ShouldCreateCollection()
        {
            // arrange
            var collection = _fixture.Fixture.Create<Collection>();

            _fixture.SetAuthorizedUserId(collection.UserId);

            var model = new CollectionCreateOrUpdateModel(collection);

            _fixture.CollectionRepository.Setup(r => r.CreateCollection(collection)).Verifiable();

            // act
            var act = (CreatedAtRouteResult)_fixture.Controller.CreateCollection(model);

            // assert
            Assert.NotNull(act);
            Assert.Equal((int)HttpStatusCode.Created, act.StatusCode);
            Assert.Equal(collection.Id, ((EntityGetModel)act.Value).Id);

            _fixture.CollectionRepository.VerifyAll();
        }

        [Theory]
        [InlineData(3)]
        [InlineData(10)]
        public void GetNewsByCollectionId_WithValidModel_ShouldOkResultAndAllNews(int newsCount)
        {
            // arrange
            var authorizedUser = _fixture.Fixture.Create<User>();
            _fixture.SetAuthorizedUserId(authorizedUser.Id);

            var collection = _fixture.Fixture.Create<Collection>();
            var news = _fixture.Fixture.CreateMany<News>(newsCount).ToArray();

            _fixture.CollectionRepository.Setup(r => r.GetCollectionByIdAndUserId(collection.Id, authorizedUser.Id)).Returns(collection).Verifiable();
            _fixture.FeedRepository.Setup(r => r.GetNewsByCollection(collection, _fixture.SyndicationManager.Object)).Returns(news).Verifiable();

            // act
            var act = _fixture.Controller.GetNewsByCollectionId(collection.Id);
            var actNews = ((IEnumerable<NewsGetModel>)((ObjectResult)act).Value).ToArray();

            // assert
            Assert.IsType<OkObjectResult>(act);
            Assert.Equal((int)HttpStatusCode.OK, ((OkObjectResult)act).StatusCode);

            Assert.Equal(newsCount, actNews.Length);

            for (var i = 0; i < newsCount; i++)
            {
                Assert.Same(news[i], actNews[i].GetEntity());
                Assert.Equal(news[i].Uri, actNews[i].Uri);
                Assert.Equal(news[i].Description, actNews[i].Description);
                Assert.Equal(news[i].Title, actNews[i].Title);
                Assert.Equal(news[i].LastUpdated, actNews[i].LastUpdated);
                Assert.Equal(news[i].Published, actNews[i].Published);
            }

            _fixture.CollectionRepository.VerifyAll();
            _fixture.FeedRepository.VerifyAll();
        }
    }

    public class CollectionControllerFixture : IDisposable
    {
        private readonly CollectionControllerMock _controller;

        public IFixture Fixture { get; set; }
        public Mock<IFeedRepository> FeedRepository { get; set; }
        public Mock<IUserRepository> UserRepository { get; set; }
        public Mock<ICollectionRepository> CollectionRepository { get; set; }
        public Mock<ISyndicationManager> SyndicationManager { get; set; }

        public CollectionController Controller => _controller;

        public CollectionControllerFixture()
        {
            Fixture = new Fixture().Customize(new AutoMoqCustomization());
            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            FeedRepository = Fixture.Freeze<Mock<IFeedRepository>>();
            UserRepository = Fixture.Freeze<Mock<IUserRepository>>();
            CollectionRepository = Fixture.Freeze<Mock<ICollectionRepository>>();

            var repositoryWrapper = Fixture.Freeze<Mock<IRepositoryWrapper>>();
            repositoryWrapper.Setup(r => r.Feed).Returns(FeedRepository.Object);
            repositoryWrapper.Setup(r => r.User).Returns(UserRepository.Object);
            repositoryWrapper.Setup(r => r.Collection).Returns(CollectionRepository.Object);

            var logger = Fixture.Freeze<Mock<ILoggerManager>>();

            SyndicationManager = Fixture.Freeze<Mock<ISyndicationManager>>();

            _controller = new CollectionControllerMock(logger.Object, repositoryWrapper.Object, SyndicationManager.Object);
        }

        public void SetAuthorizedUserId(int authorizedUserId)
        {
            _controller.SetAuthorizedUserId(authorizedUserId);
        }

        public void Dispose()
        {
        }

        private class CollectionControllerMock : CollectionController
        {
            private int _authorizedUserId;

            public CollectionControllerMock(ILoggerManager logger, IRepositoryWrapper repository, ISyndicationManager syndicationManager) : base(logger, repository, syndicationManager)
            {
            }

            public void SetAuthorizedUserId(int authorizedUserId)
            {
                _authorizedUserId = authorizedUserId;
            }

            protected override int AuthorizedUserId => _authorizedUserId;
        }
    }
}