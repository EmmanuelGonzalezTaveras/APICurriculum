using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertarParametrosPaginacion<T>(this HttpContext httpContext, IQueryable<T> queryable, int cantidadRegistroPorPagina)
        {
            double cantidad = await queryable.CountAsync();
            double cantidadPaginas = Math.Ceiling(cantidad / cantidadRegistroPorPagina);

            httpContext.Response.Headers.Add("cantidadPaginas", cantidadPaginas.ToString());

        }
    }
}
