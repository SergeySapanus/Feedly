using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyFeedlyServer.Contracts;
using MyFeedlyServer.Contracts.Repositories;
using MyFeedlyServer.Entities.Entities;
using MyFeedlyServer.Extensions;
using MyFeedlyServer.Filters;
using MyFeedlyServer.Models;
using MyFeedlyServer.Models.Extensions;
using MyFeedlyServer.Resources;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFeedlyServer.Controllers
{
    [SwaggerTag("Auth into MyFeedlyServer")]
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IDataProtector _dataProtector;

        public AuthController(ILoggerManager logger, IRepositoryWrapper repository, IDataProtectionProvider dataProtectionProvider)
        {
            _logger = logger;
            _repository = repository;
            _dataProtector = dataProtectionProvider.CreateProtector(GetDataProtectionPurpose());
        }

        [SwaggerOperation(
            Summary = "Login into MyFeedlyServer",
            Description = "On successful login JWT should has return",
            OperationId = "Login"
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "Login is successful", typeof(AuthGetModel))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "User hasn't been found in db")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid model object")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "User hasn't been authorized. Perhaps password was wrong")]
        [Route("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost]
        public IActionResult Login(
            [FromBody]
            [SwaggerParameter("User name and Password", Required = true)]
            UserCreateOrUpdateModel model)
        {
            if (model.IsNull())
            {
                _logger.LogError(string.Format(Resource.LogErrorInvalidModel, nameof(model), string.Empty));
                return BadRequest(Resource.Status400BadRequestInvalidModel);
            }

            var user = new UserCreateOrUpdateModel(_repository.User.GetUserByName(model.Name));
            if (user.IsNull())
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(user), nameof(model.Name), model.Name));
                return NotFound();
            }

            if (model.Name == user.Name && model.Password == _dataProtector.Unprotect(user.Password))
                return Ok(new AuthGetModel(GetToken(user), user.Id));

            return Unauthorized();
        }

        private string GetToken(EntityModel<User> model)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: "http://localhost:5000",
                audience: "http://localhost:5000",
                claims: new List<Claim> { new Claim(this.GetUserIdTypeName(), model.Id.ToString()) },
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        }
    }
}