using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicaDesafioBackend.Models
{
    public class ContextoJogo : DbContext
    {
        public ContextoJogo(DbContextOptions<ContextoJogo> options) : base(options)
        {
        }
        public DbSet<Jogo> Jogos { get; set; }
    }
}
