using System;
using API.DTOs.Seguimientos;
using Entidades;
using Newtonsoft.Json;

namespace API.Tests.PruebasDeIntegracion
{
	[TestClass]
	public class SeguimientoControllerTest:BasePruebas
	{
        private static readonly string url = "/api/prestamos/9cba1a99-7b2f-4250-8a70-c31243b5b000/seguimientos";

        [TestMethod]
        public async Task ObtenerReviewsDevuelve404PeliculaInexistente()
        {
            var nombreBD = Guid.NewGuid().ToString();
            var factory = ConstruirWebApplicationFactory(nombreBD);

            var cliente = factory.CreateClient();
            var respuesta = await cliente.GetAsync(url);
            Assert.AreEqual(404, (int)respuesta.StatusCode);
        }

        [TestMethod]
        public async Task ObtenerSegumientosDevuelveListadoVacio()
        {
            var nombreBD = Guid.NewGuid().ToString();
            var factory = ConstruirWebApplicationFactory(nombreBD);
            var context = ConstruirContext(nombreBD);
            context.Prestamos.Add(new Prestamo() { Id = new Guid("9cba1a99-7b2f-4250-8a70-c31243b5b000"), PrestamoID = "001-000001" });
            await context.SaveChangesAsync();

            var cliente = factory.CreateClient();
            var respuesta = await cliente.GetAsync(url);

            respuesta.EnsureSuccessStatusCode();

            var seguimientos = JsonConvert.DeserializeObject<List<SeguimientoDTO>>(await respuesta.Content.ReadAsStringAsync());
            Assert.AreEqual(0, seguimientos.Count);
        }
    }
}

