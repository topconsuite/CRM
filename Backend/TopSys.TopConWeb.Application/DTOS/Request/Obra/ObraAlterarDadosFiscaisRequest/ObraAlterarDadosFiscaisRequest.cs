using System.Collections.Generic;

namespace TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraAlterarDadosFiscaisRequest
{
    public class ObraAlterarDadosFiscaisRequest
    {
        public int UsinaCodigo { get; set; }
        public int Numero { get; set; }
        public ContratoDTO Contrato { get; set; }

        public string Cei { get; set; }
        public string ObservacaoNf { get; set; }
        public virtual ICollection<ObraTributacaoMunicipalDTO> ObraTributacoesMunicipais { get; set; }
    }
}
