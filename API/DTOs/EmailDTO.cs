using Enumeradores;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class EmailDTO : EmailCreacionDTO
    {
        public Guid Id { get; set; }
        //[Required]
        //[StringLength(40)]
        //[EmailAddress]
        //public string email { get; set; }

    }

    public class EmailCreacionDTO
    {
        [Required]
        [StringLength(40)]
        [EmailAddress]        
        public string EmailAddress { get; set; }
        public EstadoDeEmail EmailEstado { get; set; }
        public DateTime FechaCreacion { get; set; }





        public Guid ClienteId { get; set; }
    }
}
