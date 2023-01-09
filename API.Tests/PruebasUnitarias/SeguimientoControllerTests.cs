using System;
using API.Controllers;
using API.DTOs.Seguimientos;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace API.Tests.PruebasUnitarias
{
	[TestClass]
	public class SeguimientoControllerTests:BasePruebas
	{
        [TestMethod]
        public async Task UsuarioNoPuedeCrearDosSeguimientoParaUnMismoPrestamo()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);
            CrearPrestamo(nombreDB);

            var prestamoId = contexto.Prestamos.Select(x => x.Id).First();
            var segumiento1 = new Seguimiento()
            {
                PrestamoId = prestamoId,
                UsuarioId = usuarioPorDefectoId,
                Anotacion = "Anotacion 1"
            };

            contexto.Add(segumiento1);
            await contexto.SaveChangesAsync();


            var contexto2 = ConstruirContext(nombreDB);
            var mapper = ConfigurarAutoMapper();

            var controller = new SeguimientoController(contexto2, mapper, null);
            controller.ControllerContext = ConstruirControllerContext();

            var seguimientoCrecionDTO = new SeguimientoCreacionDTO { Anotacion = "Anotacion 2" };
            var respuesta = await controller.Post(prestamoId, seguimientoCrecionDTO);

            var valor = respuesta as IStatusCodeActionResult;
            Assert.AreEqual(400, valor.StatusCode.Value);

        }

        [TestMethod]
        public async Task CrearSegumiento()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);
            CrearPrestamo(nombreDB);

            var prestamoId = contexto.Prestamos.Select(x => x.Id).First();
            var contexto2 = ConstruirContext(nombreDB);

            var mapper = ConfigurarAutoMapper();

            var controller = new SeguimientoController(contexto2, mapper, null);
            controller.ControllerContext = ConstruirControllerContext();

            var seguimientoCrecionDTO = new SeguimientoCreacionDTO { Anotacion = "Anotacion 1" };
            var respuesta = await controller.Post(prestamoId, seguimientoCrecionDTO);

            var valor = respuesta as NoContentResult;
            Assert.IsNotNull(valor);

            var contexto3 = ConstruirContext(nombreDB);
            var seguimientoDb = contexto3.Seguimientos.First();
            Assert.AreEqual(usuarioPorDefectoId, seguimientoDb.UsuarioId);


        }





        private void CrearPrestamo(string nombreDB)
        {
            var contexto = ConstruirContext(nombreDB);

            contexto.Prestamos.Add(new Prestamo() { PrestamoID = "001-000001" });

            contexto.SaveChanges();
        }
    }
}

