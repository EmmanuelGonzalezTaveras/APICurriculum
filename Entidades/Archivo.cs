using Entidades.Interfaces;
using Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Archivo: IId
    {
        public Guid Id { get; set; }
        public TipoDeEntidad TipoDeEntidad { get; set; }
        public Guid EntidadId { get; set; }
        public TipoDeArchivo Tipo { get; set; }
        public string Nombre { get; set; }
    }
}
