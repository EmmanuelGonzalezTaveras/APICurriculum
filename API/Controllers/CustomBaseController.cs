using API.DTOs;
using API.Helpers;
using API.Servicios;
using AutoMapper;
using Entidades;
using Entidades.Interfaces;
using Enumeradores;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class CustomBaseController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;

        public CustomBaseController(ApplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        public CustomBaseController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        protected async Task<List<TDTO>> Get<TEntidad, TDTO>() where TEntidad : class
        {
            var entidades = await context.Set<TEntidad>().AsNoTracking().ToListAsync();
            var dtos = mapper.Map<List<TDTO>>(entidades);
            return dtos;
        }


        protected async Task<ActionResult<TDTO>> Get<TEntidad, TDTO>(Guid Id) where TEntidad : class, IId
        {
            var entidad = await context.Set<TEntidad>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id);
            if (entidad == null)
            {
                return NotFound();
            }
            return mapper.Map<TDTO>(entidad);

        }

        protected async Task<List<TDTO>> Get<TEntidad, TDTO>(PaginacionDTO paginacionDTO) where TEntidad : class
        {
          
            var queriable = context.Set<TEntidad>().AsQueryable();

            return await Get<TEntidad, TDTO>(paginacionDTO, queriable);

        }





        protected async Task<List<TDTO>> Get<TEntidad, TDTO>(PaginacionDTO paginacionDTO, IQueryable<TEntidad> queryable) where TEntidad : class
        {

            await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidadRegistrosPorPagina);
            var entidades = await queryable.Paginar(paginacionDTO).ToListAsync();

            return mapper.Map<List<TDTO>>(entidades);

        }









        protected async Task<ActionResult> Post<TCreacion, TEntidad, TLectura>(TCreacion creacionDTO, string nombreRuta) where TEntidad : class, IId
        {
            var entidad = mapper.Map<TEntidad>(creacionDTO);
            context.Add(entidad);
            await context.SaveChangesAsync();
            //para mapear la respuesta
            var dtoLectura = mapper.Map<TLectura>(entidad);

            return new CreatedAtRouteResult(nombreRuta, new { id = entidad.Id }, dtoLectura);
        }


        protected async Task<ActionResult> Put<TCreacion, TEntidad>(Guid id, TCreacion creacionDTO) where TEntidad : class, IId
        {
            var entidad = mapper.Map<TEntidad>(creacionDTO);
            entidad.Id = id;
            context.Entry(entidad).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }


        protected async Task<ActionResult> Patch<TEntidad, TDTO>(Guid id, JsonPatchDocument<TDTO> patchDocument) 
            where TDTO: class 
            where TEntidad: class, IId
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var entidadDB = await context.Set<TEntidad>().FirstOrDefaultAsync(x => x.Id == id);

            if (entidadDB == null)
            {
                return NotFound();
            }

            var entidadDTO = mapper.Map<TDTO>(entidadDB);

            patchDocument.ApplyTo(entidadDTO, ModelState);


            var esValido = TryValidateModel(entidadDTO);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(entidadDTO, entidadDB);

            await context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Metodo generico para borrar cualquier entidad enviada.
        /// </summary>
        /// <typeparam name="TEntidad">El tipo de Entidad que se va a borrar.</typeparam>
        /// <param name="id">El Id de la entidad a borrar.</param>
        /// <returns>NoContent()</returns>
        protected async Task<ActionResult> Delete<TEntidad>(Guid id) where TEntidad : class, IId, new()
        {
            var existe = await context.Set<TEntidad>().AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new TEntidad() { Id = id });

            await context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Esta sobrecaga se debe usar cuando tambien se van a borrar documentos adjuntos guardados en del disco.
        /// </summary>
        /// <typeparam name="TEntidad">El tipo de Entidad que se va a borrar.</typeparam>
        /// <param name="id"> El Id de la entidad a borrar.</param>
        /// <param name="tipoDeContendor"> El nombre del contenedor donde se encuenta el archivo a borrar</param>
        /// <returns>NoContent()</returns>
        protected async Task<ActionResult> Delete<TEntidad>(Guid id, TipoDeContendor tipoDeContendor) where TEntidad : class, IId, IFoto, new()
        {
            var existe = await context.Set<TEntidad>().AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }
            var entidadDB = await context.Set<TEntidad>().FirstOrDefaultAsync(x => x.Id == id);

            context.Remove(new TEntidad() { Id = id });

            await context.SaveChangesAsync();
            await almacenadorArchivos.BorrarArchivo(entidadDB.Foto, tipoDeContendor);
            return NoContent();
        }


    }
}
