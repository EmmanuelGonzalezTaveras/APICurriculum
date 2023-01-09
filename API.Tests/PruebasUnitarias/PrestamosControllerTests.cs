using API.Controllers;
using API.DTOs;
using Entidades;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enumeradores;
using NuGet.ContentModel;
using Moq;
using Microsoft.Extensions.Logging;

namespace API.Tests.PruebasUnitarias
{
    [TestClass]
    public class PrestamosControllerTests:BasePruebas
    {
        private string CrearDataPrueba()
        {
            var databaseName = Guid.NewGuid().ToString();
            var context = ConstruirContext(databaseName);

            var cartera = new Cartera() { 
                Id = new Guid("bd35ea7c-bd25-4ce8-89f5-245a72eb6c30"), 
                Nombre = "Cartera 1" 
            };

            var prestamos = new List<Prestamo>()
            {
                new Prestamo(){ Id = Guid.NewGuid(), PrestamoID = "Prestamo1", Estado = EstadoDePrestamo.Inactivo, FechaDeCreacion = DateTime.Today},
                new Prestamo(){Id = Guid.NewGuid(), PrestamoID = "Prestamo2", Estado = EstadoDePrestamo.Activo, FechaDeCreacion = DateTime.Today.AddDays(5)},
                new Prestamo(){Id = Guid.NewGuid(), PrestamoID = "Prestamo3", Estado = EstadoDePrestamo.Inactivo, FechaDeCreacion = DateTime.Today}
            };

            var prestamoConCartera = new Prestamo()
            {
                Id = new Guid("226651c0-14ae-49ef-a429-c906abcf8128"),
                Estado = EstadoDePrestamo.Inactivo,
                PrestamoID = "Prestamo4",
                FechaDeCreacion = DateTime.Today,
                
                
            };

            prestamos.Add(prestamoConCartera);

            context.Add(cartera);
            context.AddRange(prestamos);
            context.SaveChanges();

            var prestamoCartera = new PrestamosCarteras() { CarteraId = cartera.Id, PrestamoId = prestamoConCartera.Id };

            context.Add(prestamoCartera);
            context.SaveChanges();

            return databaseName;

        }


        [TestMethod]
        public async Task FiltraPorPrestamoId()
        {
            var nombreDb = CrearDataPrueba();
            var mapper = ConfigurarAutoMapper();
            var contexto = ConstruirContext(nombreDb);

            var controller = new PrestamosController(contexto, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var prestamoId = "Prestamo1";

            var filtroDTO = new PrestamoFiltroDTO()
            {
                PrestamoId = prestamoId,
                CantidadRegistrosPorPagina = 10
            };

            var respuesta = await controller.Filtrar(filtroDTO);
            var prestamos = respuesta.Value;
            Assert.AreEqual(1, prestamos.Count);
            Assert.AreEqual(prestamoId, prestamos[0].PrestamoID);

        }

        [TestMethod]
        public async Task FiltrarPrestamosActivos()
        {
            var nombreDb = CrearDataPrueba();
            var mapper = ConfigurarAutoMapper();
            var contexto = ConstruirContext(nombreDb);

            var controller = new PrestamosController(contexto, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var filtroDTO = new PrestamoFiltroDTO()
            {
                Estado = EstadoDePrestamo.Activo
            };

            var respuesta = await controller.Filtrar(filtroDTO);
            var prestamos = respuesta.Value;
            Assert.AreEqual(1, prestamos.Count);
            Assert.AreEqual(EstadoDePrestamo.Activo, prestamos[0].Estado);
            Assert.AreEqual("Prestamo2", prestamos[0].PrestamoID);
        }

        [TestMethod]
        public async Task FiltrarPorFechaDeCreacion()
        {

            var nombreDb = CrearDataPrueba();
            var mapper = ConfigurarAutoMapper();
            var contexto = ConstruirContext(nombreDb);

            var controller = new PrestamosController(contexto, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var filtroDTO = new PrestamoFiltroDTO()
            {
                FechaDeCreacion = DateTime.Today.AddDays(5)
            };

            var respuesta = await controller.Filtrar(filtroDTO);
            var prestamos = respuesta.Value;
            Assert.AreEqual(1, prestamos.Count);            
            Assert.AreEqual(DateTime.Today.AddDays(5), prestamos[0].FechaDeCreacion);
        }

        [TestMethod]
        public async Task FiltrarPorCartera()
        {

            var nombreDb = CrearDataPrueba();
            var mapper = ConfigurarAutoMapper();
            var contexto = ConstruirContext(nombreDb);

            var controller = new PrestamosController(contexto, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var carteraId = contexto.Carteras.Select(x => x.Id).First();

            var filtroDTO = new PrestamoFiltroDTO()
            {
                CarteraId = carteraId
                //CarteraId = new Guid("bd35ea7c-bd25-4ce8-89f5-245a72eb6c30")
            };

            var respuesta = await controller.Filtrar(filtroDTO);
            var prestamos = respuesta.Value;
            Assert.AreEqual(1, prestamos.Count);
            Assert.AreEqual("Prestamo4", prestamos[0].PrestamoID);
            //Assert.AreEqual(1, 2);
        }


        [TestMethod]
        public async Task FiltrarOrdenarPrestamoIdAscendente()
        {
            var nombreDb = CrearDataPrueba();
            var mapper = ConfigurarAutoMapper();
            var contexto = ConstruirContext(nombreDb);

            var controller = new PrestamosController(contexto, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();


            var filtroDTO = new PrestamoFiltroDTO()
            {
                TipoDeCampoOrderPrestamos = TipoDeCampoOrderPrestamos.PorPrestamoID,
                TipoDeOrdenacion = TipoDeOrdenacion.Ascendente
                
            };
            var respuesta = await controller.Filtrar(filtroDTO);
            var prestamos = respuesta.Value;


            var context2 = ConstruirContext(nombreDb);
            var prestamosDB = context2.Prestamos.OrderBy(x => x.PrestamoID).ToList();

            Assert.AreEqual(prestamosDB.Count, prestamos.Count);

            for (int i = 0; i < prestamosDB.Count; i++)
            {
                var prestamoDelCotrolador = prestamos[i];
                var prestamoDB = prestamosDB[i];

                Assert.AreEqual(prestamoDB.Id, prestamoDelCotrolador.Id);
                    
            }
        }


        [TestMethod]
            public async Task FiltrarOrdenarPrestamoIdDescendente()
            {
                var nombreDb = CrearDataPrueba();
                var mapper = ConfigurarAutoMapper();
                var contexto = ConstruirContext(nombreDb);

                var controller = new PrestamosController(contexto, mapper, null, null);
                controller.ControllerContext.HttpContext = new DefaultHttpContext();


                var filtroDTO = new PrestamoFiltroDTO()
                {
                    TipoDeCampoOrderPrestamos = TipoDeCampoOrderPrestamos.PorPrestamoID,
                    TipoDeOrdenacion = TipoDeOrdenacion.Descendente

                };
                var respuesta = await controller.Filtrar(filtroDTO);
                var prestamos = respuesta.Value;

            var context2 = ConstruirContext(nombreDb);
            var prestamosDB = context2.Prestamos.OrderByDescending(x => x.PrestamoID).ToList();



            Assert.AreEqual(prestamosDB.Count, prestamos.Count);

                for (int i = 0; i < prestamosDB.Count; i++)
                {
                    var prestamoDelCotrolador = prestamos[i];
                    var prestamoDB = prestamosDB[i];

                    Assert.AreEqual(prestamoDB.Id, prestamoDelCotrolador.Id);

                }


            }

        [TestMethod]
        public async Task FiltrarPorCampoIncorrectoDevuelvePrestmoas()
        {
            var nombreDb = CrearDataPrueba();
            var mapper = ConfigurarAutoMapper();
            var contexto = ConstruirContext(nombreDb);

            var mock = new Mock<ILogger<PrestamosController>>();

            var controller = new PrestamosController(contexto, mapper, null, mock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var filtroDTO = new PrestamoFiltroDTO()
            {
                TipoDeCampoOrderPrestamos = (TipoDeCampoOrderPrestamos)545,
                TipoDeOrdenacion = TipoDeOrdenacion.Descendente

            };
            var respuesta = await controller.Filtrar(filtroDTO);
            var prestamos = respuesta.Value;


            var context2 = ConstruirContext(nombreDb);
            var prestamosDB = context2.Prestamos.ToList();
            Assert.AreEqual(prestamosDB.Count, prestamos.Count);
            Assert.AreEqual(1, mock.Invocations.Count);
        }
    }

}
