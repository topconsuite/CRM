using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Request.BoletoExterno;
using TopSys.TopConWeb.Application.DTOS.Response.BoletoExterno;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IBoletoExternoApplicationService : IApplicationServiceBase<BoletoExterno>
    {
        ICollection<BoletoExternoResponse> ListarBoletosExternos(int codUsina, int anoContrato, int numeroContrato);
        byte[] ObterArquivo(Guid idArquivo, string chave, int sequencia);
        string ObterArquivoConvertidoBase64(byte[] arquivo);
        ResultDTO<BoletoExternoAdicionarResponse> AdicionarBoletoExterno(BoletoExternoAdicionarRequest[] request);
    }
}
