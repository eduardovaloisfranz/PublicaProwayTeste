using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PublicaDesafioBackend.Models;
using PublicaDesafioBackend.Util;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PublicaDesafioBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        // GET: api/<JogosController>
        private readonly ContextoJogo _ctx;
        public JogosController(ContextoJogo ctx)
        {
            this._ctx = ctx;
        }
        [HttpGet]
        public ActionResult Get()
        {
            //return new string[] { "value1", "value2" };
            var ListaJogos = from jogo in _ctx.Jogos.ToList() select jogo;
            return Ok(ListaJogos);
        }


        // POST api/<JogosController>
        [HttpPost]
        public ActionResult Post([FromBody] Jogo jogo)
        {
            try
            {
                if(jogo.Placar <= 0 || jogo.Placar > 1000)
                {
                    return BadRequest("Placar invalido");
                }
                _ctx.Jogos.Add(jogo);
                _ctx.SaveChanges();                
                int quantidadeJogos = _ctx.Jogos.Where(el =>  el.PessoaID.Equals(jogo.PessoaID)).Count();
                if (quantidadeJogos == 1)
                {
                    Jogo firstGame = _ctx.Jogos.Where(el => el.PessoaID.Equals(jogo.PessoaID)).FirstOrDefault<Jogo>();                    
                    Jogo jog = _ctx.Jogos.Find(firstGame.ID);
                    jog.MinimoTemporada = jog.Placar;
                    jog.MaximoTemporada = jog.Placar;
                    jog.QuebraRecordeMin = 0;
                    jog.QuebraRecordeMax = 0;
                    _ctx.SaveChanges();
                }
                else
                {                    
                    Jogo jogoAnterior = Util.Util.getPreviousValidGame(_ctx, jogo);          
                    if (jogo.Placar < jogoAnterior.MinimoTemporada)
                    {
                        jogo.MinimoTemporada = jogo.Placar;
                        jogo.MaximoTemporada = jogoAnterior.MaximoTemporada;
                        jogo.QuebraRecordeMin = jogoAnterior.QuebraRecordeMin;
                        jogo.QuebraRecordeMin += 1;
                        jogo.QuebraRecordeMax = jogoAnterior.QuebraRecordeMax;
                        _ctx.SaveChanges();
                    }
                    else if (jogo.Placar > jogoAnterior.MaximoTemporada)
                    {
                        jogo.MaximoTemporada = jogo.Placar;
                        jogo.MinimoTemporada = jogoAnterior.MinimoTemporada;
                        jogo.QuebraRecordeMax = jogoAnterior.QuebraRecordeMax;
                        jogo.QuebraRecordeMax += 1;
                        jogo.QuebraRecordeMin = jogoAnterior.QuebraRecordeMin;
                        _ctx.SaveChanges();
                    }
                    else
                    {
                        jogo.MinimoTemporada = jogoAnterior.MinimoTemporada;
                        jogo.MaximoTemporada = jogoAnterior.MaximoTemporada;
                        jogo.QuebraRecordeMin = jogoAnterior.QuebraRecordeMin;
                        jogo.QuebraRecordeMax = jogoAnterior.QuebraRecordeMax;
                        _ctx.SaveChanges();
                    }
                }
                return Accepted(jogo);
            }catch(Exception ex)
            {
                return BadRequest("Erro ao Adicionar o Placar." + ex.Message); 
            }            
        }

        // PUT api/<JogosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<JogosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
