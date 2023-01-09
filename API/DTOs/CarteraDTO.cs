using Enumeradores;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class CarteraDTO
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public EstadoDeCartera Estado { get; set; }
    }

    public class CarteraCreacionDTO
    {
        //public Guid Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; }
        public EstadoDeCartera Estado { get; set; }
    }
}
