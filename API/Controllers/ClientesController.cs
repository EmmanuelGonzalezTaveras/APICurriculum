using API.Helpers;
using API.Servicios;
using AutoMapper;
using API.DTOs;
using Entidades;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Enumeradores;
using Microsoft.OpenApi.Extensions;
using NetTopologySuite.Geometries;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClientesController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly GeometryFactory geometryFactory;

        public ClientesController(
            ApplicationDbContext context, 
            IMapper mapper, 
            IAlmacenadorArchivos almacenadorArchivos,
            GeometryFactory geometryFactory): base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
            this.geometryFactory = geometryFactory;
        }


        [HttpGet]
        public async Task<ActionResult<List<ClienteDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            return await Get<Cliente, ClienteDTO>(paginacionDTO);
        }

        [HttpGet("Cercano")]
        public async Task<ActionResult<List<ClienteCercanoDTO>>> Cercanos([FromQuery] ClienteCercanoFiltroDTO filtro)
        {
            var UbicacionUsuario = geometryFactory.CreatePoint(new Coordinate(filtro.Longitud, filtro.Latitud));

            var clientes = await context.Clientes
                .OrderBy(x => x.Ubicacion.Distance(UbicacionUsuario))
                .Where(x => x.Ubicacion.IsWithinDistance(UbicacionUsuario, filtro.DistanciaEnKms * 1000))
                .Select(x => new ClienteCercanoDTO
                {
                    Id = x.Id,
                    Nombres = x.Nombres,
                    FechaNacimiento = x.FechaNacimiento,
                    Foto = x.Foto,
                    Latitud = x.Ubicacion.Y,
                    Longitud = x.Ubicacion.X,
                    DistanciaEnMetros = Math.Round(x.Ubicacion.Distance(UbicacionUsuario))
                })
                .ToListAsync();

            return clientes;

        }






        [HttpGet("{id}", Name = "obtenerCliente")]
        public async Task<ActionResult<ClienteDTO>> Get(Guid id)
        {
            // Sin CustomBaseController
            var entidad = await context.Clientes
                .Include(x => x.Emails)
                .FirstOrDefaultAsync(x => x.Id == id);
            {
                if (entidad == null) { return NotFound(); }

                return mapper.Map<ClienteDTO>(entidad);
            }

            //Con CustomBaseController
            // return await Get<Cliente, ClienteDTO>(id);
        }



        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ClienteCreacionDTO clienteCreacionDTO)
        {
            var entidad = mapper.Map<Cliente>(clienteCreacionDTO);

            //return Ok();

            //para guardar la rusta de la foto en la base de datos y la foto en el wwwroot
            if (clienteCreacionDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await clienteCreacionDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(clienteCreacionDTO.Foto.FileName);
                    entidad.Foto = await almacenadorArchivos.GuardarArchivo(contenido, extension, TipoDeContendor.Clientes, clienteCreacionDTO.Foto.ContentType);
                }
            }


            context.Add(entidad);
            await context.SaveChangesAsync();

            var dto = mapper.Map<ClienteDTO>(entidad);
            return new CreatedAtRouteResult("obtenerCliente", new { id = entidad.Id }, dto);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromForm] ClienteCreacionDTO clienteCreacionDTO)
        {
            
            var clienteDB = await context.Clientes.FirstOrDefaultAsync(x => x.Id == id);

            if(clienteDB == null) { return NotFound(); }

            clienteDB = mapper.Map(clienteCreacionDTO, clienteDB);


            //para guardar la rusta de la foto en la base de datos y la foto en el wwwroot
            if (clienteCreacionDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await clienteCreacionDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(clienteCreacionDTO.Foto.FileName);
                    clienteDB.Foto = await almacenadorArchivos.EditarArchivo(contenido, extension, TipoDeContendor.Clientes,clienteDB.Foto, clienteCreacionDTO.Foto.ContentType);
                }
            }

            await context.SaveChangesAsync();
            return NoContent();

        }


         
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(Guid id, [FromBody] JsonPatchDocument <ClientePatchDTO> patchDocument)
        {
          
            return await Patch<Cliente, ClientePatchDTO>(id, patchDocument);
        }





        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
          
            return await Delete<Cliente>(id, TipoDeContendor.Clientes);

        }

    }
}
