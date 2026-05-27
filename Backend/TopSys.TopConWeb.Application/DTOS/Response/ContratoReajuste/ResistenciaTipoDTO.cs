using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.ContratoReajuste
{
    public class ResistenciaTipoDTO
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }

        public string Abreviatura { get; set; }

        public EResistenciaVinculoTipo Vinculo { get; set; }
    }
}
