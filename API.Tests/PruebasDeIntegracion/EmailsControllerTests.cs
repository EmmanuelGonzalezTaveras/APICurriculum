using System;
using API.DTOs;
using Entidades;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Tests.PruebasDeIntegracion
{
    [TestClass]
	public class EmailsControllerTests: BasePruebas
	{
		
        private static readonly string url = "/api/emails";

        [TestMethod]
        public async Task ObtenerTodosLosEmailsListadoVacio()
        {
            var nombreBD = Guid.NewGuid().ToString();
            var factory = ConstruirWebApplicationFactory(nombreBD);

            var cliente = factory.CreateClient();
            var respuesta = await cliente.GetAsync(url);

            respuesta.EnsureSuccessStatusCode();

            var emails = JsonConvert
                .DeserializeObject<List<EmailDTO>>(await respuesta.Content.ReadAsStringAsync());

            Assert.AreEqual(0, emails.Count);
        }

        [TestMethod]
        public async Task ObtenerTodosLosEmails()
        {
            var nombreBD = Guid.NewGuid().ToString();
            var factory = ConstruirWebApplicationFactory(nombreBD);

            var contexto = ConstruirContext(nombreBD);
            contexto.Emails.Add(new Email() { EmailAddress = "primero@hotmail.com" });
            contexto.Emails.Add(new Email() { EmailAddress = "segundo@hotmail.com" });
            await contexto.SaveChangesAsync();

            var cliente = factory.CreateClient();
            var respuesta = await cliente.GetAsync(url);

            respuesta.EnsureSuccessStatusCode();

            var emails = JsonConvert
                .DeserializeObject<List<EmailDTO>>(await respuesta.Content.ReadAsStringAsync());

            Assert.AreEqual(2, emails.Count);
        }

        [TestMethod]
        public async Task BorrarEmail()
        {
            var nombreBD = Guid.NewGuid().ToString();
            var factory = ConstruirWebApplicationFactory(nombreBD);

            var contexto = ConstruirContext(nombreBD);
            contexto.Emails.Add(new Email() { Id = new Guid("2b977ecf-ee28-4273-9483-5a98d63e7336") , EmailAddress = "primero@hotmail.com" });
            await contexto.SaveChangesAsync();

            var cliente = factory.CreateClient();
            var respuesta = await cliente.DeleteAsync($"{url}/2b977ecf-ee28-4273-9483-5a98d63e7336");
            respuesta.EnsureSuccessStatusCode();

            var contexto2 = ConstruirContext(nombreBD);
            var existe = await contexto2.Emails.AnyAsync();
            Assert.IsFalse(existe);
        }

        [TestMethod]
        public async Task BorrarEmailRetorna401()
        {
            var nombreBD = Guid.NewGuid().ToString();
            var factory = ConstruirWebApplicationFactory(nombreBD, ignorarSeguridad: false);

            var cliente = factory.CreateClient();
            var respuesta = await cliente.DeleteAsync($"{url}/2b977ecf-ee28-4273-9483-5a98d63e7336");
            Assert.AreEqual("Unauthorized", respuesta.ReasonPhrase);
        }
    }
}

