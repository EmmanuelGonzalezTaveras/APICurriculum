using Entidades;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Helpers.Attributes
{
    public class PrestamoExisteAttribute : Attribute, IAsyncResourceFilter
    {
        private readonly ApplicationDbContext Dbcontext;

        public PrestamoExisteAttribute(ApplicationDbContext context)
        {
            this.Dbcontext = context;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context,
            ResourceExecutionDelegate next)
        {
            var prestamoIdObject = context.HttpContext.Request.RouteValues["prestamoId"];

            if (prestamoIdObject == null)
            {
                return;
            }

            var prestamoId = Guid.Parse(prestamoIdObject.ToString());

            var existePelicula = await Dbcontext.Prestamos.AnyAsync(x => x.Id == prestamoId);

            if (!existePelicula)
            {
                context.Result = new NotFoundResult();
            }
            else
            {
                await next();
            }
        }
    }
}
