using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Manifiesto.Data.Admin;
using Manifiesto.Web.Controllers;
using Moq;
using System.Web.Mvc;
using Manifiesto.Web.Models;
using System.Web;

namespace Manifiesto.Web.Tests.Controllers
{
    /// <summary>
    /// Summary description for AccountControllerTest
    /// </summary>
    [TestClass]
    public class AccountControllerTest
    {

        private Mock<IAdminService> _mockService; // dependency of controller
        private AccountController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockService = new Mock<IAdminService>();
            _controller = new AccountController(_mockService.Object);
        }

        [TestMethod]
        public void Login_Return_NotNull()
        {

            string returnUrl = "Invalid Model";

            var result = _controller.Login(returnUrl);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Login_Post_Return_InvalidModel()
        {
            LoginViewModel model = new LoginViewModel() { UserName = "admin", Password = "12345" };

            var result = _controller.Login(model);

            _controller.ModelState.AddModelError("", "Invalid Model");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Login_Post_InvalidUser()
        {
            LoginViewModel model = new LoginViewModel() { UserName = "admin", Password = "12345" };

            var result = _controller.Login(model);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Login_Post_Return_NotNull()
        {
            var mock = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mock.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);

            // Arrange
            LoginViewModel model = new LoginViewModel() { UserName = "soporte8", Password = "12345" };

            _mockService.Setup(a => a.Encrypt_MD5(model.Password)).Returns("827ccb0eea8a706c4c34a16891f84e7b");

            _mockService.Setup(f => f.get_users_login()).Returns(new List<Usuarios_EmpresasViewModel>
            {
                new Usuarios_EmpresasViewModel() { id_usuario = 1344, pw_name = "soporte8", pw_passwd = "827ccb0eea8a706c4c34a16891f84e7b" },
                new Usuarios_EmpresasViewModel() { id_usuario = 2, pw_name = "admin", pw_passwd = "12345"  }
            });

            _controller.ControllerContext = mock.Object;
            var result = _controller.Login(model);

            // Assert
            _mockService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));          
        }

        [TestMethod]
        public void LogOff_Return_NotNull()
        {
            RedirectToRouteResult result = _controller.LogOff() as RedirectToRouteResult;       
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

    }
}
