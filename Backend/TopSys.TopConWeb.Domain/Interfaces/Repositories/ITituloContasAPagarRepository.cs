using System;
using System.Collections.Generic;
using System.ComponentModel;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface ITituloContasAPagarRepository : IServiceBase<TituloContasAPagar>
    {
        bool ExisteTituloCreditoFornecedor(long loteBaixaDeCredito, int empresa, int fornecedorDocumento, string serieDocumento, int numeroDocumento, double valorDocumento);
        bool ExisteTitulo(long loteBaixa);
        void RemoveTituloDeCredito(long loteBaixaDeCredito, int empresa, int fornecedorDocumento, string serieDocumento, int numeroDocumento, double valorDocumento);

    }
}
