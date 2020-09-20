using PublicaDesafioBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicaDesafioBackend.Util
{
    public class Util
    {
        public static Jogo getPreviousValidGame(ContextoJogo ctx, Jogo ultimoJogoInserido)
        {
            Jogo lastAvaibleGame = null;
            int idToSearch = ultimoJogoInserido.ID - 1;
            do
            {
                if(idToSearch < 0)
                {
                    break;
                }
                lastAvaibleGame = ctx.Jogos.Find(idToSearch);
                idToSearch--;               

            } while (lastAvaibleGame == null);
            return lastAvaibleGame;
        }
       
    }
}
