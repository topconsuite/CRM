using System.Collections.Generic;
using System.Linq;
using TopSys.TopConWeb.Domain.Entities.Integracao;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Services
{
    public class IntegracaoService : IIntegracaoService
    {

        private readonly IIntegracaoRepository _integracaoRepository;
        private readonly IIntervenienteRepository _intervenienteRepository;

        public IntegracaoService(IIntegracaoRepository integracaoRepository, IIntervenienteRepository intervenienteRepository)
        {
            _integracaoRepository = integracaoRepository;
            _intervenienteRepository = intervenienteRepository;
        }

        public void SalvarRetornoHorariosBetoneiraLab360(IntegracaoRetornoHorariosBetoneira retorno)
        {
            _integracaoRepository.SalvarRetornoHorariosBetoneiraLab360(retorno);
        }

        public IEnumerable<TotaisContrato> ObterTotaisRemessaContratosPorCliente(int codigoCliente)
        {
            return _integracaoRepository.ObterTotaisRemessaContratosPorCliente(codigoCliente);
        }

        public TotaisContasRecebereRemessasPorCliente ObterTotaisContasReceberPorCliente(string cpfCnpj, string inscricaoEstadual)
        {

            var cliente = _intervenienteRepository.ObterPorCpfCnpj(cpfCnpj, inscricaoEstadual);

            var total = new TotaisContasRecebereRemessasPorCliente();

            if (cliente == null)
            {
                AssertionConcern.Notify("Cliente", "Cliente não localizado");
                return total;
            }

            total.Cliente = $"{cliente.Codigo} - {cliente.Razao}";
            total.Credito = _integracaoRepository.obterInformacoesCreditoPorCliente(cliente.Codigo);
            total.TotaisPorContrato = _integracaoRepository.ObterTotaisRemessaContratosPorCliente(cliente.Codigo);
            total.QuantidadeContratos = total.TotaisPorContrato.Count();

            foreach(var resumoContrato in total.TotaisPorContrato)
            {
                total.ValorFaturamentoTotal += resumoContrato.FaturamentoContrato;
                total.VolumeM3ContratadoTotal += resumoContrato.VolumeM3Contratado;
                total.VolumeM3EntregueTotal += resumoContrato.VolumeM3Entregue;
            }

            return total;
            

        }

    }
}
