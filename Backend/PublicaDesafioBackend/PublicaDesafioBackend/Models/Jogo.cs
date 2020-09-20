using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicaDesafioBackend.Models
{
    public class Jogo
    {
        public int ID { get; set; }

        public short Placar { get; set; }

        public short MinimoTemporada { get; set; }

        public short MaximoTemporada { get; set; }

        public short QuebraRecordeMin { get; set; }

        public short QuebraRecordeMax { get; set; }
    }
}
