using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface ICadastroGeralRepository : IRepositoryBase<CadastroGeral>
    {

        void AtualizarId(int idAtual, int idNovo);

        ICollection<CadastroGeral> ListarMotivosBloqueioInterveniente();
        ICollection<CadastroGeral> ListarViasCaptacao();
        ICollection<CadastroGeral> ListarFuncoes();
        ICollection<CadastroGeral> ListarTipoObra();
        ICollection<CadastroGeral> ListarPorteObra();
        ICollection<CadastroGeral> ListarTemposAprovacaoMedicao();
        ICollection<CadastroGeral> ListarEquipamentoTipo();
        ICollection<CadastroGeral> ListarFuncionarioDepartamento();
        ICollection<CadastroGeral> ListarFuncionarioFuncao();
        ICollection<CadastroGeral> ListarFuncionarioStatus();


        string ObterDescricaoEquipamentoBombaPorObraBomba(int obraUsina, int obraNumero, int obraSequencia, int obraVersao = 0);

        CadastroGeral ObterPorIdFuncionarioFuncao(int id, bool tracking = false);
        CadastroGeral ObterPorExternalIdFuncionarioFuncao(string externalId, bool tracking = false);

        CadastroGeral ObterPorIdFuncionarioDepartamento(int id, bool tracking = false);
        CadastroGeral ObterPorExternalIdFuncionarioDepartamento(string externalId, bool tracking = false);

        CadastroGeral ObterPorIdFuncionarioStatus(int id, bool tracking = false);
        CadastroGeral ObterPorExternalIdFuncionarioStatus(string externalId, bool tracking = false);

        CadastroGeral ObterPorIdEquipamentoTipo(int id, bool tracking = false);
        CadastroGeral ObterPorExternalIdEquipamentoTipo(string externalId, bool tracking = false);

        bool EstaEmUsoFuncionarioStatus(int id);
        bool EstaEmUsoFuncionarioDepartamento(int id);
        bool EstaEmUsoFuncionarioFuncao(int id);
        bool EstaEmUsoEquipamentoTipo(int id);
        int ObterProximoCodigo(ECadastroGeralTipo type);

    }
}