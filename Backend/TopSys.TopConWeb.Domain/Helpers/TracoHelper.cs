using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Helpers
{
    public static class TracoHelper
    {
        public static string FormatarResistencia(this ResistenciaTipo resistenciaTipo, float mpa, int consumo)
        {
            if (resistenciaTipo == null)
                return "";

            string valor = "";

            switch (resistenciaTipo.Vinculo)
            {
                case EResistenciaVinculoTipo.Consumo:
                    valor = consumo.ToString(" 000");
                    break;
                case EResistenciaVinculoTipo.Mpa:
                    valor = mpa.ToString(" 0.0");
                    break;
            }

            return resistenciaTipo.Abreviatura + valor;
        }
    }
}
