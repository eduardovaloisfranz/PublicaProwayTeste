using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicaDesafioBackend.Models
{
    public class Jogo
    {
        public int ID { get; set; }

        public ushort Placar { get; set; }

        public ushort MinimoTemporada { get; set; }

        public ushort MaximoTemporada { get; set; }

        public ushort QuebraRecordeMin { get; set; }

        public ushort QuebraRecordeMax { get; set; }
    }
}
