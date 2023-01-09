namespace API.DTOs
{
    public class PaginacionDTO
    {
        private int cantidadRegistrosPorPagina = 10;
        private readonly int cantidadMaximaRegistrosPorPagina = 50;

        public int Pagina { get; set; } = 1;
        public int CantidadRegistrosPorPagina
        {
            get => cantidadRegistrosPorPagina;
            set
            {
                cantidadRegistrosPorPagina = (value > cantidadMaximaRegistrosPorPagina) ? cantidadMaximaRegistrosPorPagina : value;

                //es lo mismo de lo arriba pero mas sencillo, un if pero que solo tiene dos opciones para una misma variabloe
                //if (value > cantidadMaximaRegistrosPorPagina)
                //{
                //    cantidadRegistrosPorPagina = cantidadMaximaRegistrosPorPagina;
                //}
                //else
                //{
                //    cantidadRegistrosPorPagina = value;
                //}
            }
        }

    }
}
