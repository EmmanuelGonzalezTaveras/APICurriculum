using System;
using API.Controllers;
using API.DTOs.Cuentas;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace API.Tests.PruebasUnitarias
{
	[TestClass]
	public class CuentasControllerTests:BasePruebas
	{
		//[TestMethod]
		public async Task CrearUsuario()
		{
            var nombreDB = Guid.NewGuid().ToString();
            await CrearUsuarioHelper(nombreDB);
            var contexto2 = ConstruirContext(nombreDB);
            var conteo = await contexto2.Users.CountAsync();
            Assert.AreEqual(1, conteo);
		}


        public async Task CrearUsuarioHelper(string nombreDb)
        {
            var cuentasController = ConstruirCuentasController(nombreDb);
            var userInfo = new UserInfo() { Email = "micorreo@hotmail.com", Password = "Aa!miPassword" };
            await cuentasController.CreateUser(userInfo);
        }



        private CuentasController ConstruirCuentasController(string nombreBD)
        {
            var context = ConstruirContext(nombreBD);
            var miUserStore = new UserStore<IdentityUser>(context);
            var userManager = ConstruirUserManager(miUserStore);
            var mapper = ConfigurarAutoMapper();

            var httpContext = new DefaultHttpContext();
            MockAuth(httpContext);
            var signInManager = SetupSignInManager(userManager, httpContext);

            var miConfiguracion = new Dictionary<string, string>
            {
                {"JWT:key", "ALKSJDFLLASDIEZVXVCXNKASDKHJKASDJHFISUQWEROIUETRPODSFGOAKDFSLKHDFKGHKDSNOKQNONADSKJGNKJNDGKJDGFOHSNDKANSDKANSDPQJMLKSDNFGLKNJSDIFGJBQOENLKDFNGJKSDHBG" }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(miConfiguracion)
                .Build();

            return new CuentasController(userManager, signInManager, configuration, context, mapper);
        }



      //private UserManager<TUser> BuildUserManager<TUser>(IUserStore<TUser> store = null) where TUser : class
        private UserManager<TUser> ConstruirUserManager<TUser>(IUserStore<TUser>? store = null) where TUser : class

        {
            store = store ?? new Mock<IUserStore<TUser>>().Object;
            var options = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions();
            idOptions.Lockout.AllowedForNewUsers = false;

            options.Setup(o => o.Value).Returns(idOptions);

            var userValidators = new List<IUserValidator<TUser>>();

            var validator = new Mock<IUserValidator<TUser>>();
            userValidators.Add(validator.Object);
            var pwdValidators = new List<PasswordValidator<TUser>>();
            pwdValidators.Add(new PasswordValidator<TUser>());

            var userManager = new UserManager<TUser>(store, options.Object, new PasswordHasher<TUser>(),
                userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null,
                new Mock<ILogger<UserManager<TUser>>>().Object);

            validator.Setup(v => v.ValidateAsync(userManager, It.IsAny<TUser>()))
                .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();

            return userManager;
        }


        private static SignInManager<TUser> SetupSignInManager<TUser>(UserManager<TUser> manager,
            HttpContext context, ILogger logger = null, IdentityOptions identityOptions = null,
            IAuthenticationSchemeProvider schemeProvider = null) where TUser : class
        {
            var contextAccessor = new Mock<IHttpContextAccessor>();
            contextAccessor.Setup(a => a.HttpContext).Returns(context);
            identityOptions = identityOptions ?? new IdentityOptions();
            var options = new Mock<IOptions<IdentityOptions>>();
            options.Setup(a => a.Value).Returns(identityOptions);
            var claimsFactory = new UserClaimsPrincipalFactory<TUser>(manager, options.Object);
            schemeProvider = schemeProvider ?? new Mock<IAuthenticationSchemeProvider>().Object;
            var sm = new SignInManager<TUser>(manager, contextAccessor.Object, claimsFactory, options.Object, null, schemeProvider, new DefaultUserConfirmation<TUser>());
            sm.Logger = logger ?? (new Mock<ILogger<SignInManager<TUser>>>()).Object;
            return sm;
        }

        private Mock<IAuthenticationService> MockAuth(HttpContext context)
        {
            var auth = new Mock<IAuthenticationService>();
            context.RequestServices = new ServiceCollection().AddSingleton(auth.Object).BuildServiceProvider();
            return auth;
        }

    }
}

