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
using NSubstitute;
using static ApiBlogCA.Test.Fakes.UserControllerTest;
using Repository.Repositories.EntityDbContext;
using Microsoft.EntityFrameworkCore;
using Domain.Repository.Entities;
using System.Linq;

namespace ApiBlogCA.Test
{
    public partial class UserControllerTest
    {
        IUnitOfWork Uow { get; set; }
        IMapper Mapper { get; set; }
        ITokenJwt TokenJwt { get; set; }
        IMailService EmailService { get; set; }
        IServiceProvider Container { get; set; }
        UsersHandler Handler { get; set; }
        UsersController Controller { get; set; }
        
        [SetUp]
        public void Setup()
        {
            // Los test deben ser idempotentes: A igual parametros, deber�an arrojar igual resultado.
            // Con lo cual, inicializar los servicios una �nica vez y reutilizarlos, no siempre ser� lo mejor.
            // Porque un test, podr�a modificar el estado de un servicio y el pr�ximo test ya no ser�a idempotente,
            // porque se vi� afectado por el servicio modificado por el primer test.
            
            // No est� mal colocar la creaci�n en el constructor, incluso ser�a bueno para mejorar la performance
            // de los tests. Pero tener en cuenta que es m�s importante que sean idempotentes.
            // para que siempre que se ejecuten, sin importar el orden, el resultado sea satisfactorio.
            // Ning�n test deber�a depender de otro test o de otro resultado.

            // As� que, mientras tanto, es mejor crear y configurar todo con la corrida de cada test.
            // Hasta que puedas identiticar cu�les servicios se pueden utilizar, sin afectar el resultado de los tests.
            Container = ContainerFactory.Create();
            Uow = Container.GetService<IUnitOfWork>();
            Mapper = Container.GetService<IMapper>();
            TokenJwt = Container.GetService<ITokenJwt>(); // Este, quiz�s dependa de cada test, un test podr�a probar que un rol X funcione bien, y otro podr�a probar que el rol Y funcione bien.
            EmailService = Container.GetService<IMailService>(); // Este podr�a setearse una �nica vez. por ej.
            Handler = new UsersHandler(Uow, Mapper, EmailService, TokenJwt);
            Controller = new UsersController();

            // Si estuvieras probando el controller, y no te interesa probar el repository
            // Podr�as hacer que el repo devuelva siempre lo mismo, y concentrarte en probar solo el controller
            // Por ejemplo, podr�a inicializar ciertos substitutos / mocks.
            // Son objetos iguales en caracter�sticas a los reales, pero que retornan resultados predefinidos.
            // M�s info de referencia sobre c�mo implementarlo de forma gen�rica     
            // https://gist.github.com/jhoerr/9a064c83d04076fb056b98a30f4664f9
            // http://sinairv.github.io/blog/2015/10/04/mock-entity-framework-dbset-with-nsubstitute/

            // Tambi�n podr�as hacer sustitutos de tus clases, para poder retornar clases Fake (ficticias)
            // Que no hacen nada, o solo hacen lo m�nimo indispensable necesatio para tu test.
            Uow = Substitute.For<UnitOfWork>();
            Uow.Articles.Returns(new FakeArticlesRepository());
        }


        [Test]
        public void TestUser_ShouldResponseStringNoNullOrEmpty()
        {
            // arrange
            var expected = false;
            var controller = new UsersController();
            // act
            var result = string.IsNullOrEmpty(controller.TestUser());
            // assert
            Assert.AreEqual(expected, result);
        }
        [Test]
        public void AdminUser_Should_Response_String_NoNullOrEmpty()
        {
            // arrange
            var expected = false;
            var controller = new UsersController();
            // act
            var result = string.IsNullOrEmpty(controller.TestAdmin());
            // assert
            Assert.AreEqual(expected, result);
        }
        [Test]
        public void AdminUser_Should_Response_Message_Valid()
        {
            // arrange
            var expected = "You are a Admin";
            var controller = new UsersController();
            // act
            var result = controller.TestAdmin();
            // assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public async Task GetUser_Should_Response_Action_NotFound()
        {
            // arrange
            var expected = StatusCodes.Status404NotFound;
            // act
            var result = await Controller.GetUser(Handler, 1000);
            // assert
            var resp = (StatusCodeResult)result.Result;
            Assert.AreEqual(expected, resp.StatusCode);
        }

    }
}