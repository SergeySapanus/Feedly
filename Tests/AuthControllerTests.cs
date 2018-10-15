using System;
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
            var act = _fixture.Controller.Login(null);

            // assert
            Assert.IsType<BadRequestObjectResult>(act);
            Assert.Equal((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)act).StatusCode);
        }

        [Fact]
        public void Login_WithInvalidUserName_ShouldNotFoundResult()
        {
            // arrange
            var user = _fixture.Fixture.Create<User>();

            _fixture.UserRepository.Setup(r => r.GetUserByName(user.Name)).Returns((User)null).Verifiable();

            var model = new UserCreateOrUpdateModel(user);

            // act
            var act = _fixture.Controller.Login(model);

            // assert
            Assert.IsType<NotFoundResult>(act);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFoundResult)act).StatusCode);

            _fixture.UserRepository.VerifyAll();
        }

        [Fact]
        public void Login_WithInvalidUserName_ShouldUnauthorizedResult()
        {
            // arrange
            var user = _fixture.Fixture.Create<User>();
            var invalidUser = _fixture.Fixture.Create<User>();

            _fixture.UserRepository.Setup(r => r.GetUserByName(invalidUser.Name)).Returns(user).Verifiable();

            var model = new UserCreateOrUpdateModel(invalidUser);

            // act
            var act = _fixture.Controller.Login(model);

            // assert
            Assert.IsType<UnauthorizedResult>(act);
            Assert.Equal((int)HttpStatusCode.Unauthorized, ((UnauthorizedResult)act).StatusCode);

            _fixture.UserRepository.VerifyAll();
        }

        [Fact]
        public void Login_WithInvalidPassword_ShouldUnauthorizedResult()
        {
            // arrange
            var user = _fixture.Fixture.Create<User>();
            var invalidUser = _fixture.Fixture.Create<User>();

            var protectedData = WebEncoders.Base64UrlDecode(user.Password);
            var utf8Encoding = new UTF8Encoding(false, true).GetBytes(invalidUser.Password);

            _fixture.UserRepository.Setup(r => r.GetUserByName(user.Name)).Returns(user).Verifiable();
            _fixture.DataProtector.Setup(d=>d.Unprotect(protectedData)).Returns(utf8Encoding).Verifiable();

            var model = new UserCreateOrUpdateModel(user);

            // act
            var act = _fixture.Controller.Login(model);

            // assert
            Assert.IsType<UnauthorizedResult>(act);
            Assert.Equal((int)HttpStatusCode.Unauthorized, ((UnauthorizedResult)act).StatusCode);

            _fixture.UserRepository.VerifyAll();
            _fixture.DataProtector.VerifyAll();
        }

        [Fact]
        public void Login_WithValidCredentials_ShouldOkResult()
        {
            // arrange
            var user = _fixture.Fixture.Create<User>();

            var protectedData = WebEncoders.Base64UrlDecode(user.Password);
            var utf8Encoding = new UTF8Encoding(false, true).GetBytes(user.Password);

            _fixture.UserRepository.Setup(r => r.GetUserByName(user.Name)).Returns(user).Verifiable();
            _fixture.DataProtector.Setup(d => d.Unprotect(protectedData)).Returns(utf8Encoding).Verifiable();

            var model = new UserCreateOrUpdateModel(user);

            // act
            var act = _fixture.Controller.Login(model);

            // assert
            Assert.IsType<OkObjectResult>(act);
            Assert.Equal((int)HttpStatusCode.OK, ((OkObjectResult)act).StatusCode);
            Assert.NotNull(((AuthGetModel)((OkObjectResult)act).Value).Token);

            _fixture.UserRepository.VerifyAll();
            _fixture.DataProtector.VerifyAll();
        }
    }

    public class AuthControllerFixture : IDisposable
    {
        public IFixture Fixture { get; set; }
        public Mock<IUserRepository> UserRepository { get; set; }
        public AuthController Controller { get; set; }
        public Mock<IDataProtector> DataProtector { get; set; }

        public AuthControllerFixture()
        {
            Fixture = new Fixture().Customize(new AutoMoqCustomization());
            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            UserRepository = Fixture.Freeze<Mock<IUserRepository>>();

            var repositoryWrapper = Fixture.Freeze<Mock<IRepositoryWrapper>>();
            repositoryWrapper.Setup(r => r.User).Returns(UserRepository.Object);

            DataProtector = Fixture.Freeze<Mock<IDataProtector>>();

            var dataProtectionProvider = Fixture.Freeze<Mock<IDataProtectionProvider>>();
            dataProtectionProvider.Setup(d => d.CreateProtector(It.IsAny<string>())).Returns(DataProtector.Object);

            var logger = Fixture.Freeze<Mock<ILoggerManager>>();
            Controller = new AuthController(logger.Object, repositoryWrapper.Object, dataProtectionProvider.Object);
        }

        public void Dispose()
        {
        }
    }
}