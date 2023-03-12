using System.Text.Json.Serialization;

namespace tp1.DTO
{
    public class AmigoDTO
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public List<FriendDTO> Friends { get; set; }

        public AmigoDTO(string nome, string sobrenome, string email, List<FriendDTO> friends)
        {
            Nome = nome;
            Sobrenome = sobrenome;
            Email = email;
            Friends = friends;
        }
    }
}
