using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class ResistenciaTipo
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }

        public string  Abreviatura { get; set; }

        public EResistenciaVinculoTipo Vinculo { get; set; }
    }
}
