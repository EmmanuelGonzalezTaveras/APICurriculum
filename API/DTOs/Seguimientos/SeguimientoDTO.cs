using Microsoft.AspNetCore.Identity;

namespace API.DTOs.Seguimientos
{
    public class SeguimientoDTO
    {

        public Guid Id { get; set; }
        public Guid PrestamoId { get; set; }
        public Guid ClienteId { get; set; }
        public DateTime Fecha { get; set; }
        public string Anotacion { get; set; }
        public bool SeguimientoPendiente { get; set; }
        public DateTime FechaDeSeguimiento { get; set; }
        public Guid UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
    }
}
