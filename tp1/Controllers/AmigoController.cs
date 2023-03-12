using Entidade;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositorio;
using tp1.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace tp1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmigoController : ControllerBase
    {
        private AmigoContext context;

        public AmigoController(AmigoContext context)
        {
            this.context = context;
        }


        // GET: api/<AmigoController>
        [HttpGet]
        public IActionResult Get()
        {
            List<AmigoDTO> amgs = new List<AmigoDTO>();
            foreach(Amigo amg in this.context.amigos.Include(x => x.friends))
            {
                amgs.Add(this.getOutput(amg));
            }
            return Ok(amgs);
        }

        // GET api/<AmigoController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var amg = this.context.amigos.Include(x => x.friends).FirstOrDefault(x => x.Id == id);
            
            if (amg == null)
            {
                return NotFound();
            }

           /* List<FriendDTO> friends;
            if(amg.friends == null || amg.friends.Count == 0)
            {
                friends = null;
            }
                
            friends = this.convertToFriendDto(amg.friends);
            AmigoDTO amigo = this.convertToAmgDto(amg, friends);*/


            return Ok(this.getOutput(amg));
        }

        // POST api/<AmigoController>
        [HttpPost]
        public IActionResult Post([FromBody] Amigo amigo)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var amg = this.context.amigos.FirstOrDefault(x => x.Email == amigo.Email);

            if (amg != null)
            {
                return UnprocessableEntity(new
                {
                    Errors = "Email já cadastrado na base de dados, por favor utilize outro"
                });
            }

            this.context.amigos.Add(amigo);
            this.context.SaveChanges();

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
            var user = this.context.amigos.FirstOrDefault(x => x.Id == id);
            var amg = this.context.amigos.FirstOrDefault(x => x.Id == idAmg);

            
            if (user == null || amg == null)
            {
                return NotFound();
            }

            user.friends.Add(amg);
            amg.friends.Add(user);

            this.context.amigos.Update(user);
            this.context.amigos.Update(amg);
            this.context.SaveChanges();

            return Ok();
        }

        // PUT api/<AmigoController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Amigo newAmigo)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var amg = this.context.amigos.FirstOrDefault(x => x.Id == id);

            if (amg == null)
            {
                return NotFound();
            }

            amg.Nome = newAmigo.Nome;
            amg.Sobrenome = newAmigo.Sobrenome;
            amg.Email = newAmigo.Email;
            amg.Aniversario = newAmigo.Aniversario;

            this.context.amigos.Update(amg);
            this.context.SaveChanges();

            return Ok(amg);
        }

        // DELETE api/<AmigoController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var amg = this.context.amigos.FirstOrDefault(x => x.Id == id);

            if (amg == null)
            {
                return NotFound();
            }

            this.context.amigos.Remove(amg);
            this.context.SaveChanges();

            return NoContent();

        }

        private AmigoDTO getOutput(Amigo amg)
        {
            List<FriendDTO> friends;
            if (amg.friends == null || amg.friends.Count == 0)
            {
                friends = null;
            }

            friends = this.convertToFriendDto(amg.friends);
            AmigoDTO amigo = this.convertToAmgDto(amg, friends);

            return amigo;
        }
        private AmigoDTO convertToAmgDto(Amigo amg, List<FriendDTO> friends)
        {
            AmigoDTO amigo = new AmigoDTO(amg.Nome, amg.Sobrenome, amg.Email, friends);
            return amigo;
        }

        private List<FriendDTO> convertToFriendDto(ICollection<Amigo> friends) {
            List<FriendDTO> newFriends = new List<FriendDTO>();

            foreach(Amigo amigo in friends)
            {
                FriendDTO friend = new FriendDTO(amigo.Nome, amigo.Sobrenome, amigo.Aniversario);
                newFriends.Add(friend);
            }
            return newFriends;
        }
    }
}
