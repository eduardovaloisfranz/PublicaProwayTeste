using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PublicaDesafioBackend.Models
{
    public class Pessoa
    {
        public int ID { get; set; }
        
        [StringLength(50)]
        public string NomeCompleto { get; set; }

        [StringLength(50)]
        public string Email { get; set; }
        
        [StringLength(250)]
        public string Senha { get; set; }

        public virtual List<Jogo> Jogos { get; set; }

    }
}
