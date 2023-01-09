using Entidades.Interfaces;
using Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Cartera: IId
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public EstadoDeCartera Estado { get; set; }
        public List<PrestamosCarteras> PrestamosCartera { get; set; }
    }
}
