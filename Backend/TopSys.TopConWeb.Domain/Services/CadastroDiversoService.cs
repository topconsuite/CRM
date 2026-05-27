using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class CadastroDiversoService : ServiceBase<CadastroDiverso>, ICadastroDiversoService
    {
        private readonly ICadastroDiversoRepository _cadastroDiversoRepository;

        public CadastroDiversoService(ICadastroDiversoRepository cadastroDiversoRepository) : base(cadastroDiversoRepository)
        {
            _cadastroDiversoRepository = cadastroDiversoRepository;
        }

        public ICollection<CadastroDiverso> ListarAndares()
        {
            return _cadastroDiversoRepository.ListarAndares();
        }

        public ICollection<CadastroDiverso> ListarCondicoesPagamento()
        {
            return _cadastroDiversoRepository.ListarCondicoesPagamento();
        }

        public ICollection<CadastroDiverso> ListarDiasDaSemanaFixo()
        {
            return _cadastroDiversoRepository.ListarDiasDaSemanaFixo();
        }

        public ICollection<CadastroDiverso> ListarOpcoesDeVencimentoEmDiaNaoUtil()
        {
            return _cadastroDiversoRepository.ListarOpcoesDeVencimentoEmDiaNaoUtil();
        }

        public ICollection<CadastroDiverso> ListarQuantidadeDeCorposDeProva()
        {
            return _cadastroDiversoRepository.ListarQuantidadeDeCorposDeProva();
        }

        public ICollection<CadastroDiverso> ListarModeloDocumentoRemessaConcreto()
        {
            return _cadastroDiversoRepository.ListarModeloDocumentoRemessaConcreto();
        }
    }
}
