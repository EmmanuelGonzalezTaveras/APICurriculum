using Entidades;
using Entidades.Validaciones;
using Enumeradores;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class PrestamoDTO
    {
        public Guid Id { get; set; }
        [StringLength(20)]
        public string PrestamoID { get; set; }
        public DateTime FechaDeCreacion { get; set; }
        public EstadoDePrestamo Estado { get; set; }
        public string Foto { get; set; }

        public List<PrestamosCarteras> PrestamosCarteras { get; set; }
    }


    public class PrestamoCreacionDTO
    {
        // public Guid Id { get; set; }
        [StringLength(20)]
        public string PrestamoID { get; set; }
        public DateTime FechaDeCreacion { get; set; }
        public EstadoDePrestamo Estado { get; set; }
        [PesoArchivoValidacion(PesoMaximoEnMagabytes: 4)]
        [TipoArchivoValidacion(tipoArchivo: TipoArchivo.Imagen)]
        public IFormFile Foto { get; set; }
    }

    public class PrestamoPatchDTO
    {
        public Guid Id { get; set; }
        [StringLength(20)]
        public string PrestamoID { get; set; }
        public EstadoDePrestamo Estado { get; set; }
        public DateTime FechaDeCreacion { get; set; }

    }

    public class PrestamosIndexDTO
    {
        public List<PrestamoDTO> MasRecientes { get; set; }
        public List<PrestamoDTO> Activos { get; set; }
    }




    public class PrestamoFiltroDTO
    {
        public int Pagina { get; set; } = 1;
        public int CantidadRegistrosPorPagina { get; set; } = 10;
        public PaginacionDTO PaginacionDTO
        {
            get { return new PaginacionDTO()
            { 
                Pagina = Pagina, 
                CantidadRegistrosPorPagina = CantidadRegistrosPorPagina 
            }; }
        }
        public Guid? CarteraId { get; set; }
        public Guid? Id { get; set; }
        public string? PrestamoId { get; set; }        
        public DateTime? FechaDeCreacion { get; set; }
        public EstadoDePrestamo? Estado { get; set; }

        public TipoDeCampoOrderPrestamos? TipoDeCampoOrderPrestamos { get; set; }
        public TipoDeOrdenacion? TipoDeOrdenacion { get; set; } = Enumeradores.TipoDeOrdenacion.Ascendente;// TipoDeOrdenacion.Ascendente;


    }
}
