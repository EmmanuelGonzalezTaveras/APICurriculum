using System.Data;
using API.DTOs;
using AutoMapper;
using Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/emails")]
    public class EmailsController : CustomBaseController 
    {
     

        public EmailsController(ApplicationDbContext context, IMapper mapper): base(context, mapper)
        {
        }

        [HttpGet]
        public async Task<ActionResult<List<EmailDTO>>> Get()

        {
          
            return await Get<Email, EmailDTO>();

        }


        [HttpGet("{id}", Name = "ObtenerEmail")]
        public async Task<ActionResult<EmailDTO>> Get(Guid id)
        {
           
            return await Get<Email, EmailDTO>(id);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EmailCreacionDTO emailCreacionDTO)
        {
            
            return await Post<EmailCreacionDTO, Email, EmailDTO>(emailCreacionDTO, "ObtenerEmail");


        }


        [HttpPut("{id}")] 
        public async Task<ActionResult> Put(Guid id, [FromBody] EmailCreacionDTO emailCreacionDTO)
        {
            
            return await Put<EmailCreacionDTO, Email>(id, emailCreacionDTO);
        }


        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Delete(Guid id)
        {
            
            return await Delete<Email>(id);

        }


    }
}
