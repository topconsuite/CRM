using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Entities
{
    public interface IHasEndereco
    {
        string EnderecoCep { get; set; }
        string EnderecoLogradouro { get; set; }
        int EnderecoNumero { get; set; }
        string EnderecoComplemento { get; set; }
        string EnderecoBairro { get; set; }
        int? EnderecoMunicipioCodigo { get; set; }
        Municipio EnderecoMunicipio { get; set; }
    }
}
