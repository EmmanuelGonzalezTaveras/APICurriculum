using System.ComponentModel;
using System.Runtime.Serialization;

namespace Enumeradores
{
    public class Tipos
    {

    }


    public enum TipoArchivo
    {
        Imagen = 1
    }


    public enum TipoDeDeudor
    {
        Deudor = 1,
        Garante = 2

    }
    public enum TipoDeCampoOrderPrestamos
    {
        PorPrestamoID = 1,
        PorFechaDeCreacion = 2


    }
    public enum TipoDeOrdenacion
    {
        Ascendente = 1,
        Descendente = 2
    }

    #region "Iguales"


    public enum TipoDeContendor
    {
        Clientes = 1,
        Prestamos = 2,
    }

    public enum TipoDeEntidad
    {
        Cliente = 1,
        Prestamos = 2
    }

    #endregion

    public enum TipoDeArchivo
    {
        Cedula = 1,
        Foto = 2,

    }


}