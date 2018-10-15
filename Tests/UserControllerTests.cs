using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
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
        public void GetAllUsers_WithoutAuthorizedUser_ShouldContainsAllUsers(int usersCount)
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

        [Fact]
        public void GetUser_WithAuthorizedUser_ShouldContainsAuthorizedUser()
        {
            // arrange
            var authorizedUser = _fixture.Fixture.Create<User>();

            _fixture.UserRepository.Setup(u => u.GetUserById(authorizedUser.Id)).Returns(authorizedUser).Verifiable();
            _fixture.SetAuthorizedUserId(authorizedUser.Id);

            // act
            var act = (UserGetModel)((ObjectResult)_fixture.Controller.GetUser()).Value;

            // assert
            Assert.Same(authorizedUser, act.GetEntity());

            _fixture.UserRepository.VerifyAll();
        }

        [Fact]
        public void GetUserWithCollections_WithAuthorizedUser_ShouldContainsAuthorizedUserWithCollections()
        {
            // arrange
            var authorizedUser = _fixture.Fixture.Create<User>();

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
        public void CreateUser_WithValidModel_ShouldCreatedWithProtectedPassword()
        {
            // arrange
            var expectedProtectedPassword = _fixture.Fixture.Create<string>();
            var expectedId = _fixture.Fixture.Create<int>();
            var user = _fixture.Fixture.Create<User>();

            var utf8Encoding = new UTF8Encoding(false, true).GetBytes(user.Password);
            var protectedData = WebEncoders.Base64UrlDecode(expectedProtectedPassword);

            _fixture.UserRepository.Setup(r => r.CreateUser(user)).Callback(() => user.Id = expectedId).Verifiable();
            _fixture.DataProtector.Setup(d => d.Protect(utf8Encoding)).Returns(protectedData).Verifiable();

            var model = new UserCreateOrUpdateModel(user);

            // act
            var act = (CreatedAtRouteResult)_fixture.Controller.CreateUser(model);

            // assert
            Assert.NotNull(act);
            Assert.Equal((int)HttpStatusCode.Created, act.StatusCode);
            Assert.Equal(expectedId, ((EntityGetModel)act.Value).Id);
            Assert.Equal(expectedProtectedPassword, ((User)((EntityGetModel)act.Value).GetEntity()).Password);

            _fixture.UserRepository.VerifyAll();
            _fixture.DataProtector.VerifyAll();
        }

        [Fact]
        public void UpdateUser_WithValidModelAndAuthorizedUser_ShouldUpdatedWithProtectedPassword()
        {
            // arrange
            var expectedProtectedPassword = _fixture.Fixture.Create<string>();
            var user = _fixture.Fixture.Create<User>();
            var dbUser = _fixture.Fixture.Create<User>();

            var utf8Encoding = new UTF8Encoding(false, true).GetBytes(user.Password);
            var protectedData = WebEncoders.Base64UrlDecode(expectedProtectedPassword);

            _fixture.SetAuthorizedUserId(user.Id);

            _fixture.UserRepository.Setup(r => r.GetUserById(user.Id)).Returns(dbUser).Verifiable();
            _fixture.UserRepository.Setup(r => r.UpdateUser(dbUser, user)).Callback(() => dbUser.Password = user.Password).Verifiable();
            _fixture.DataProtector.Setup(d => d.Protect(utf8Encoding)).Returns(protectedData).Verifiable();

            var model = new UserCreateOrUpdateModel(user);

            // act
            var act = (NoContentResult)_fixture.Controller.UpdateUser(model);

            // assert
            Assert.NotNull(act);
            Assert.Equal((int)HttpStatusCode.NoContent, act.StatusCode);
            Assert.Equal(expectedProtectedPassword, dbUser.Password);

            _fixture.UserRepository.VerifyAll();
            _fixture.DataProtector.VerifyAll();
        }

        [Fact]
        public void DeleteUser_WithValidModelAndAuthorizedUser_ShouldNoContentResult()
        {
            // arrange
            var user = _fixture.Fixture.Create<User>();

            _fixture.SetAuthorizedUserId(user.Id);

            _fixture.UserRepository.Setup(r => r.GetUserById(user.Id)).Returns(user).Verifiable();
            _fixture.UserRepository.Setup(r => r.DeleteUser(user)).Verifiable();

            // act
            var act = (NoContentResult)_fixture.Controller.DeleteUser();

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
        public Mock<IDataProtector> DataProtector { get; set; }

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

            DataProtector = Fixture.Freeze<Mock<IDataProtector>>();

            var dataProtectionProvider = Fixture.Freeze<Mock<IDataProtectionProvider>>();
            dataProtectionProvider.Setup(d => d.CreateProtector(It.IsAny<string>())).Returns(DataProtector.Object);

            _controller = new UserControllerMock(logger.Object, repositoryWrapper.Object, dataProtectionProvider.Object);
        }

        public void SetAuthorizedUserId(int authorizedUserId)
        {
            _controller.SetAuthorizedUserId(authorizedUserId);
        }

        public void Dispose()
        {
        }

        private class UserControllerMock : UserController
        {
            private int _authorizedUserId;

            public UserControllerMock(ILoggerManager logger, IRepositoryWrapper repository, IDataProtectionProvider dataProtectionProvider) : base(logger, repository, dataProtectionProvider)
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