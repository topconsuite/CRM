using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Generic;
using TopSys.TopConWeb.Application.DTOS.Request.TituloContasAReceber;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.TituloContasAReceber;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface ITituloContasAReceberApplicationService : IApplicationServiceBase<TituloContasAReceber>
    {
        IEnumerable<TituloContasAReceberResponse> ListarPorNumeroCartaoAutorizacao(int numeroCartao, string autorizacao);
        IEnumerable<TituloContasAReceberResponse> ListarPorNumeroCartaoAutorizacaoDuplicado(int idUsina, int contratoAno, int contratoNumero, int numeroCartao, string autorizacao);

        ResultDTO<PublicoTituloContasAReceberResponseList> ObterPorParametros(int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv, int desdobramento);
        ResultDTO<PublicoTituloContasAReceberResponseList> ObterPorParametros(int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento, int codBancoBand, int numAgencia, int numConta, int numContaDv);
        ResultDTO<PublicoReguaDeCobrancaTituloContasAReceberResponse> ObterPorIdReguaDeCobranca(string id);
        ResultDTO<PublicoTituloContasAReceberResponseList> Listar(DateTime? dataEmissao, DateTime? dataOperacao, int tipoDocumento, int? centroCusto, string serieDocumento, long? numeroDocumento, int cliente, int pagina = 0, int limite = 0);
        ResultDTO<PagedList<PublicoTituloContasAReceberResponse>> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int? page, int? limit);
        ResultDTO<PublicoTituloContasAReceberAdicionarResponse> TituloContasAReceberAdicionar(TituloContasAReceberAdicionarRequest[] request);
        ResultDTO<PublicoTituloContasAReceberResponse> TituloContasAReceberAtualizar(int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv, int desdobramento, TituloContasAReceberAtualizarRequest request);
    }


}
