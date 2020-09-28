using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
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

        // POST api/<JogosController>
        /// <summary>
        /// Adiciona Jogo do Usuário. é informado apenas o placar e a regra de negocio é efetuada no Controller
        /// </summary>
        /// <param name="jogo">Objeto do Tipo Jogo</param>
        ///<response code="401">Caso tente adicionar um jogo que o ID do Token é diferente do informado na Requisição do tipo Jogo</response>
        ///<response code="400">Caso o placar do jogo é invalido</response>
        ///<response code="202">Caso o jogo seja considerado valido</response>
        [HttpPost]
        [Authorize]        
        public ActionResult Post([FromBody] Jogo jogo)
        {
            try
            {
                string stream = Request.Headers[HeaderNames.Authorization].ToString();
                stream = stream.Replace("Bearer ", string.Empty);
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(stream);
                var tokenS = handler.ReadToken(stream) as JwtSecurityToken;
                int IdUserToken = Convert.ToInt32(tokenS.Claims.First(claim => claim.Type == "primarysid").Value.ToString());
                if (jogo.PessoaID != IdUserToken)
                {
                    return Unauthorized("Você não pode adicionar nenhum jogo para alguem que não seja você");
                }

                if (jogo.Placar <= 0 || jogo.Placar > 1000)
                {
                    return BadRequest("Placar invalido");
                }
                _ctx.Jogos.Add(jogo);
                _ctx.SaveChanges();
                int quantidadeJogos = _ctx.Jogos.Where(el => el.PessoaID.Equals(jogo.PessoaID)).Count();
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
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao Adicionar o Placar." + ex.Message);
            }
        }
        /// <summary>
        /// Obtem os jogos por Id
        /// </summary>
        /// <param name="id">Id da pessoa dona dos Jogos</param>
        ///<response code="200">Retorna a lista de Jogos do Id informado</response>
        ///<response code="401">Caso o Id do Token é diferente do id Informado na requisição</response>
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult Get(int id)
        {
            string stream = Request.Headers[HeaderNames.Authorization].ToString();
            stream = stream.Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = handler.ReadToken(stream) as JwtSecurityToken;
            int IdUserToken = Convert.ToInt32(tokenS.Claims.First(claim => claim.Type == "primarysid").Value.ToString());
            if (IdUserToken == id)
            {
                var listaDeJogos = _ctx.Jogos.Where(el => el.PessoaID.Equals(id)).ToList();
                return Ok(listaDeJogos);
            }
            else
            {
                return Unauthorized("Você não pode visualizar jogos que não pertencem a você");
            }

        }

        /// <summary>
        /// Deleta um Jogo baseado no ID
        /// </summary>
        /// <param name="id">ID Do jogo Desejado</param>
        ///<response code="404">Caso não encontre o jogo</response>
        ///<response code="200">Registro é apagado</response>
        ///<response code="400">Caso seja detectado alguma Exceção no código que não foi tratado</response>
        // DELETE api/<JogosController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Delete(int id)
        {
            try
            {
                Jogo jogo = _ctx.Jogos.Find(id);
                if (jogo == null)
                {
                    return NotFound("Nenhum Jogo foi Encontrado neste ID");

                }
                string stream = Request.Headers[HeaderNames.Authorization].ToString();
                stream = stream.Replace("Bearer ", string.Empty);
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(stream);
                var tokenS = handler.ReadToken(stream) as JwtSecurityToken;
                int IdUserToken = Convert.ToInt32(tokenS.Claims.First(claim => claim.Type == "primarysid").Value.ToString());
                if (jogo.PessoaID == IdUserToken)
                {
                    _ctx.Jogos.Remove(jogo);
                    _ctx.SaveChanges();
                    return Ok("Registro Apagado com Sucesso");
                }
                else
                {
                    return Unauthorized("Você não pode apagar um jogo que não é seu");
                }
            }
            catch (Exception e)
            {
                return BadRequest("Erro ao tentar Apagar este Jogo" + e.Message);
            }    
     
       
    }
}
}
