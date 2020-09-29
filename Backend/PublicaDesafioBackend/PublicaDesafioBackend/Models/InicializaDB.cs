using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicaDesafioBackend.Models
{
    public class InicializaDB
    {
        public static void InicializaBanco(ContextoJogo ctx)
        {
            if (ctx.pessoas.Any())
            {
                return;
            }
            Pessoa pes = new Pessoa()
            {
                NomeCompleto = "Maria Santos",
                Email = "maria123@gmail.com",
                Senha = Util.Util.HashPassword("senha123")
            };
            ctx.pessoas.Add(pes);
            ctx.SaveChanges();

            if (ctx.Jogos.Any())
            {
                return;
            }            
            var listaJogos = new Jogo[]
            {
                    new Jogo()
                    {
                    Placar = 12,
                    MinimoTemporada = 12,
                    MaximoTemporada = 12,
                    QuebraRecordeMin = 0,
                    QuebraRecordeMax = 0,
                    PessoaID = 1
                    },
                    new Jogo()
                    {
                    Placar = 24,
                    MinimoTemporada = 12,
                    MaximoTemporada = 24,
                    QuebraRecordeMin = 0,
                    QuebraRecordeMax = 1,
                    PessoaID = 1
                    },
                    new Jogo()
                    {
                    Placar = 10,
                    MinimoTemporada = 10,
                    MaximoTemporada = 24,
                    QuebraRecordeMin = 1,
                    QuebraRecordeMax = 1,
                    PessoaID = 1,
                    },
                    new Jogo()
                    {
                    Placar = 24,
                    MinimoTemporada = 10,
                    MaximoTemporada = 24,
                    QuebraRecordeMin = 1,
                    QuebraRecordeMax = 1,
                    PessoaID = 1
                    }
            };

            foreach (Jogo jog in listaJogos)
            {
                ctx.Jogos.Add(jog);
            }
            ctx.SaveChanges();

        }

    }
}
