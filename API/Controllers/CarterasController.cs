using API.DTOs;
using API.Servicios;
using AutoMapper;
using Entidades;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/carteras")]
    [ApiController]
    public class CarterasController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CarterasController(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CarteraDTO>>> Get()
        {
            return await Get<Cartera, CarteraDTO>();
        }

        [HttpGet("{id:Guid}", Name = "obtenerCartera")]
        public async Task<ActionResult<CarteraDTO>> Get(Guid id)
        {
            return await Get<Cartera, CarteraDTO>(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CarteraCreacionDTO carteraCreacionDTO)
        {
            return await Post<CarteraCreacionDTO, Cartera, CarteraDTO>(carteraCreacionDTO, "obtenerCartera");
        }

        [HttpPut]
        public async Task<ActionResult> Put (Guid id, [FromBody] CarteraCreacionDTO carteraCreacionDTO)
        {
            return await Put<CarteraCreacionDTO, Cartera>(id, carteraCreacionDTO);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            return await Delete<Cartera>(id);
        }

            
            

    }
}
