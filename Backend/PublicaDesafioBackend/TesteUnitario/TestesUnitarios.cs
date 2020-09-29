using Microsoft.VisualStudio.TestTools.UnitTesting;
using PublicaDesafioBackend.Models;
using PublicaDesafioBackend.Util;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.EntityFrameworkCore;

namespace TesteUnitario
{
    [TestClass]
    public class TestesUnitarios
    {
        [TestMethod]
        public void TestUserIsValid()
        {
            Pessoa pessoa = new Pessoa();
            pessoa.NomeCompleto = "";
            pessoa.Email = "";
            pessoa.Senha = "";
            bool resultado = Util.UserIsValid(pessoa);
            Assert.IsFalse(resultado);
        }
        [TestMethod]
        public void TesteObterUltimoJogoValidoMelhorCaso()
        {
            string dbName = Guid.NewGuid().ToString();
            DbContextOptions<ContextoJogo> options = new DbContextOptionsBuilder<ContextoJogo>()
                            .UseInMemoryDatabase(databaseName: dbName).Options;

            // Seed
            using (ContextoJogo dbContext = new ContextoJogo(options))
            {
                //   PersonEntity person = PersonEntity.Create(1, "Batman", "Gotham City");
                Pessoa pes = new Pessoa()
                {
                    NomeCompleto = "Maria Teste",
                    Email = "maria123@gmail.com",
                    Senha = Util.HashPassword("senha123")
                };
                dbContext.pessoas.Add(pes);
                dbContext.SaveChanges();
                var listaJogos = new Jogo[]
                {
                    new Jogo()
                    {
                        ID = 1,
                        Placar = 12,
                        MinimoTemporada = 12,
                        MaximoTemporada = 12,
                        QuebraRecordeMin = 0,
                        QuebraRecordeMax = 0,
                        PessoaID = 1
                    },
                    new Jogo()
                    {
                        ID = 2,
                        Placar = 24,
                        MinimoTemporada = 12,
                        MaximoTemporada = 24,
                        QuebraRecordeMin = 0,
                        QuebraRecordeMax = 1,
                        PessoaID = 1
                    },
                    new Jogo()
                    {
                        ID = 3,
                        Placar = 10,
                        MinimoTemporada = 10,
                        MaximoTemporada = 24,
                        QuebraRecordeMin = 1,
                        QuebraRecordeMax = 1,
                        PessoaID = 1,
                    }
                };

                foreach (Jogo jog in listaJogos)
                {
                    dbContext.Jogos.Add(jog);
                }
                dbContext.SaveChanges();

                Jogo jogo = new Jogo()
                {
                    ID = 4,
                    Placar = 24,
                    MinimoTemporada = 10,
                    MaximoTemporada = 24,
                    QuebraRecordeMin = 1,
                    QuebraRecordeMax = 1,
                    PessoaID = 1
                };
                dbContext.SaveChanges();
                Jogo lastGameValid = Util.getPreviousValidGame(dbContext, jogo);
                bool resultado = lastGameValid.ID == 3;
                Assert.IsTrue(resultado);         
            }
        }
        [TestMethod]
        public void TesteObterUltimoJogoValidoPiorCaso()
        {
            string dbName = Guid.NewGuid().ToString();
            DbContextOptions<ContextoJogo> options = new DbContextOptionsBuilder<ContextoJogo>()
                            .UseInMemoryDatabase(databaseName: dbName).Options;            
            using (ContextoJogo dbContext = new ContextoJogo(options))
            {                
                Pessoa pes = new Pessoa()
                {
                    NomeCompleto = "Maria Teste",
                    Email = "maria123@gmail.com",
                    Senha = Util.HashPassword("senha123")
                };
                dbContext.pessoas.Add(pes);
                dbContext.SaveChanges();
                var listaJogos = new Jogo[]
                {
                    new Jogo()
                    {
                        ID = 1,
                        Placar = 12,
                        MinimoTemporada = 12,
                        MaximoTemporada = 12,
                        QuebraRecordeMin = 0,
                        QuebraRecordeMax = 0,
                        PessoaID = 1
                    },
                    new Jogo()
                    {
                        ID = 2,
                        Placar = 24,
                        MinimoTemporada = 12,
                        MaximoTemporada = 24,
                        QuebraRecordeMin = 0,
                        QuebraRecordeMax = 1,
                        PessoaID = 1
                    },
                    new Jogo()
                    {
                        ID = 3,
                        Placar = 10,
                        MinimoTemporada = 10,
                        MaximoTemporada = 24,
                        QuebraRecordeMin = 1,
                        QuebraRecordeMax = 1,
                        PessoaID = 1,
                    }
                };

                foreach (Jogo jog in listaJogos)
                {
                    dbContext.Jogos.Add(jog);
                }
                dbContext.SaveChanges();

                Jogo jogo = new Jogo()
                {
                    ID = 4,
                    Placar = 24,
                    MinimoTemporada = 10,
                    MaximoTemporada = 24,
                    QuebraRecordeMin = 1,
                    QuebraRecordeMax = 1,
                    PessoaID = 1
                };
                dbContext.SaveChanges();
                dbContext.Jogos.Remove(dbContext.Jogos.Find(3));
                dbContext.Jogos.Remove(dbContext.Jogos.Find(2));
                dbContext.SaveChanges();
                //removendo jogos 3,2 tenho que obter logicamente o primeiro.
                Jogo lastGameValid = Util.getPreviousValidGame(dbContext, jogo);
                bool resultado = lastGameValid.ID == 1;
                Assert.IsTrue(resultado);
            }
        }
        [TestMethod]
        public void TesteHashPassword()
        {
            string Senha = "senha123";
            Senha = Util.HashPassword(Senha);

            bool resultado = !Senha.Equals("senha123");
            Assert.IsTrue(resultado);
        }
    }
}
