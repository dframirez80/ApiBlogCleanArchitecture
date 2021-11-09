using NUnit.Framework;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using ApiBlogCA.Controllers;
using Domain.Repository;
using System;
using AutoMapper;
using Domain.Security;
using Repository.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Domain.DomainServices;
using Domain.Email;
using Microsoft.AspNetCore.Mvc;
using Domain.Repository.Entities;

namespace ApiBlogCA.Test
{
    public class UserControllerTest
    {
        IUnitOfWork Uow { get; set; }
        IMapper Mapper { get; set; }
        ITokenJwt TokenJwt { get; set; }
        IMailService EmailService { get; set; }
        IServiceProvider Container { get; set; }
        UsersHandler Handler { get; set; }
        UsersController Controller { get; set; }
        public UserControllerTest() {
            Container = ContainerFactory.Create();

            Uow = Container.GetService<IUnitOfWork>();
            Mapper = Container.GetService<IMapper>();
            TokenJwt = Container.GetService<ITokenJwt>();
            EmailService = Container.GetService<IMailService>();
            Handler = new UsersHandler(Uow, Mapper, EmailService, TokenJwt);
            Controller = new UsersController();
        }
        [SetUp]
        public void Setup() {
        }
        [Test]
        public void TestUser_ShouldResponseStringNoNullOrEmpty() {
            // arrange
            var expected = false;
            var controller = new UsersController();
            // act
            var result = string.IsNullOrEmpty(controller.TestUser());
            // assert
            Assert.AreEqual(expected, result);
        }
        [Test]
        public void AdminUser_Should_Response_String_NoNullOrEmpty() {
            // arrange
            var expected = false;
            var controller = new UsersController();
            // act
            var result = string.IsNullOrEmpty(controller.TestAdmin());
            // assert
            Assert.AreEqual(expected, result);
        }
        [Test]
        public void AdminUser_Should_Response_Message_Valid() {
            // arrange
            var expected = "You are a Admin";
            var controller = new UsersController();
            // act
            var result = controller.TestAdmin();
            // assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public async Task GetUser_Should_Response_Action_NotFound() {
            // arrange
            var expected = StatusCodes.Status404NotFound;
            // act
            var result = await Controller.GetUser(Handler,1000);
            // assert
            var resp = (StatusCodeResult)result.Result;
            Assert.AreEqual(expected, resp.StatusCode);
        }

    }
}