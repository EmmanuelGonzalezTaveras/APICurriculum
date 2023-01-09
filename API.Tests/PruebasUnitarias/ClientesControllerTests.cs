using API.DTOs;
using API.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using API.Controllers;
using Moq;
using Enumeradores;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch.Operations;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace API.Tests.PruebasUnitarias
{
    [TestClass]

    public class ClientesControllerTests: BasePruebas
    {
        [TestMethod]
        public async Task ObtenerClientesPaginados()
        {
            var nombreBD = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreBD);
            var mapper = ConfigurarAutoMapper();

            contexto.Clientes.Add(new Cliente() { Nombres = "Cliente 1" });
            contexto.Clientes.Add(new Cliente() { Nombres = "Cliente 2" });
            contexto.Clientes.Add(new Cliente() { Nombres = "Cliente 3" });
            await contexto.SaveChangesAsync();

            var contexto2 = ConstruirContext(nombreBD);

            var controller = new ClientesController(contexto2, mapper, null,null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var pagina1 = await controller.Get(new PaginacionDTO() { Pagina = 1, CantidadRegistrosPorPagina = 2 });
            var clientesPagina1 = pagina1.Value;
            Assert.AreEqual(2, clientesPagina1.Count);

            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var pagina2 = await controller.Get(new PaginacionDTO() { Pagina = 2, CantidadRegistrosPorPagina = 2 });
            var actoresPagina2 = pagina2.Value;
            Assert.AreEqual(1, actoresPagina2.Count);

            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var pagina3 = await controller.Get(new PaginacionDTO() { Pagina = 3, CantidadRegistrosPorPagina = 2 });
            var actoresPagina3 = pagina3.Value;
            Assert.AreEqual(0, actoresPagina3.Count);
        }



        //Solo utilizar en Windows LocalDbDataBaseInitializer solo funcionen Windows, no en mac
        //[TestMethod]
        //public async Task ObtenerClientesA5Kilometros()
        //{
        //    var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

        //    using (var context = LocalDbDatabaseInitializer.GetDbContextLocalDb(false))
        //    {
        //        var clientes = new List<Cliente>()
        //        {
        //            new Cliente
        //            {
        //                Id = new Guid("867abb74-ea36-4ab6-8d7a-84ee60377ed8"),
        //                Nombres = "Emmanuel",
        //                Ubicacion = geometryFactory.CreatePoint(new Coordinate(-70.491848, 19.422422))
        //            },
        //            new Cliente
        //            {
        //                Id = new Guid("5f7c3904-444b-48ad-b659-c5c78b4cd464"),
        //                Nombres = "Dandy Gas",
        //                Ubicacion = geometryFactory.CreatePoint(new Coordinate(-70.4935142, 19.4208089))
        //            }
        //            ,
        //            new Cliente
        //            {
        //                Id = new Guid("728d976d-5d62-46fb-9d94-29f4bbcd044a"),
        //                Nombres = "Marlen",
        //                Ubicacion = geometryFactory.CreatePoint(new Coordinate(-70.4930522, 19.4239908))
        //            }
        //             ,
        //            new Cliente
        //            {
        //                Id = new Guid("0c6a5e70-7358-45bb-8b17-8ffb7e55edb0"),
        //                Nombres = "Ana Felicia",
        //                Ubicacion = geometryFactory.CreatePoint(new Coordinate(-81.5604586, 28.5604586))
        //            }
        //        };

        //        context.AddRange(clientes);
        //        await context.SaveChangesAsync();
        //    }

        //    var filtro = new ClienteCercanoFiltroDTO()
        //    {
        //        DistanciaEnKms = 5,
        //        Latitud = 19.4178829,
        //        Longitud = -70.4968528

        //    };
        //    using (var context = LocalDbDatabaseInitializer.GetDbContextLocalDb(false))
        //    {
        //        var mapper = ConfigurarAutoMapper();
        //        var controller = new ClientesController(context, mapper, null, geometryFactory);
        //        var respuesta = await controller.Cercanos(filtro);
        //        var valor = respuesta.Value;
        //        Assert.AreEqual(3, valor.Count);

        //    }


        //}



        [TestMethod]
        public async Task CrearClienteSinFoto()
        {
            var nombreBD = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreBD);
            var mapper = ConfigurarAutoMapper();

            var actor = new ClienteCreacionDTO() { Nombres = "Felipe", FechaNacimiento = DateTime.Now };

            var mock = new Mock<IAlmacenadorArchivos>();
            mock.Setup(x => x.GuardarArchivo(null, null, TipoDeContendor.Clientes, null))
                .Returns(Task.FromResult("url"));

            var controller = new ClientesController(contexto, mapper, mock.Object,null);
            var respuesta = await controller.Post(actor);
            var resultado = respuesta as CreatedAtRouteResult;
            Assert.AreEqual(201, resultado.StatusCode);

            var contexto2 = ConstruirContext(nombreBD);
            var listado = await contexto2.Clientes.ToListAsync();
            Assert.AreEqual(1, listado.Count);
            Assert.IsNull(listado[0].Foto);

            Assert.AreEqual(0, mock.Invocations.Count);
        }

        [TestMethod]
        public async Task CrearActorConFoto()
        {
            var nombreBD = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreBD);
            var mapper = ConfigurarAutoMapper();

            var content = Encoding.UTF8.GetBytes("Imagen de prueba");
            var archivo = new FormFile(new MemoryStream(content), 0, content.Length, "Data", "imagen.jpg");
            archivo.Headers = new HeaderDictionary();
            archivo.ContentType = "image/jpg";

            var cliente = new ClienteCreacionDTO()
            {
                Nombres = "nuevo cliente",
                FechaNacimiento = DateTime.Now,
                Foto = archivo
            };

            var mock = new Mock<IAlmacenadorArchivos>();
            mock.Setup(x => x.GuardarArchivo(content, ".jpg", TipoDeContendor.Clientes, archivo.ContentType))
                .Returns(Task.FromResult("url"));

            var controller = new ClientesController(contexto, mapper, mock.Object,null);
            var respuesta = await controller.Post(cliente);
            var resultado = respuesta as CreatedAtRouteResult;
            Assert.AreEqual(201, resultado.StatusCode);

            var contexto2 = ConstruirContext(nombreBD);
            var listado = await contexto2.Clientes.ToListAsync();
            Assert.AreEqual(1, listado.Count);
            Assert.AreEqual("url", listado[0].Foto);
            Assert.AreEqual(1, mock.Invocations.Count);
        }

        [TestMethod]
        public async Task PatchRetorna404SiActorNoExiste()
        {
            var nombreBD = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreBD);
            var mapper = ConfigurarAutoMapper();

            var controller = new ClientesController(contexto, mapper, null,null);
            var patchDoc = new JsonPatchDocument<ClientePatchDTO>();
            var respuesta = await controller.Patch(new Guid(), patchDoc);
            var resultado = respuesta as StatusCodeResult;
            Assert.AreEqual(404, resultado.StatusCode);
        }

        [TestMethod]
        public async Task PatchActualizaUnSoloCampo()
        {
            var nombreBD = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreBD);
            var mapper = ConfigurarAutoMapper();

            var fechaNacimiento = DateTime.Now;
            var cliente = new Cliente() { Id = new Guid("0437cd92-ad57-4434-a9f7-3ef953577244"), Nombres = "Felipe", FechaNacimiento = fechaNacimiento };
            contexto.Add(cliente);
            await contexto.SaveChangesAsync();

            var contexto2 = ConstruirContext(nombreBD);
            var controller = new ClientesController(contexto2, mapper, null, null);

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(x => x.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<object>()));


            controller.ObjectValidator = objectValidator.Object;

            var patchDoc = new JsonPatchDocument<ClientePatchDTO>();
            patchDoc.Operations.Add(new Operation<ClientePatchDTO>("replace", "/nombres", null, "Claudia"));
            var respuesta = await controller.Patch(new Guid("0437cd92-ad57-4434-a9f7-3ef953577244"), patchDoc);
            var resultado = respuesta as StatusCodeResult;
            Assert.AreEqual(204, resultado.StatusCode);

            var contexto3 = ConstruirContext(nombreBD);
            var clienteDB = await contexto3.Clientes.FirstAsync();
            Assert.AreEqual("Claudia", clienteDB.Nombres);
            Assert.AreEqual(fechaNacimiento, clienteDB.FechaNacimiento);
        }
    }
}
