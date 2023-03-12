using Entidade;
using Microsoft.EntityFrameworkCore;
using Repositorio;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AmigoService
    {
        private AmigoContext context;

        public AmigoService(AmigoContext _context)
        {
            this.context = _context;
        }

        public IEnumerable<AmigoDTO> GetAll() {
            List<AmigoDTO> amgs = new List<AmigoDTO>();
            foreach (Amigo amg in this.context.amigos.Include(x => x.friends))
            {
                amgs.Add(this.getOutput(amg));
            }
            return amgs;
        }

        public AmigoDTO GetAmigo(int id) {
            var amg = this.context.amigos.Include(x => x.friends).FirstOrDefault(x => x.Id == id);
            if(amg == null)
            {
                return null;
            }
            return this.getOutput(amg);
        }

        public Entidade.Amigo Create(Amigo amigo)
        {
            
            var amg = this.context.amigos.FirstOrDefault(x => x.Email == amigo.Email);

            if (amg != null)
            {
                return null;
            }

            this.context.amigos.Add(amigo);
            this.context.SaveChanges();

            return amigo;
        }

        public bool AddFriend(int id, int idAmg)
        {
            var user = this.context.amigos.FirstOrDefault(x => x.Id == id);
            var amg = this.context.amigos.FirstOrDefault(x => x.Id == idAmg);


            if (user == null || amg == null)
            {
                return false;
            }

            user.friends.Add(amg);
            amg.friends.Add(user);

            this.context.amigos.Update(user);
            this.context.amigos.Update(amg);
            this.context.SaveChanges();

            return true;
        }

        public Entidade.Amigo Update(int id, Entidade.Amigo newAmigo)
        {
            var amg = this.context.amigos.FirstOrDefault(x => x.Id == id);

            amg.Nome = newAmigo.Nome;
            amg.Sobrenome = newAmigo.Sobrenome;
            amg.Email = newAmigo.Email;
            amg.Aniversario = newAmigo.Aniversario;

            this.context.amigos.Update(amg);
            this.context.SaveChanges();

            return amg;
        }

        public bool Delete(int id)
        {
            var amg = this.context.amigos.FirstOrDefault(x => x.Id == id);

            if (amg == null)
            {
                return false;
            }

            this.context.amigos.Remove(amg);
            this.context.SaveChanges();

            return true;
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

        private List<FriendDTO> convertToFriendDto(ICollection<Amigo> friends)
        {
            List<FriendDTO> newFriends = new List<FriendDTO>();

            foreach (Amigo amigo in friends)
            {
                FriendDTO friend = new FriendDTO(amigo.Nome, amigo.Sobrenome, amigo.Aniversario);
                newFriends.Add(friend);
            }
            return newFriends;
        }
    }
}
