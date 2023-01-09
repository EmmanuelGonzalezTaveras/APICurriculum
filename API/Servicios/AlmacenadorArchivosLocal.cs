using Enumeradores;

namespace API.Servicios
{
    public class AlmacenadorArchivosLocal : IAlmacenadorArchivos
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AlmacenadorArchivosLocal(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }




        private string CambioTipoDeContenderoAString (TipoDeContendor tipoDeContendor)
        {
            if (tipoDeContendor == TipoDeContendor.Clientes)
            {
                return "Clientes";
            }
            if (tipoDeContendor == TipoDeContendor.Prestamos)
            {
                return "Prestamos";
            }

            return null;
        }


        public Task BorrarArchivo(string ruta, TipoDeContendor tipoDeContendor)
        {
            var contenedor = CambioTipoDeContenderoAString(tipoDeContendor);

            if (ruta != null)
            {
                var nombreArchivo = Path.GetFileName(ruta);
                string directorioArchivo = Path.Combine(env.WebRootPath, contenedor, nombreArchivo);

                if (File.Exists(directorioArchivo))
                {
                    File.Delete(directorioArchivo);
                }
            }

            return Task.FromResult(0);
        }


        public async Task<string> EditarArchivo(byte[] contenido, string extension, TipoDeContendor tipoDeContendor, string ruta, string contentType)
        {
            
            await BorrarArchivo(ruta, tipoDeContendor);
            return await GuardarArchivo(contenido, extension, tipoDeContendor, contentType);

        }




        public async Task<string> GuardarArchivo(byte[] contenido, string extension, TipoDeContendor TipoDeContendor, string contentType)
        {
            var contenedor = CambioTipoDeContenderoAString(TipoDeContendor);

            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(env.WebRootPath, contenedor);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string ruta = Path.Combine(folder, nombreArchivo);
            await File.WriteAllBytesAsync(ruta, contenido);

            var urlActual = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            var urlParaBD = Path.Combine(urlActual, contenedor, nombreArchivo).Replace("\\", "/");

            return urlParaBD;
        }







    }
}
