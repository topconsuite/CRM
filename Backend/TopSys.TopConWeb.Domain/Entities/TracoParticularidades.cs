using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class TracoParticularidades
    {
        public int UsinaCodigo { get; set; }
        public int UsoCodigo { get; set; }
        public int ResistenciaTipoCodigo { get; set; }
        public int PedraCodigo { get; set; }
        public int SlumpCodigo { get; set; }
        public float Mpa { get; set; }
        public int Consumo { get; set; }
        public int SlumpInicial { get; set; }
        public float Ecs { get; set; }
        public float Eci { get; set; }
        public string TracoEspecificacao { get; set; }

        public string  Especificacao { get; set; }
    }
}
