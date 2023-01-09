using Enumeradores;

namespace API.Servicios
{
    public interface IAlmacenadorArchivos
    {
        Task<string> EditarArchivo(byte[] contenido, string extension, TipoDeContendor tipoDeContendor, string ruta, string contentType);

        Task BorrarArchivo(string ruta, TipoDeContendor tipoDeContendor);

        Task<string> GuardarArchivo(byte[] contenido, string extension, TipoDeContendor tipoDeContendor, string contentType);
    }
}
