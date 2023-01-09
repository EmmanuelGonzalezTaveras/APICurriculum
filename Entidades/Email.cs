using Entidades.Interfaces;
using Enumeradores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Email: IId
    {
        //public int id { get; set; }
        public Guid Id { get; set; }
        [Required]
        [StringLength(40)]
        [EmailAddress]
        public string EmailAddress { get; set; }
        public EstadoDeEmail EmailEstado { get; set; }
        public DateTime FechaCreacion { get; set; }


        public Guid ClienteId { get; set; }
        //public Cliente ClienteEmail { get; set; }
    }
}
