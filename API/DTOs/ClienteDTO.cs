using API.Helpers;
using API.DTOs;
using Entidades.Validaciones;
using Enumeradores;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class ClienteDTO
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(60)]
        public string Nombres { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Foto { get; set; }

        [Range(-90, 90)]
        public double Latitud { get; set; }
        [Range(-180, 180)]
        public double Longitud { get; set; }

        public List<EmailDTO> Emails { get; set; }
    }

    public class ClienteCreacionDTO
    {
        [Required]
        [StringLength(60)]
        public string Nombres { get; set; }
        public DateTime FechaNacimiento { get; set; }
        [PesoArchivoValidacion(PesoMaximoEnMagabytes: 4)]
        [TipoArchivoValidacion(tipoArchivo: TipoArchivo.Imagen)]
        public IFormFile Foto { get; set; }

        [Range(-90, 90)]
        public double Latitud { get; set; }
        [Range(-180, 180)]
        public double Longitud { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<List<EmailCreacionDTO>>))]        
        public List<EmailCreacionDTO> Emails { get; set; }
    }

    public class ClientePatchDTO
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(60)]
        public string Nombres { get; set; }
        public DateTime FechaNacimiento { get; set; }

        [Range(-90,90)]
        public double Latitud { get; set; }
        [Range(-180,180)]
        public double Longitud { get; set; }
    }

    public class ClienteCercanoFiltroDTO
    {
        [Range(-90, 90)]
        public double Latitud { get; set; }
        [Range(-180, 180)]
        public double Longitud { get; set; }
        private int distanciaEnKms = 10;
        private int distanciaMaximaKms = 5000000;
        public int DistanciaEnKms
        {
            get
            {
                return distanciaEnKms;
            }
            set
            {
                distanciaEnKms = (value > distanciaMaximaKms) ? distanciaMaximaKms : value;
            }
        }

    }

    public class ClienteCercanoDTO: ClienteDTO
    {
        public double DistanciaEnMetros { get; set; }
    }



}
