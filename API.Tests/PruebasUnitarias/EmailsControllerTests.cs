using API.Controllers;
using API.DTOs;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests.PruebasUnitarias
{
    [TestClass]
    public class EmailControllerTests : BasePruebas
    {
        [TestMethod]
        public async Task ObtenerTodosLosGeneros()
        {
            // Preparacion
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);
            var mapper = ConfigurarAutoMapper();

            contexto.Emails.Add(new Email() { EmailAddress = "Email 1" });
            contexto.Emails.Add(new Email() { EmailAddress = "Email 2" });
            await contexto.SaveChangesAsync();

            var contexto2 = ConstruirContext(nombreDB);

            //Prueba
            var controller = new EmailsController(contexto2, mapper);
            var respuesta = await controller.Get();

            //Verificacion
            var emails = respuesta.Value;
            Assert.AreEqual(2, emails.Count);

        }
        [TestMethod]
        public async Task ObtenerGeneroPorIdNoExistente()
        {
            // Preparacion
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);
            var mapper = ConfigurarAutoMapper();

            var controller = new EmailsController(contexto, mapper);
            var respuesta = await controller.Get(new Guid());

            var resultado = respuesta.Result as StatusCodeResult;
            Assert.AreEqual(404, resultado.StatusCode);

        }

        [TestMethod]
        public async Task ObtenerGeneroPorIdExistente()
        {
            // Preparacion
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);
            var mapper = ConfigurarAutoMapper();

            contexto.Emails.Add(new Email() { Id = new Guid("95c806bb-2888-4760-bbe4-1caef0f38fd4"), EmailAddress = "Email 1" });
            contexto.Emails.Add(new Email() { Id = new Guid("4e0622db-f90c-436c-82ca-4a755c0fdede"), EmailAddress = "Email 2" });
            await contexto.SaveChangesAsync();

            var contexto2 = ConstruirContext(nombreDB);

            //Prueba
            var controller = new EmailsController(contexto2, mapper);


            var id = new Guid("95c806bb-2888-4760-bbe4-1caef0f38fd4");
            var respuesta = await controller.Get(id);
            var resultado = respuesta.Value;
            Assert.AreEqual(id, resultado.Id);

        }

        [TestMethod]
        public async Task CrearEmail()
        {
            // Preparacion
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);
            var mapper = ConfigurarAutoMapper();

            var nuevoEmail = new EmailCreacionDTO() { EmailAddress = "Nuevo email" };

            var controller = new EmailsController(contexto, mapper);

            var respuesta = await controller.Post(nuevoEmail);
            var resultado = respuesta as CreatedAtRouteResult;
            Assert.IsNotNull(resultado);


            var contexto2 = ConstruirContext(nombreDB);
            var cantidad = await contexto2.Emails.CountAsync();
            Assert.AreEqual(1, cantidad);


            //Assert.IsTrue(ValidateModel(nuevoEmail));

        }

        [TestMethod]
        public async Task ActualizarEmail()
        {
            // Preparacion
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);
            var mapper = ConfigurarAutoMapper();

            contexto.Emails.Add(new Email() { Id = new Guid("4e0622db-f90c-436c-82ca-4a755c0fdede"), EmailAddress = "Email 1" });
            await contexto.SaveChangesAsync();


            var contexto2 = ConstruirContext(nombreDB);
            var controller = new EmailsController(contexto2, mapper);

            var emailCreacionDTO = new EmailCreacionDTO() { EmailAddress = "Nuevo email" };
            var id = new Guid("4e0622db-f90c-436c-82ca-4a755c0fdede");

            var respuesta = await controller.Put(id, emailCreacionDTO);

            var resultado = respuesta as StatusCodeResult;

            Assert.AreEqual(204, resultado.StatusCode);

            //para verificar que hay un nuevo email en la base de datos
            var contexto3 = ConstruirContext(nombreDB);
            var existe = await contexto3.Emails.AnyAsync(x => x.EmailAddress == "Nuevo email");
            Assert.IsTrue(existe);

        }


        [TestMethod]
        public async Task IntentaBorrarEmailNoExistente()
        {
            // Preparacion
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);
            var mapper = ConfigurarAutoMapper();

            var controller = new EmailsController(contexto, mapper);

            var respuesta = await controller.Delete(new Guid());

            var resultado = respuesta as StatusCodeResult;

            Assert.AreEqual(404, resultado.StatusCode);

        }

        [TestMethod]
        public async Task BorrarEmail()
        {
            // Preparacion
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);
            var mapper = ConfigurarAutoMapper();

            contexto.Emails.Add(new Email() { Id = new Guid("4e0622db-f90c-436c-82ca-4a755c0fdede"), EmailAddress = "Email 1" });
            await contexto.SaveChangesAsync();

            var contexto2 = ConstruirContext(nombreDB);

            var controller = new EmailsController(contexto2, mapper);

            var respuesta = await controller.Delete(new Guid("4e0622db-f90c-436c-82ca-4a755c0fdede"));

            var resultado = respuesta as StatusCodeResult;

            Assert.AreEqual(204, resultado.StatusCode);

            var contexto3 = ConstruirContext(nombreDB);

            var existe = await contexto3.Emails.AnyAsync();

            Assert.IsFalse(existe);
        }

    }
    
}
