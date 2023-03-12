using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidade
{
    public class Amigo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo 'Nome' Obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo 'Sobrenome' Obrigatório")]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "Campo 'Email' Obrigatório")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo 'Anversário' Obrigatório")]
        public DateTime Aniversario { get; set; }

        public ICollection<Amigo> amigos;
    }
}
