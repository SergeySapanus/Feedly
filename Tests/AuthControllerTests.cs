using System;
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
using MyFeedlyServer.Tests.Extensions;
using Xunit;

namespace MyFeedlyServer.Tests
{
    public class AuthControllerTests : IClassFixture<AuthControllerFixture>
    {
        private readonly AuthControllerFixture _fixture;

        public AuthControllerTests(AuthControllerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Login_WithoutCredentials_ShouldBadRequestResult()
        {
            // arrange

            // act
            var act = _fixture.AuthController.Login(null);

            // assert
            Assert.IsType<BadRequestObjectResult>(act);
            Assert.Equal((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)act).StatusCode);
        }

        [Fact]
        public void Login_WithInvalidUserName_ShouldNotFoundResult()
        {
            // arrange
            var user = _fixture.Fixture.Create<User>();

            _fixture.UserRepository.Setup(r => r.GetUserByName(user.Name)).Returns((User)null);

            var model = new UserCreateOrUpdateModel(user);

            // act
            var act = _fixture.AuthController.Login(model);

            // assert
            Assert.IsType<NotFoundResult>(act);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)act).StatusCode);
        }

        [Fact]
        public void Login_WithInvalidCredentials_ShouldUnauthorizedResult()
        {
            // arrange
            var user = _fixture.Fixture.Create<User>();

            _fixture.UserRepository.Setup(r => r.GetUserByName(user.Name)).Returns(user);

            var invalidUser = _fixture.Fixture.Create<User>();
            var model = new UserCreateOrUpdateModel(invalidUser);

            // act
            var act = _fixture.AuthController.Login(model);

            // assert
            Assert.IsType<UnauthorizedResult>(act);
            Assert.Equal((int)HttpStatusCode.Unauthorized, ((UnauthorizedResult)act).StatusCode);
        }

        [Fact]
        public void Login_WithValidCredentials_ShouldOkResult()
        {
            // arrange
            var user = _fixture.Fixture.Create<User>();

            _fixture.UserRepository.Setup(r => r.GetUserByName(user.Name)).Returns(user);

            var model = new UserCreateOrUpdateModel(user);

            // act
            var act = _fixture.AuthController.Login(model);

            // assert
            Assert.IsType<OkObjectResult>(act);
            Assert.Equal((int)HttpStatusCode.OK, ((OkObjectResult)act).StatusCode);
            Assert.NotNull(((OkObjectResult)act).Value.ConvertToDynamicByJsonConvert()?.Token);
        }
    }

    public class AuthControllerFixture : IDisposable
    {
        public IFixture Fixture { get; set; }
        public Mock<IUserRepository> UserRepository { get; set; }
        public AuthController AuthController { get; set; }

        public AuthControllerFixture()
        {
            Fixture = new Fixture().Customize(new AutoMoqCustomization());
            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            UserRepository = Fixture.Freeze<Mock<IUserRepository>>();

            var repositoryWrapper = Fixture.Freeze<Mock<IRepositoryWrapper>>();
            repositoryWrapper.Setup(r => r.User).Returns(UserRepository.Object);

            var logger = Fixture.Freeze<Mock<ILoggerManager>>();
            AuthController = new AuthController(logger.Object, repositoryWrapper.Object);
        }

        public void Dispose()
        {
        }
    }
}