using Entidades.Interfaces;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    
    public class Cliente: IId, IFoto
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(60)]
        public string Nombres { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Foto { get; set; }
        public Point Ubicacion { get; set; }



        public List<Email> Emails { get; set; }
    }
}
