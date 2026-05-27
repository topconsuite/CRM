using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class CadastroGeralService : ServiceBase<CadastroGeral>, ICadastroGeralService
    {
        private readonly ICadastroGeralRepository _cadastroGeralRepository;

        public string ObterDescricaoEquipamentoBombaPorObraBomba(int obraUsina, int obraNumero, int obraSequencia, int obraVersao = 0)
        {
            return _cadastroGeralRepository.ObterDescricaoEquipamentoBombaPorObraBomba(obraUsina, obraNumero, obraSequencia, obraVersao);
        }

        public CadastroGeralService(ICadastroGeralRepository cadastroGeralRepository) : base(cadastroGeralRepository)
        {
            _cadastroGeralRepository = cadastroGeralRepository;
        }

        public ICollection<CadastroGeral> ListarFuncoes()
        {
            return _cadastroGeralRepository.ListarFuncoes();
        }

        public ICollection<CadastroGeral> ListarMotivosBloqueioInterveniente()
        {
            return _cadastroGeralRepository.ListarMotivosBloqueioInterveniente();
        }

        public ICollection<CadastroGeral> ListarViasCaptacao()
        {
            return _cadastroGeralRepository.ListarViasCaptacao();
        }

        public ICollection<CadastroGeral> ListarTipoObra()
        {
            return _cadastroGeralRepository.ListarTipoObra();
        }

        public ICollection<CadastroGeral> ListarPorteObra()
        {
            return _cadastroGeralRepository.ListarPorteObra();
        }

        public ICollection<CadastroGeral> ListarTemposAprovacaoMedicao()
        {
            return _cadastroGeralRepository.ListarTemposAprovacaoMedicao();
        }

        public ICollection<CadastroGeral> Listar(ECadastroGeralTipo type)
        {
            switch (type)
            {
                case ECadastroGeralTipo.EquipamentoTipo:
                    return _cadastroGeralRepository.ListarEquipamentoTipo();
                case ECadastroGeralTipo.FuncionarioDepartamento:
                    return _cadastroGeralRepository.ListarFuncionarioDepartamento();
                case ECadastroGeralTipo.FuncionarioFuncao:
                    return _cadastroGeralRepository.ListarFuncionarioFuncao();
                case ECadastroGeralTipo.FuncionarioStatus:
                    return _cadastroGeralRepository.ListarFuncionarioStatus();
                default:
                    return null;
            }
        }

        public CadastroGeral ObterPorId(int id, ECadastroGeralTipo type, bool tracking = false)
        {
            switch(type)
            {
                case ECadastroGeralTipo.FuncionarioFuncao:
                    return _cadastroGeralRepository.ObterPorIdFuncionarioFuncao(id, tracking);
                case ECadastroGeralTipo.FuncionarioDepartamento:
                    return _cadastroGeralRepository.ObterPorIdFuncionarioDepartamento(id, tracking);
                case ECadastroGeralTipo.FuncionarioStatus:
                    return _cadastroGeralRepository.ObterPorIdFuncionarioStatus(id, tracking);
                case ECadastroGeralTipo.EquipamentoTipo:
                    return _cadastroGeralRepository.ObterPorIdEquipamentoTipo(id, tracking);
                default:
                    return null;
            }
        }

        public CadastroGeral ObterPorExternalId(string externalId, ECadastroGeralTipo type, bool tracking = false)
        {
            switch(type)
            {
                case ECadastroGeralTipo.FuncionarioFuncao:
                    return _cadastroGeralRepository.ObterPorExternalIdFuncionarioFuncao(externalId, tracking);
                case ECadastroGeralTipo.FuncionarioDepartamento:
                    return _cadastroGeralRepository.ObterPorExternalIdFuncionarioDepartamento(externalId, tracking);
                case ECadastroGeralTipo.FuncionarioStatus:
                    return _cadastroGeralRepository.ObterPorExternalIdFuncionarioStatus(externalId, tracking);
                case ECadastroGeralTipo.EquipamentoTipo:
                    return _cadastroGeralRepository.ObterPorExternalIdEquipamentoTipo(externalId, tracking);
                default:
                    return null;
            }
        }

        public bool EstaEmUsoCadastroGeral(int id, ECadastroGeralTipo type)
        {
            switch(type)
            {
                case ECadastroGeralTipo.FuncionarioFuncao:
                    return _cadastroGeralRepository.EstaEmUsoFuncionarioFuncao(id);
                case ECadastroGeralTipo.FuncionarioDepartamento:
                    return _cadastroGeralRepository.EstaEmUsoFuncionarioDepartamento(id);
                case ECadastroGeralTipo.FuncionarioStatus:
                    return _cadastroGeralRepository.EstaEmUsoFuncionarioStatus(id);
                case ECadastroGeralTipo.EquipamentoTipo:
                    return _cadastroGeralRepository.EstaEmUsoEquipamentoTipo(id);
                default:
                    return true;
            }
        }

        public void AtualizarId(int idAtual, int idNovo)
        {
            _cadastroGeralRepository.AtualizarId(idAtual, idNovo);
        }

        public int ObterProximoCodigo(ECadastroGeralTipo type)
        {
            return _cadastroGeralRepository.ObterProximoCodigo(type);
        }
    }
}
