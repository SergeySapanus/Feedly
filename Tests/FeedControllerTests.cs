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
using MyFeedlyServer.Entities.Models;
using Xunit;

namespace MyFeedlyServer.Tests
{
    public class FeedControllerTests : IClassFixture<FeedControllerFixture>
    {
        private readonly FeedControllerFixture _fixture;

        public FeedControllerTests(FeedControllerFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(3)]
        [InlineData(10)]
        public void GetAllFeeds_WithoutAuthorizedUser_ShouldContainsAllFeeds(int feedsCount)
        {
            // arrange
            var feeds = _fixture.Fixture.CreateMany<Feed>(feedsCount).ToArray();

            _fixture.FeedRepository.Setup(f => f.GetAllFeeds()).Returns(feeds).Verifiable();

            // act
            var act = ((IEnumerable<FeedGetModel>)((ObjectResult)_fixture.Controller.GetAllFeeds()).Value).ToArray();

            // assert
            Assert.Equal(feedsCount, act.Length);

            for (var i = 0; i < feedsCount; i++)
            {
                Assert.Same(feeds[i], act[i].GetEntity());
                Assert.Equal(feeds[i].Uri, act[i].Uri);
                Assert.Equal(feeds[i].Hash, act[i].Hash);
            }

            _fixture.FeedRepository.VerifyAll();
        }

        [Fact]
        public void GetFeedById_WithValidModel_ShouldOkResult()
        {
            // arrange
            var feed = _fixture.Fixture.Create<Feed>();

            _fixture.FeedRepository.Setup(r => r.GetFeedById(feed.Id)).Returns(feed).Verifiable();

            // act
            var act = _fixture.Controller.GetFeedById(feed.Id);

            // assert
            Assert.IsType<OkObjectResult>(act);
            Assert.Equal((int)HttpStatusCode.OK, ((OkObjectResult)act).StatusCode);

            _fixture.FeedRepository.VerifyAll();
        }

        [Fact]
        public void CreateFeed_FeedWithoutHash_ShouldCreateFeed()
        {
            // arrange
            var authorizedUser = _fixture.Fixture.Create<User>();
            var feed = _fixture.Fixture.Create<Feed>();
            var collection = _fixture.Fixture.Create<Collection>();

            var model = new FeedCreateOrUpdateModel(feed);

            _fixture.SetAuthorizedUserId(authorizedUser.Id);

            _fixture.CollectionRepository.Setup(r => r.GetCollectionByIdAndUserId(model.CollectionId, authorizedUser.Id)).Returns(collection).Verifiable();
            _fixture.FeedRepository.Setup(r => r.GetFeedByHash(feed.GetHashCode())).Returns((Feed)null).Verifiable();
            _fixture.FeedRepository.Setup(r => r.CreateFeed(collection, feed)).Verifiable();

            // act
            var act = (CreatedAtRouteResult)_fixture.Controller.CreateFeed(model);

            // assert
            Assert.NotNull(act);
            Assert.Equal((int)HttpStatusCode.Created, act.StatusCode);
            Assert.Equal(feed.Id, ((EntityModel<Feed>)act.Value).Id);

            _fixture.CollectionRepository.VerifyAll();
            _fixture.FeedRepository.VerifyAll();
        }

        [Fact]
        public void CreateFeed_FeedWithHash_ShouldCreateCollectionFeed()
        {
            // arrange
            var authorizedUser = _fixture.Fixture.Create<User>();
            var feed = _fixture.Fixture.Create<Feed>();
            var collection = _fixture.Fixture.Create<Collection>();

            var model = new FeedCreateOrUpdateModel(feed);

            _fixture.SetAuthorizedUserId(authorizedUser.Id);

            _fixture.CollectionRepository.Setup(r => r.GetCollectionByIdAndUserId(model.CollectionId, authorizedUser.Id)).Returns(collection).Verifiable();
            _fixture.FeedRepository.Setup(r => r.GetFeedByHash(feed.GetHashCode())).Returns(feed).Verifiable();
            _fixture.CollectionFeedRepository.Setup(r => r.CreateCollectionFeed(collection, feed)).Verifiable();

            // act
            var act = (CreatedAtRouteResult)_fixture.Controller.CreateFeed(model);

            // assert
            Assert.NotNull(act);
            Assert.Equal((int)HttpStatusCode.Created, act.StatusCode);
            Assert.Equal(feed.Id, ((EntityModel<Feed>)act.Value).Id);

            _fixture.CollectionRepository.VerifyAll();
            _fixture.FeedRepository.VerifyAll();
            _fixture.CollectionFeedRepository.VerifyAll();
        }
    }

    public class FeedControllerFixture : IDisposable
    {
        private readonly FeedControllerMock _controller;

        public IFixture Fixture { get; set; }
        public Mock<IFeedRepository> FeedRepository { get; set; }
        public Mock<ICollectionFeedRepository> CollectionFeedRepository { get; set; }
        public Mock<ICollectionRepository> CollectionRepository { get; set; }

        public FeedController Controller => _controller;

        public FeedControllerFixture()
        {
            Fixture = new Fixture().Customize(new AutoMoqCustomization());
            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            FeedRepository = Fixture.Freeze<Mock<IFeedRepository>>();
            CollectionFeedRepository = Fixture.Freeze<Mock<ICollectionFeedRepository>>();
            CollectionRepository = Fixture.Freeze<Mock<ICollectionRepository>>();

            var repositoryWrapper = Fixture.Freeze<Mock<IRepositoryWrapper>>();
            repositoryWrapper.Setup(r => r.Feed).Returns(FeedRepository.Object);
            repositoryWrapper.Setup(r => r.CollectionFeed).Returns(CollectionFeedRepository.Object);
            repositoryWrapper.Setup(r => r.Collection).Returns(CollectionRepository.Object);

            var logger = Fixture.Freeze<Mock<ILoggerManager>>();
            _controller = new FeedControllerMock(logger.Object, repositoryWrapper.Object);
        }

        public void SetAuthorizedUserId(int authorizedUserId)
        {
            _controller.SetAuthorizedUserId(authorizedUserId);
        }

        public void Dispose()
        {
        }

        private class FeedControllerMock : FeedController
        {
            private int _authorizedUserId;

            public FeedControllerMock(ILoggerManager logger, IRepositoryWrapper repository) : base(logger, repository)
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