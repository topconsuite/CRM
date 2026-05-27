using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Text.RegularExpressions;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Domain.Constants;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class TituloContasAPagarRepository : RepositoryBase<TituloContasAPagar>, ITituloContasAPagarRepository
    {
        const int DOCUMENTO_TIPO_CARTAO = 88;
        private readonly IdentityHelperService _identityHelperService;

        public TituloContasAPagarRepository(AppDataContext context, IdentityHelperService identityHelperService) : base(context)
        {
            _identityHelperService = identityHelperService;
        }

        public bool ExisteTituloCreditoFornecedor(long loteBaixaDeCredito, int empresa, int fornecedorDocumento, string serieDocumento, int numeroDocumento, double valorDocumento)
        {
            var tituloContasAPagar = _context.TitulosContasAPagar.AsNoTracking()
               .Where(t => t.EmpresaCodigo == empresa &&
                           t.FornecedorCodigo == fornecedorDocumento &&
                           t.DocumentoTipoCodigo == (int)EDocumentoTipo.CreditoCliente &&
                           t.DocumentoSerie == serieDocumento &&
                           t.DocumentoNumero == numeroDocumento &&
                           t.Valor == valorDocumento &&
                           t.LoteBaixaDeCredito == loteBaixaDeCredito)
               .FirstOrDefault();

            return tituloContasAPagar != null;
        }

        public bool ExisteTitulo(long loteBaixa)
        {
            var tituloContasAPagar = _context.TitulosContasAPagar.AsNoTracking()
               .Where(t => t.LoteBaixa == loteBaixa)
               .FirstOrDefault();

            return tituloContasAPagar != null;
        }

        // TESTED: 
        public void RemoveTituloDeCredito(long loteBaixaDeCredito, int empresa, int fornecedorDocumento, string serieDocumento, int numeroDocumento, double valorDocumento)
        {
            var sqlComando = new StringBuilder();

            sqlComando.Append("DELETE FROM fin_cap");
            sqlComando.Append($" WHERE Emp = {empresa}");
            sqlComando.Append($" AND Forn = {fornecedorDocumento}");
            sqlComando.Append($" AND Ser_Doc = '{serieDocumento}'");
            sqlComando.Append($" AND Num_Doc = {numeroDocumento}");
            sqlComando.Append($" AND forn = {fornecedorDocumento}");
            sqlComando.Append($" AND Vl = '{valorDocumento * -1}'");
            sqlComando.Append($" AND lote_bx_cred = {loteBaixaDeCredito}");
            sqlComando.Append($" AND Tp_Doc = {(int)EDocumentoTipo.CreditoCliente}");
            sqlComando.Append($" AND desdo = {Titulo.DESDOBRAMENTO_TITULO_PRINCIPAL}");


            var comandoSql = sqlComando.ToString();

            _context.Database.Connection.Execute(comandoSql);
            _context.Database.Connection.GravarLogGeral("API CR", "fin_cap", comandoSql);
        }

    }
}
