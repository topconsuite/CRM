using System;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Response.ContratoReajuste
{
    public class ObraTracoReajusteResponse
    {
        public int UsoCodigo { get; set; }
        public UsoDTO Uso { get; set; }
        public int PedraCodigo { get; set; }
        public PedraDTO Pedra { get; set; }
        public int SlumpCodigo { get; set; }
        public SlumpDTO Slump { get; set; }
        public int ResistenciaTipoCodigo { get; set; }
        public virtual ResistenciaTipoDTO ResistenciaTipo { get; set; }
        public float Mpa { get; set; }
        public int Consumo { get; set; }
        public float PrecoVigente { get; set; }
        public float PrecoRecalculado { get; set; }
        public string DescricaoPersonalizada { get; set; } = "";

        public string Descricao
        {
            get
            {
                if (ResistenciaTipo == null || Pedra == null || Slump == null || Uso == null) return "";
                if (DescricaoPersonalizada != "") return DescricaoPersonalizada;

                var vinculo = ResistenciaTipo.Vinculo;
                var mpaConsumo = (vinculo == EResistenciaVinculoTipo.Mpa ? Mpa : (vinculo == EResistenciaVinculoTipo.Consumo ? Consumo : 0));

                return $"{ResistenciaTipo.Abreviatura} {mpaConsumo}/{Pedra.Descricao}/{Slump.Descricao}/{Uso.Descricao}";
            }
        }
    }
}
