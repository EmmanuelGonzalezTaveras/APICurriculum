using API.DTOs;
using API.DTOs.Seguimientos;
using API.Helpers.Attributes;
using API.Helpers;
using API.Servicios;
using AutoMapper;
using Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/prestamos/{prestamoId:guid}/seguimientos")]
    [ServiceFilter(typeof(PrestamoExisteAttribute))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SeguimientoController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public SeguimientoController(
            ApplicationDbContext context,
            IMapper mapper,
            IAlmacenadorArchivos almacenadorArchivos) : base(context, mapper, almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<SeguimientoDTO>>> Get(Guid prestamoId, [FromQuery] PaginacionDTO paginacionDTO)
        {
         
            var queriable = context.Seguimientos.Include(x => x.Usuario).AsQueryable();
            queriable = queriable.Where(x => x.PrestamoId == prestamoId);
            return await Get<Seguimiento, SeguimientoDTO>(paginacionDTO, queriable);
        }



        [HttpPost]      
        public async Task<ActionResult> Post(Guid prestamoId, [FromBody] SeguimientoCreacionDTO seguimientoCreacionDTO)
        {

            var usuarioId = new Guid(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var seguimientoExiste = await context.Seguimientos
                .AnyAsync(x => x.PrestamoId == prestamoId && x.UsuarioId == usuarioId);
            if (seguimientoExiste)
            {
                return BadRequest("El usuario ya escribio un seguimiento para este prestamo");
            }





            var seguimiento = mapper.Map<Seguimiento>(seguimientoCreacionDTO);

            seguimiento.PrestamoId = prestamoId;
            seguimiento.UsuarioId = usuarioId;

            context.Add(seguimiento);
            await context.SaveChangesAsync();

            return NoContent();                    

        }

        [HttpPut("{seguimientoId:guid}")]
        public async Task<ActionResult> Put(Guid prestamoId, Guid seguimientoId, [FromBody] SeguimientoCreacionDTO seguimientoCreacionDTO)
        {
            var seguimientoDB = await context.Seguimientos.FirstOrDefaultAsync(x => x.Id == seguimientoId);

            if (seguimientoDB == null) { return NotFound(); }

            var usuarioId = new Guid(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            if(seguimientoDB.UsuarioId != usuarioId)
            {
                return BadRequest("No tiene permisos de editar este seguimiento"); 
            }

            seguimientoDB = mapper.Map(seguimientoCreacionDTO, seguimientoDB); //pasando los datos que se actualizacon en seguimientoCreacionDTO

            await context.SaveChangesAsync();

            return NoContent();

        }
        [HttpDelete("{seguimientoId:guid}")]
        public async Task<ActionResult> Delete (Guid seguimientoId)
        {
            var seguimientoDB = await context.Seguimientos.FirstOrDefaultAsync(x => x.Id == seguimientoId);

            if (seguimientoDB == null) { return NotFound(); }

            var usuarioId = new Guid(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            if (seguimientoDB.UsuarioId != usuarioId) { return Forbid(); }

            context.Remove(seguimientoDB);
            await context.SaveChangesAsync();
            return NoContent();

        }

    }
}
