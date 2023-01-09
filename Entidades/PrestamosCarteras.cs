using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class PrestamosCarteras
    {
        public Guid PrestamoId { get; set; }
        public Guid CarteraId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Prestamo Prestamo { get; set; }
        public Cartera Cartera { get; set; }
    }
}
