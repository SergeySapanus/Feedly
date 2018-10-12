using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoFixture;
using AutoFixture.AutoMoq;
using Contracts.Repositories;
using Contracts.Repositories.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MyFeedlyServer.Contracts;
using MyFeedlyServer.Controllers;
using MyFeedlyServer.Entities.Entities;
using MyFeedlyServer.Entities.Models;
using Xunit;

namespace MyFeedlyServer.Tests
{
    public class UserControllerTests : IClassFixture<UserControllerFixture>
    {
        private readonly UserControllerFixture _fixture;

        public UserControllerTests(UserControllerFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(3)]
        [InlineData(10)]
        public void GetAllUsers_GetResult_ShouldContainsAllUsers(int usersCount)
        {
            // arrange
            var users = _fixture.Fixture.CreateMany<User>(usersCount).ToArray();

            _fixture.UserRepository.Setup(u => u.GetAllUsers()).Returns(users).Verifiable();

            // act
            var act = ((IEnumerable<UserGetModel>)((ObjectResult)_fixture.Controller.GetAllUsers()).Value).ToArray();

            // assert
            Assert.Equal(usersCount, act.Length);

            for (var i = 0; i < usersCount; i++)
            {
                Assert.Same(users[i], act[i].GetEntity());
                Assert.Equal(users[i].Name, act[i].Name);
            }

            _fixture.UserRepository.VerifyAll();
        }

        [Theory]
        [InlineData(3, 1)]
        [InlineData(10, 4)]
        public void GetUser_GetResult_ShouldContainsAuthorizedUser(int usersCount, int authorizedUserIndex)
        {
            // arrange
            var users = _fixture.Fixture.CreateMany<User>(usersCount).ToArray();
            var authorizedUser = users[authorizedUserIndex];

            _fixture.UserRepository.Setup(u => u.GetUserById(authorizedUser.Id)).Returns(authorizedUser).Verifiable();
            _fixture.SetAuthorizedUserId(authorizedUser.Id);

            // act
            var act = (UserGetModel)((ObjectResult)_fixture.Controller.GetUser()).Value;

            // assert
            Assert.Same(authorizedUser, act.GetEntity());

            _fixture.UserRepository.VerifyAll();
        }

        [Theory]
        [InlineData(5, 2)]
        [InlineData(8, 7)]
        public void GetUserWithCollections_GetResult_ShouldContainsAuthorizedUserWithCollections(int usersCount, int authorizedUserIndex)
        {
            // arrange
            var users = _fixture.Fixture.CreateMany<User>(usersCount).ToArray();
            var authorizedUser = users[authorizedUserIndex];

            _fixture.UserRepository.Setup(u => u.GetUserById(authorizedUser.Id)).Returns(authorizedUser).Verifiable();
            _fixture.SetAuthorizedUserId(authorizedUser.Id);

            // act
            var act = ((UserWithCollectionsGetModel)((ObjectResult)_fixture.Controller.GetUserWithCollections()).Value).GetEntity();

            // assert
            Assert.Same(authorizedUser, act);
            Assert.All(act.Collections, collection => Assert.Contains(collection, authorizedUser.Collections));

            _fixture.UserRepository.VerifyAll();
        }

        [Fact]
        public void CreateUser_WithValidModel_ShouldCreatedAtRouteResult()
        {
            // arrange
            var expectedId = _fixture.Fixture.Create<int>();
            var user = _fixture.Fixture.Create<User>();

            _fixture.UserRepository.Setup(r => r.CreateUser(user)).Callback(() => user.Id = expectedId).Verifiable();

            var model = new UserCreateOrUpdateModel(user);

            // act
            var act = (CreatedAtRouteResult)_fixture.Controller.CreateUser(model);

            // assert
            Assert.NotNull(act);
            Assert.Equal((int)HttpStatusCode.Created, act.StatusCode);
            Assert.Equal(expectedId, ((EntityModel<User>)act.Value).Id);

            _fixture.UserRepository.VerifyAll();
        }

        [Fact]
        public void UpdateUser_WithValidModel_ShouldNoContentResult()
        {
            // arrange
            var user = _fixture.Fixture.Create<User>();
            var dbUser = _fixture.Fixture.Create<User>();

            _fixture.SetAuthorizedUserId(user.Id);

            _fixture.UserRepository.Setup(r => r.GetUserById(user.Id)).Returns(dbUser).Verifiable();
            _fixture.UserRepository.Setup(r => r.UpdateUser(dbUser, user)).Verifiable();

            var model = new UserCreateOrUpdateModel(user);

            // act
            var act = (NoContentResult)_fixture.Controller.UpdateUser(model);

            // assert
            Assert.NotNull(act);
            Assert.Equal((int)HttpStatusCode.NoContent, act.StatusCode);

            _fixture.UserRepository.VerifyAll();
        }
    }

    public class UserControllerFixture : IDisposable
    {
        private readonly UserControllerMock _controller;

        public IFixture Fixture { get; set; }
        public Mock<IUserRepository> UserRepository { get; set; }

        public UserController Controller => _controller;

        public UserControllerFixture()
        {
            Fixture = new Fixture().Customize(new AutoMoqCustomization());
            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            UserRepository = Fixture.Freeze<Mock<IUserRepository>>();

            var repositoryWrapper = Fixture.Freeze<Mock<IRepositoryWrapper>>();
            repositoryWrapper.Setup(r => r.User).Returns(UserRepository.Object);

            var logger = Fixture.Freeze<Mock<ILoggerManager>>();
            _controller = new UserControllerMock(logger.Object, repositoryWrapper.Object);
        }

        public void SetAuthorizedUserId(int? authorizedUserId)
        {
            _controller.SetAuthorizedUserId(authorizedUserId);
        }

        public void Dispose()
        {
        }

        private class UserControllerMock : UserController
        {
            private int? _authorizedUserId;

            public UserControllerMock(ILoggerManager logger, IRepositoryWrapper repository) : base(logger, repository)
            {
            }

            public void SetAuthorizedUserId(int? authorizedUserId)
            {
                _authorizedUserId = authorizedUserId;
            }

            protected override int? AuthorizedUserId => _authorizedUserId;
        }
    }
}