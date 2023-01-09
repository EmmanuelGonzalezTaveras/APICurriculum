using Entidades.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Seguimiento: IId
    {
        public Guid Id { get; set; }
        public Guid PrestamoId { get; set; }
        public Guid ClienteId { get; set; }
        public DateTime Fecha { get; set; }
        public string Anotacion { get; set; }
        public bool SeguimientoPendiente { get; set; }
        public DateTime FechaDeSeguimiento { get; set; }
        public Guid UsuarioId { get; set; }
        public IdentityUser Usuario { get; set; }
    }
}
