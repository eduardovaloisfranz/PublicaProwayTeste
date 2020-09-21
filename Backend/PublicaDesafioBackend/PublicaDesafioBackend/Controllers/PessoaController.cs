using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        // GET: api/<PessoaController>
        [HttpPost("login")]
        public ActionResult login(Pessoa user)
        {
            user.Senha = Util.Util.HashPassword(user.Senha);
            Pessoa usuario = _context.pessoas.Where(el => el.Email.Equals(user.Email) && el.Senha.Equals(user.Senha)).FirstOrDefault();
            if (usuario == null)
            {
                return BadRequest("Usuario não encontrado");
            }
            else
            {
                string token = Util.Util.generateToken(usuario);
                return Ok(token);
            }
        }

        [HttpPost("criarConta")]
        public ActionResult CriarConta(Pessoa user)
        {
            try
            {
                user.Senha = Util.Util.HashPassword(user.Senha);
                _context.pessoas.Add(user);
                _context.SaveChanges();
                return NoContent();

            }catch(Exception ex)
            {
                return BadRequest("Problema ao Criar a Conta" + ex.Message);
            }

        
        }


        // GET api/<PessoaController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PessoaController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PessoaController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
      
    }
}
