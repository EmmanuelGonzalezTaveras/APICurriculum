using API.DTOs;
using API.Helpers;
using API.Servicios;
using AutoMapper;
using Entidades;
using Enumeradores;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace API.Controllers
{
    [ApiController]
    [Route("api/prestamos")]
    public class PrestamosController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly ILogger logger;
      
       

        public PrestamosController(ApplicationDbContext context, 
            IMapper mapper, 
            IAlmacenadorArchivos almacenadorArchivos, 
            ILogger<PrestamosController> logger): base(context,mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
            this.logger = logger;
        }

     

        [HttpGet]
        public async Task<ActionResult<PrestamosIndexDTO>> Get()
        {
            var top = 5;
            var hoy = DateTime.Today;

            var masRecientes = await context.Prestamos
                .Where(x => x.FechaDeCreacion < hoy)
                .OrderBy(x => x.FechaDeCreacion)
                .Take(top)
                .ToListAsync();

            var activos = await context.Prestamos
                .Where(x => x.Estado == EstadoDePrestamo.Activo)
                .Take(top)
                .ToListAsync();

            var resultado = new PrestamosIndexDTO();
            resultado.MasRecientes = mapper.Map<List<PrestamoDTO>>(masRecientes);
            resultado.Activos = mapper.Map<List<PrestamoDTO>>(activos);
            return resultado;
        }

        [HttpGet("filtro")]
        public async Task<ActionResult<List<PrestamoDTO>>> Filtrar([FromQuery] PrestamoFiltroDTO prestamoFiltroDTO)
        {

            var prestamosQueryable = context.Prestamos.AsQueryable();

           

            if (prestamoFiltroDTO.Id.HasValue)
            {
                prestamosQueryable = prestamosQueryable.Where(x => x.Id == prestamoFiltroDTO.Id);
            }



            if (prestamoFiltroDTO.CarteraId.HasValue)
            {

                prestamosQueryable = prestamosQueryable
                   .Include(a => a.PrestamosCarteras
                   .Where(x => x.CarteraId == prestamoFiltroDTO.CarteraId))
                   .Where(x => x.PrestamosCarteras.Count > 0).AsQueryable();

            }


            if (!string.IsNullOrEmpty(prestamoFiltroDTO.PrestamoId))
            {
                prestamosQueryable = prestamosQueryable.Where(x => x.PrestamoID.Contains(prestamoFiltroDTO.PrestamoId));
            }

            if (prestamoFiltroDTO.Estado.HasValue)
            {
                prestamosQueryable = prestamosQueryable.Where(x => x.Estado == prestamoFiltroDTO.Estado);
            }


            if (prestamoFiltroDTO.FechaDeCreacion.HasValue)
            {
                prestamosQueryable = prestamosQueryable.Where(x => x.FechaDeCreacion == prestamoFiltroDTO.FechaDeCreacion);
            }


            try
            {
                if (prestamoFiltroDTO.TipoDeCampoOrderPrestamos.HasValue)
                {             
                    var tipoDeOrdenacion = (prestamoFiltroDTO.TipoDeOrdenacion == TipoDeOrdenacion.Ascendente) ? "ascending" : "descending";

                    var tipoDeCampoOrderPrestamos = Enum.GetName(typeof(TipoDeCampoOrderPrestamos), prestamoFiltroDTO.TipoDeCampoOrderPrestamos).Remove(0,3);

                    prestamosQueryable = prestamosQueryable.OrderBy($"{tipoDeCampoOrderPrestamos} {tipoDeOrdenacion}");

                
                                       
                    
                }

            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message, ex, "sadf");
            }
         








            await HttpContext.InsertarParametrosPaginacion(prestamosQueryable, prestamoFiltroDTO.CantidadRegistrosPorPagina);

            var prestamos = await prestamosQueryable.Paginar(prestamoFiltroDTO.PaginacionDTO).ToListAsync();
          

            return mapper.Map<List<PrestamoDTO>>(prestamos);
        }











        [HttpGet("{id}", Name = "obtenerPrestamo")]
        public async Task<ActionResult<PrestamoDTO>> Get(Guid id)
        {
            var prestamo = await context.Prestamos.FirstOrDefaultAsync(x => x.Id == id);

            if (prestamo == null)
            {
                return NotFound();
            }

            return mapper.Map<PrestamoDTO>(prestamo);

        }

        
        [HttpGet("obtenerPrestmoPorPrestmoID/{prestamoid}", Name = "obtenerPrestamoPorPrestamoId")]
        
        public async Task<ActionResult<PrestamoDTO>> GetPrestamoID(string prestamoid)
        {
            var prestamo = await context.Prestamos.FirstOrDefaultAsync(x => x.PrestamoID == prestamoid);

            if (prestamo == null)
            {
                return NotFound();
            }

            return mapper.Map<PrestamoDTO>(prestamo);

        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] PrestamoCreacionDTO prestamoCreacionDTO)
        {
            var prestamo = mapper.Map<Prestamo>(prestamoCreacionDTO);

            if (prestamoCreacionDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await prestamoCreacionDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(prestamoCreacionDTO.Foto.FileName);
                    prestamo.Foto = await almacenadorArchivos.GuardarArchivo(contenido, extension, TipoDeContendor.Prestamos, prestamoCreacionDTO.Foto.ContentType);
                }
            }


            context.Add(prestamo);
            await context.SaveChangesAsync();
            var prestamoDTO = mapper.Map<PrestamoDTO>(prestamo);
            return new CreatedAtRouteResult("obtenerPrestamo", new { id = prestamo.Id }, prestamoDTO);

        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromForm] PrestamoCreacionDTO prestamoCreacionDTO)
        {

            var prestamoDB = await context.Prestamos.FirstOrDefaultAsync(x => x.Id == id);

            if (prestamoDB == null) { return NotFound(); }

            prestamoDB = mapper.Map(prestamoCreacionDTO, prestamoDB);

            //para guardar la rusta de la foto en la base de datos y la foto en el wwwroot
            if (prestamoCreacionDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await prestamoCreacionDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(prestamoCreacionDTO.Foto.FileName);
                    prestamoDB.Foto = await almacenadorArchivos.EditarArchivo(contenido, extension, TipoDeContendor.Prestamos, prestamoDB.Foto, prestamoCreacionDTO.Foto.ContentType);
                }
            }


            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<PrestamoPatchDTO> patchDocument)
        {
            return await Patch<Prestamo, PrestamoPatchDTO>(id, patchDocument);

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {

            return await Delete<Prestamo>(id, TipoDeContendor.Prestamos);

        }

    }
}
