using Entidades.Interfaces;
using Enumeradores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Prestamo: IId, IFoto
    {
        public Guid Id { get; set; }        
        [StringLength(20)]
        public string PrestamoID { get; set; }        
        public DateTime FechaDeCreacion { get; set; }
        public EstadoDePrestamo Estado { get; set; }
        public string Foto { get; set; }

        public List<PrestamosCarteras> PrestamosCarteras { get; set; }
    }
}
