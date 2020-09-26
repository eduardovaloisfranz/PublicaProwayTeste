using Microsoft.IdentityModel.Tokens;
using PublicaDesafioBackend.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PublicaDesafioBackend.Util
{
    public class Util
    {
        public static string Secret = "0E7ABB89019388DAC739E3CDBA66BF045D8A7284346131487992A1A9C065637F8D64D8CE23BE410D111B33724B202C67265E90E037A7BAAA32510484472032CF";
        public static Jogo getPreviousValidGame(ContextoJogo ctx, Jogo ultimoJogoInserido)
        {                 
            var myGames = ctx.Jogos.Where(el => el.PessoaID.Equals(ultimoJogoInserido.PessoaID)).ToList();
            foreach(Jogo jog in myGames.Reverse<Jogo>())
            {
                if(jog.ID < ultimoJogoInserido.ID)
                {
                    return jog;
                }
            }            
            
            return null;
        }


        public static string HashPassword(string password)
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] hashBytes;
            using (HashAlgorithm hash = SHA1.Create())
                hashBytes = hash.ComputeHash(encoding.GetBytes(password));

            StringBuilder hashValue = new StringBuilder(hashBytes.Length * 2);
            foreach (byte b in hashBytes)
            {
                hashValue.AppendFormat(CultureInfo.InvariantCulture, "{0:X2}", b);
            }

            return hashValue.ToString();
        }

        public static string generateToken(Pessoa user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Util.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.PrimarySid, user.ID.ToString()),
                    new Claim(ClaimTypes.Name, user.NomeCompleto.ToString())
                    //new Claim(ClaimTypes.Role, user.autorizacao.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static bool UserIsValid(Pessoa user)
        {
            if (user.NomeCompleto == null || user.NomeCompleto.Length < 3)
            {
                return false;
            }
            else if (user.Email == null ||( user.Email.Length < 3 && user.Email.Contains("@")))
            {
                return false;
            }
            else if (user.Senha == null || user.Senha.Length < 3)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static string recoveryPasswordToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Util.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Email, email.ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
      
    }
}
