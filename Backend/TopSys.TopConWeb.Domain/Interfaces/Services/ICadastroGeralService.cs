using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface ICadastroGeralService : IServiceBase<CadastroGeral>
    {
        ICollection<CadastroGeral> ListarMotivosBloqueioInterveniente();
        ICollection<CadastroGeral> ListarViasCaptacao();
        ICollection<CadastroGeral> ListarFuncoes();
        ICollection<CadastroGeral> ListarTipoObra();
        ICollection<CadastroGeral> ListarPorteObra();
        ICollection<CadastroGeral> ListarTemposAprovacaoMedicao();
        ICollection<CadastroGeral> Listar(ECadastroGeralTipo type);
        CadastroGeral ObterPorId(int id, ECadastroGeralTipo type, bool tracking = false);
        CadastroGeral ObterPorExternalId(string externalId, ECadastroGeralTipo type, bool tracking = false);
        bool EstaEmUsoCadastroGeral(int id, ECadastroGeralTipo type); 
        void AtualizarId(int idAtual, int idNovo);
        int ObterProximoCodigo(ECadastroGeralTipo type);

        string ObterDescricaoEquipamentoBombaPorObraBomba(int obraUsina, int obraNumero, int obraSequencia, int obraVersao = 0);
    }
}
