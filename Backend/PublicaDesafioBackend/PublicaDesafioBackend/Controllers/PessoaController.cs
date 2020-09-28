using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using PublicaDesafioBackend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PublicaDesafioBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaController : ControllerBase
    {
        private readonly ContextoJogo _context;
        public PessoaController(ContextoJogo _ctx)
        {
            this._context = _ctx;
        }
        /// <summary>
        /// Efetuar Login, passando dados de Email e Senha        
        /// </summary>
        /// <param name="user">Objeto do tipo Pessoa</param>
        /// <response code="404">Caso não encontre o usuário baseado no Email e Senha</response>
        /// <response code="200">Se encontrar o usuário</response>
        // GET: api/<PessoaController>
        [HttpPost("login")]
        public ActionResult login(Pessoa user)
        {
            user.Senha = Util.Util.HashPassword(user.Senha);
            Pessoa usuario = _context.pessoas.Where(el => el.Email.Equals(user.Email) && el.Senha.Equals(user.Senha)).FirstOrDefault();
            usuario.Senha = null;
            if (usuario == null)
            {
                return NotFound("Usuario não encontrado");
            }
            else
            {
                string token = Util.Util.generateToken(usuario);
                var jogos = _context.Jogos.Where(el => el.PessoaID.Equals(usuario.ID)).ToList();
                usuario.Jogos = null;
                var data = new object[3];
                data[0] = token;
                data[1] = jogos;
                data[2] = usuario;
                return Ok(data);
            }
        }

        /// <summary>
        /// Criar conta: Efetua validação se caso o usuário é Valido.
        /// </summary>
        /// <param name="user">Objeto do Tipo Pessoa</param>
        ///<response code="400">Caso o usuário que é passado na requisição é considerado invalido</response>
        ///<response code="200">Caso seja valido é adicionado o usuario e retorna uma Mensagem dizendo que a ação foi concluida com exito</response>        
        [HttpPost("criarConta")]
        public ActionResult CriarConta(Pessoa user)
        {
            try
            {
             
                if (!Util.Util.UserIsValid(user))
                {
                    return BadRequest("Por favor cheque os dados e insira novamente");
                }
                user.Senha = Util.Util.HashPassword(user.Senha);                
                _context.pessoas.Add(user);
                _context.SaveChanges();
                return Ok("Conta criada com Sucesso");

            }catch(Exception ex)
            {
                return BadRequest("Problema ao Criar a Conta" + ex.Message);
            }

        
        }
        /// <summary>
        /// Usuario manda email e recebe em seu email um Token JWT para alterar a sua senha        
        /// </summary>
        /// <param name="user">Objeto do Tipo Pessoa</param>
        ///<response code="404">Caso não encontre o usuário</response>
        ///<response code="200">Caso encontre o usuario: retorna mensagem de exito informando sobre que o email que receberá com o Token</response>
        [HttpPost("recuperarSenha")]
        public ActionResult changePassword([FromBody] Pessoa user)
        {          
            try
            {
                Pessoa selectedUser = _context.pessoas.Where(el => el.Email.Equals(user.Email)).FirstOrDefault();
                if (selectedUser == null)
                {
                    return NotFound("Usuário não encontrado");
                }
                string token = Util.Util.recoveryPasswordToken(user.Email);
                string corpoEmail = "Informe esse código para recuperar sua senha: <strong>" + token + "</strong> Entretantoo esse token possui validade de apenas 30 minutos";
                Util.Util.SendEmail(user.Email, corpoEmail, "Recuperação de Conta");
                return Ok("Enviado email contendo o Token para alteração de senha");

            }
            catch (Exception)
            {
                return BadRequest("Problema interno");
            }
        }
        /// <summary>
        /// Alteração de Senha com o Token recebido no Email.
        /// /// </summary>
        /// <param name="token">Token JWT</param>
        ///<response code="400">Caso o token esteja expirado</response>
        ///<response code="200">Altera a senha para uma guid e retorna esta senha na requisição</response>
        [HttpPost("token")]
        public ActionResult changePassword([FromBody] TokenPassword token)
        {
            var stream = token.token;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = handler.ReadToken(stream) as JwtSecurityToken;

            var email = tokenS.Claims.First(claim => claim.Type == "email").Value;
            var exp = tokenS.Claims.First(claim => claim.Type == "exp").Value;
            DateTime expirationTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            expirationTime = expirationTime.AddSeconds(Convert.ToDouble(exp)).ToUniversalTime();
            if (DateTime.UtcNow > expirationTime)
            {
                return BadRequest("Token expirado");
            }
            else
            {
                Pessoa user = _context.pessoas.Where(user => user.Email.Equals(email)).FirstOrDefault();
                var guid = Guid.NewGuid().ToString();
                user.Senha = Util.Util.HashPassword(guid);
                _context.SaveChanges();
                return Ok(guid);
            }

        }
        /// <summary>
        /// Altera as informações de usuário
        /// </summary>
        /// <param name="id">ID Da pessoa que deseja modificar</param>
        /// <param name="pes">Objeto do tipo pessoa que será enviado na requisição</param>
        /// <response code="404">Caso não encontre a pessoa</response>
        /// <response code="200">Caso encontre a pessoa: altera a senha para uma Guid</response>
        /// <response code="401">Caso usuário tente alterar um ID que não seja o dele</response>
        [HttpPut("{id}")]
        [Authorize]
        public ActionResult Put(int id, [FromBody] Pessoa pes)
        {
            try
            {
                Pessoa pessoa = _context.pessoas.Find(id);
                if(pessoa == null)
                {
                    return NotFound("Pessoa não encontrada");
                }
                string stream = Request.Headers[HeaderNames.Authorization].ToString();
                stream = stream.Replace("Bearer ", string.Empty);
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(stream);
                var tokenS = handler.ReadToken(stream) as JwtSecurityToken;
                int IdUserToken = Convert.ToInt32(tokenS.Claims.First(claim => claim.Type == "primarysid").Value.ToString());
                if(pessoa.ID == IdUserToken)
                {
                    pessoa.NomeCompleto = pes.NomeCompleto;
                    pessoa.Senha = Util.Util.HashPassword(pes.Senha);
                    pessoa.Email = pes.Email;
                    _context.SaveChanges();                    
                    return Ok("Registro Alterado com Sucesso");
                }
                else
                {
                    return Unauthorized("Você não pode editar uma informação ao qual não é sua");
                }

            }
            catch(Exception ex)
            {
                return BadRequest("Erro ao Editar o Usuário: " + ex.Message);
            }
            
        }

    }
}
