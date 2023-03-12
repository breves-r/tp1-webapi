using Entidade;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositorio;
using Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace tp1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmigoController : ControllerBase
    {
        private AmigoService service;

        public AmigoController(AmigoContext context, AmigoService service)
        {
            this.service = service;
        }


        // GET: api/<AmigoController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this.service.GetAll());
        }

        // GET api/<AmigoController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var amg = this.service.GetAmigo(id);

            return amg != null ? Ok(amg) : NotFound();
        }

        // POST api/<AmigoController>
        [HttpPost]
        public IActionResult Post([FromBody] Amigo amigo)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);


            var amg = this.service.Create(amigo);

            if(amg == null )
            {
                return UnprocessableEntity();
            }
            
            return Created($"/amigo/{amigo.Id}", amigo);
            
        }

        // POST api/<AmigoController>/5
        [HttpPost("add/{id}")]
        public IActionResult AddAmigo(int id, [FromBody] int idAmg)
        {
            if(id == idAmg)
            {
                return BadRequest();
            }

            bool ok = this.service.AddFriend(id, idAmg);

            if(ok == false)
                return NotFound();

            return Ok();
        }

        // PUT api/<AmigoController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Amigo newAmigo)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            if(this.service.GetAmigo(id) == null)
                return NotFound();

            var amg = this.service.Update(id, newAmigo);

            return Ok(amg);
        }

        // DELETE api/<AmigoController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool isDeleted = this.service.Delete(id);

            if(isDeleted == false)
                return NotFound();

            return NoContent();

        }

    }
}
