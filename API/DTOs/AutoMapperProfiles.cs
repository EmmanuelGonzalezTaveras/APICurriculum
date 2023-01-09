using AutoMapper;
using API.DTOs;
using Entidades;
using Enumeradores;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.AspNetCore.Identity;
using API.DTOs.Cuentas;
using API.DTOs.Seguimientos;

namespace API.DTOs
{
    public class AutoMapperProfiles : Profile
    {
        private readonly GeometryFactory geometryFactory;

        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            CreateMap<Email, EmailDTO>().ReverseMap();
            CreateMap<EmailCreacionDTO, Email>();




            CreateMap<Seguimiento, SeguimientoDTO>()
                .ForMember(x => x.NombreUsuario, x => x.MapFrom(y => y.Usuario.UserName));
            CreateMap<SeguimientoDTO, Seguimiento>();
            CreateMap<SeguimientoCreacionDTO, Seguimiento>();





            CreateMap<Cartera, CarteraDTO>().ReverseMap();
            CreateMap<CarteraCreacionDTO, Cartera>();

            

            CreateMap<IdentityUser, UsuarioDTO>();



#region "Map de Clientes"

            //CreateMap<Cliente, ClienteDTO>().ReverseMap();
            CreateMap<Cliente, ClienteDTO>()
                .ForMember(x => x.Latitud, x => x.MapFrom(y => y.Ubicacion.Y)) //para NetTopologySuite
                .ForMember(x => x.Longitud, x => x.MapFrom(y => y.Ubicacion.X));

            CreateMap<ClienteDTO, Cliente>()
                .ForMember(x => x.Ubicacion, x => x.MapFrom(y => geometryFactory.CreatePoint(new Coordinate(y.Longitud, y.Latitud))));

            //aqui se esta ignorando la foto, para que el cliente no siempre tenga que enviarla
            //al altualizar los datos 
            CreateMap<ClienteCreacionDTO, Cliente>()
                .ForMember(x => x.Foto, options => options.Ignore())
                .ForMember(x => x.Emails, options => options.MapFrom(MapClienteEmail))
                .ForMember(x => x.Ubicacion, x => x.MapFrom(y => geometryFactory.CreatePoint(new Coordinate(y.Longitud, y.Latitud))));


            CreateMap<Cliente, ClientePatchDTO>()
                .ForMember(x => x.Latitud, x => x.MapFrom(y => y.Ubicacion.Y)) //para NetTopologySuite
                .ForMember(x => x.Longitud, x => x.MapFrom(y => y.Ubicacion.X));

            CreateMap<ClientePatchDTO, Cliente>()
                .ForMember(x => x.Ubicacion, x => x.MapFrom(y => geometryFactory.CreatePoint(new Coordinate(y.Longitud, y.Latitud))));

           


         




          

                #endregion





                CreateMap<Prestamo, PrestamoDTO>().ReverseMap();
            //aqui se esta ignorando la foto, para que el cliente no siempre tenga que enviarla
            //al altualizar los datos
            CreateMap<PrestamoCreacionDTO, Prestamo>()
                .ForMember(x => x.Foto, options => options.Ignore());

            CreateMap<PrestamoPatchDTO, Prestamo>().ReverseMap();
            this.geometryFactory = geometryFactory;
        }




        //private List<Log> MapLogTipo(LogCreacionDTO logCreacionDTO, Log log)
        //{
        //    var resultado = new List<Log>();

        //    if (logCreacionDTO == null)
        //    {
        //        return resultado;
        //    }

        //    foreach (var logs in logCreacionDTO)
        //    {
        //        var tipoDeCampoOrderPrestamos = Enum.GetName(typeof(TipoDeLog), logCreacionDTO.Level);
        //        //logCreacionDTO.Level = Enum.GetName(typeof(TipoDeLog), logCreacionDTO.Level);
        //    }

        //}





        //private ClienteCercanoDTO MapClienteUbicacion(ClienteCercanoDTO clienteCercanoDTO, Cliente cliente, ClienteCercanoFiltroDTO filtro)
        //{
        //    var UbicacionUsuario = geometryFactory.CreatePoint(new Coordinate(filtro.Longitud, filtro.Latitud));

        //    clienteCercanoDTO.DistanciaEnMetros = Math.Round(cliente.Ubicacion.Distance(UbicacionUsuario));

        //    return clienteCercanoDTO;
        //}




       


            //CreateMap<ClienteDTOCreacion, Cliente>()
            //      .ForMember(x => x.Emails, options => options.MapFrom(MapClienteEmail));


            private List<Email> MapClienteEmail(ClienteCreacionDTO clienteCreacionDTO, Cliente cliente)
        {
            var resultado = new List<Email>();
            
            if (clienteCreacionDTO.Emails == null)
            {
                return resultado;
            }

            foreach (var email in clienteCreacionDTO.Emails)
            {
                resultado.Add(new Email()
                {
                    ClienteId = cliente.Id,
                    EmailAddress = email.EmailAddress,
                    EmailEstado = email.EmailEstado,
                    FechaCreacion = email.FechaCreacion,
                    


                });
            }

            return resultado;
        }
    }
}
