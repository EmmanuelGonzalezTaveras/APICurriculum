using Microsoft.AspNetCore.Identity;

namespace API.DTOs.Seguimientos
{
    public class SeguimientoCreacionDTO
    {

        public Guid PrestamoId { get; set; }
        public Guid ClienteId { get; set; }
        public DateTime Fecha { get; set; }
        public string Anotacion { get; set; }
        public bool SeguimientoPendiente { get; set; }
        public DateTime FechaDeSeguimiento { get; set; }
        //public Guid UsuarioId { get; set; } no debe ponerse porque no se puede identificar al cliente cual es su usuario
    }
}
