using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using Dapper;
using System.Text.RegularExpressions;
using TopSys.TopConWeb.Infra.Data.Helpers;
using Topsys.TopConWeb.SharedKernel.Services;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class EnderecoRepository : RepositoryBase<Endereco>, IEnderecoRepository
    {
        private readonly IdentityHelperService _identityHelperService;
        public EnderecoRepository(AppDataContext context, IdentityHelperService identityHelperService) : base(context)
        {
            _context = context;
            _identityHelperService = identityHelperService;
        }

        public Endereco ObterPorCep(string cep)
        {
            var endereco = _context.Enderecos
                .Include(e => e.Municipio)
                .Where(e => e.Cep == cep)
                .AsNoTracking()
                .FirstOrDefault();

            return endereco;
        }

        public IEnumerable<Municipio> ListarMunicipios()
        {
            var municipios = _context.Municipios.AsNoTracking().ToList();

            return municipios;
        }

        public Municipio ObterMunicipioPorNomeUf(string municipioNome, string uf)
        {
            var municipio = _context.Municipios
                .Where(t => t.Nome.Equals(municipioNome) && t.Uf.Equals(uf))
                .AsNoTracking()
                .FirstOrDefault();

            return municipio;
        }

        public Municipio ObterMunicipioPorNomeUf(string municipioNome, string uf, params char[] escaparCaracteres)
        {
            Regex pattern = new Regex($"[{new String(escaparCaracteres)}]");

            var municipioNomeEscapado = pattern.Replace(municipioNome, "");

            var municipio = _context.Municipios
                .Where(t => (t.Nome.Equals(municipioNomeEscapado)) && t.Uf.Equals(uf))
                .AsNoTracking()
                .FirstOrDefault();

            return municipio;
        }

        public bool UsinaAtendeCep(int idUsina, string cep)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT");
            sqlComando.Append(" atende");
            sqlComando.Append(" FROM topsys.con_tab_preco_cep");
            sqlComando.Append(" WHERE");
            sqlComando.Append(" usina_entrega=@ID_USINA");
            sqlComando.Append(" AND cep_de<=@CEP");
            sqlComando.Append(" AND cep_ate>=@CEP");
            
            var result = _context.Database.Connection.Query(sqlComando.ToString(), new { ID_USINA = idUsina, CEP = cep })
                .Select(row => new { row.atende })
                .FirstOrDefault();

            if (result == null) return true;

            return result.atende == "S";
        }

        public float? ObterValorAdicionalM3PorUsinaCep(int idUsina, string cep)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT");
            sqlComando.Append(" atende, vlr_adic_m3 valor");
            sqlComando.Append(" FROM topsys.con_tab_preco_cep");
            sqlComando.Append(" WHERE");
            sqlComando.Append(" usina_entrega=@ID_USINA");
            sqlComando.Append(" AND cep_de<=@CEP");
            sqlComando.Append(" AND cep_ate>=@CEP");

            var result = _context.Database.Connection.Query(sqlComando.ToString(), new { ID_USINA = idUsina, CEP = cep })
                .Select(row => new { row.atende, row.valor })
                .FirstOrDefault();

            if (result == null) return 0.0f;

            if (result.atende == "N") return null;

            return result.valor;

        }

        public Municipio ObterMunicipioPorIbgeCodigo(int ibgeCodigo)
        {
            var municipio = _context.Municipios
                .Where(t => t.IbgeCodigo == ibgeCodigo && t.IbgeCodigo != 0)
                .AsNoTracking()
                .FirstOrDefault();

            return municipio;
        }

        public void Salvar(Endereco endereco)
        {
            StringBuilder sqlComando = new StringBuilder();

            var i = endereco.Logradouro.IndexOf(" ");
            if (i == -1) i = 0;
            var logradouro = endereco.Logradouro.Substring(i).Trim();
            var tipo = endereco.Logradouro.Substring(0, i);
            var tipoN = tipo;

            if (logradouro == "")
            {
                tipoN = "MNC";
                tipo = "MUNICIPIO";
                logradouro = endereco.Municipio.Nome;
            }

            sqlComando.Append("REPLACE INTO topfixo.ger_cep");
            sqlComando.Append(" SET");
            sqlComando.Append(" LOGRADOURO=@LOGRADOURO,");
            sqlComando.Append(" ESTADO=@ESTADO,");
            sqlComando.Append(" BAIRRO=@BAIRRO,");
            sqlComando.Append(" CIDADE=@CIDADE,");
            sqlComando.Append(" TIPO=@TIPO,");
            sqlComando.Append(" CEP=@CEP,");
            sqlComando.Append(" TIPON=@TIPON,");
            sqlComando.Append(" intinerante='',");
            sqlComando.Append(" confiavel=0");

            _context.Database.Connection.Execute(sqlComando.ToString(), new {
                LOGRADOURO = logradouro,
                ESTADO = endereco.Municipio.Uf,
                BAIRRO = endereco.Bairro,
                CIDADE = endereco.Municipio.Nome,
                TIPO = tipo,
                CEP = endereco.Cep,
                TIPON = tipoN
            });
        }

        public Endereco ObterEnderecoUsina(int codUsina)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT");
            sqlComando.Append(" c.cep Cep, c.endereco Logradouro, c.num Numero, c.bairro Bairro");
            sqlComando.Append(" FROM fis_filial c");
            sqlComando.Append(" INNER JOIN con_usina f");
            sqlComando.Append(" ON c.emp_filial = f.emp_filial");
            sqlComando.Append(" WHERE");
            sqlComando.Append(" f.cod=@ID");

            return _context.Database.Connection.Query<Endereco>(sqlComando.ToString(), new { ID = codUsina })
                .FirstOrDefault();
        }

        public void SalvarMunicipio(Municipio municipio)
        {
            var sqlComando = municipio.MontarSqlInsert(_context);

            _context.Database.Connection.Execute(sqlComando);
            _context.Database.Connection.GravarLogGeral(_identityHelperService.GetUserName(), "ger_municipio", sqlComando.ToString());
        }
    }
}
