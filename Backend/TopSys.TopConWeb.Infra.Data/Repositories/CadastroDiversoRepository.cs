using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class CadastroDiversoRepository : RepositoryBase<CadastroDiverso>, ICadastroDiversoRepository
    {
        public CadastroDiversoRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public ICollection<CadastroDiverso> ListarAndares()
        {
            return _context
                .CadastrosDiversos
                .Where(t => t.Aplicativo == "topcon" && t.ProgramaNumero == 6115 && t.ProgramaCampo == "andar")
                .AsNoTracking()
                .OrderBy(t => t.Descricao)
                .ToList();
        }

        public ICollection<CadastroDiverso> ListarCondicoesPagamento()
        {
            return _context
                .CadastrosDiversos
                .Where(t => t.Aplicativo == "" && t.ProgramaNumero == 12 && t.ProgramaCampo == "base_nf")
                .AsNoTracking()
                .ToList();
        }

        public ICollection<CadastroDiverso> ListarDiasDaSemanaFixo()
        {
            return _context
                .CadastrosDiversos
                .Where(t => t.Aplicativo == "" && t.ProgramaNumero == 12 && t.ProgramaCampo == "dia_semana_fixo" && t.Codigo != "0")
                .AsNoTracking()
                .ToList();
        }

        public ICollection<CadastroDiverso> ListarOpcoesDeVencimentoEmDiaNaoUtil()
        {
            return _context
                .CadastrosDiversos
                .Where(t => t.Aplicativo == "topcompras" && t.ProgramaNumero == 12 && t.ProgramaCampo == "dia_util_vcto")
                .AsNoTracking()
                .ToList();
        }

        public ICollection<CadastroDiverso> ListarQuantidadeDeCorposDeProva()
        {
            return _context
                .CadastrosDiversos
                .Where(t => t.Aplicativo == "topcon" && t.ProgramaNumero == 0 && t.ProgramaCampo == "cp_prv_qtde")
                .AsNoTracking()
                .ToList();
        }

        public ICollection<CadastroDiverso> ListarModeloDocumentoRemessaConcreto()
        {
            return _context
                .CadastrosDiversos
                .Where(t => t.Aplicativo == "topcon" && t.ProgramaNumero == 6111 && t.ProgramaCampo == "modelo_doc_rem_concr")
                .AsNoTracking()
                .ToList();
        }

    }
}
