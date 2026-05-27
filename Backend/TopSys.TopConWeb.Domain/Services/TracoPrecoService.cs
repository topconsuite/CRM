using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class TracoPrecoService : ServiceBase<TracoPreco>, ITracoPrecoService
    {
        private readonly ITracoPrecoRepository _tracoPrecoRepository;
        private readonly IUsinaRepository _usinaRepository;
        private readonly ICustoServicoRepository _custoServicoRepository;
        private readonly IParametroRepository _parametroRepository;
        private readonly ICalculoImpostosService _calculoImpostosService;
        private readonly IUsinaService _usinaService;


        public TracoPrecoService(ITracoPrecoRepository tracoPrecoRepository, IUsinaRepository usinaRepository,
            ICustoServicoRepository custoServicoRepository, IParametroRepository parametroRepository, ICalculoImpostosService calculoImpostosService, IUsinaService usinaService)
            : base(tracoPrecoRepository)
        {
            _tracoPrecoRepository = tracoPrecoRepository;
            _usinaRepository = usinaRepository;
            _custoServicoRepository = custoServicoRepository;
            _parametroRepository = parametroRepository;
            _calculoImpostosService = calculoImpostosService;
            _usinaService = usinaService;
        }

        public DateTime ObterDataVigenciaPorDataBaseUsina(DateTime dataBase, int idUsina)
        {
            return _tracoPrecoRepository.ObterDataVigenciaPorDataBaseUsina(dataBase, idUsina);
        }

        public int ObterNumeroTabelaVigentePorDataBaseUsina(DateTime dataBase, int idUsina)
        {
            return _tracoPrecoRepository.ObterNumeroTabelaVigentePorDataBaseUsina(dataBase, idUsina);
        }

        public int ObterNumeroTabelaVigentePorUsina(int idUsina)
        {
            return _tracoPrecoRepository.ObterNumeroTabelaVigentePorUsina(idUsina);
        }

        public IEnumerable<TracoPreco> ListarPorDataUsina(DateTime data, int idUsina)
        {
            var dataVigenciaTabela = _tracoPrecoRepository.ObterDataVigenciaPorDataBaseUsina(data, idUsina);

            return _tracoPrecoRepository.ListarPorDataUsina(dataVigenciaTabela, idUsina);
        }

        public PagedList<TracoPreco> ListarPorDataUsinaPagina(DateTime data, int idUsina, int pagina, int porPagina, int? segmentacao, Expression<Func<TracoPreco, bool>> filter)
        {

            DateTime dataVigenciaTabela;

            IEnumerable<string> tracosAtivos;

            if (idUsina > 0)
            {
                tracosAtivos = _tracoPrecoRepository.ListarTracosAtivos(idUsina);

                dataVigenciaTabela = _tracoPrecoRepository.ObterDataVigenciaPorDataBaseUsina(data, idUsina);
                return _tracoPrecoRepository.ListarPorDataUsinaPagina(dataVigenciaTabela, idUsina, pagina, porPagina, segmentacao, tracosAtivos, filter);
            } 
            else
            {
                tracosAtivos = _tracoPrecoRepository.ListarTracosAtivos();

                return _tracoPrecoRepository.ListarTodosPorPagina(pagina, porPagina, segmentacao, tracosAtivos, filter);
            }


        }

        public IEnumerable<TracoPreco> ListarPrecosAtuaisPorObra(int idUsina, int obraNumero)
        {

            var obra = _tracoPrecoRepository.ListarFiltrados<Obra>(t => t.UsinaCodigo == idUsina
               && t.Numero == obraNumero, t => t.ObraTracos).FirstOrDefault();

            var numeroTabelaVigente = _tracoPrecoRepository.ObterNumeroTabelaVigentePorUsina(obra.UsinaEntregaCodigo);

            var tracoPrecos = new List<TracoPreco>();

            foreach (var traco in obra.ObraTracos)
            {
                var tracoPreco = ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(obra.UsinaEntregaCodigo, traco.UsoCodigo, traco.PedraCodigo, traco.SlumpCodigo, traco.ResistenciaTipoCodigo, traco.Fck, traco.Consumo, obra);
                if (tracoPreco != null) tracoPrecos.Add(tracoPreco);
            }

            return tracoPrecos;
        }

        public IEnumerable<TracoPreco> ListarPrecosAtuaisPorObra(int numVersao, int idUsina, int obraNumero)
        {

            var obra = _tracoPrecoRepository.ListarFiltrados<ObraVersao>(t => t.NumeroVersao == numVersao && t.UsinaCodigo == idUsina
            && t.Numero == obraNumero, t => t.ObraTracos).FirstOrDefault();

            var numeroTabelaVigente = _tracoPrecoRepository.ObterNumeroTabelaVigentePorUsina(obra.UsinaEntregaCodigo);

            var tracoPrecos = new List<TracoPreco>();

            foreach (var traco in obra.ObraTracos)
            {
                var tracoPreco = ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(obra.UsinaEntregaCodigo, traco.UsoCodigo, traco.PedraCodigo, traco.SlumpCodigo, traco.ResistenciaTipoCodigo, traco.Fck, traco.Consumo, obra);
                if (tracoPreco != null) tracoPrecos.Add(tracoPreco);
            }

            return tracoPrecos;
        }

        public int ObterStatusTracoPorObraVersao(ObraTracoVersao traco, ObraVersao obra)
        {

            var tracoPreco = ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(
                     obra.UsinaEntregaCodigo,
                     traco.UsoCodigo,
                     traco.PedraCodigo,
                     traco.SlumpCodigo,
                     traco.ResistenciaTipoCodigo,
                     traco.Fck,
                     traco.Consumo, obra);

            if (tracoPreco is null)
                return 0;

            return ObterStatusPorTracoPreco(tracoPreco);

        }

        public int ObterStatusTracoPorObra(ObraTraco traco, Obra obra)
        {

            var tracoPreco = ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(
                    obra.UsinaEntregaCodigo,
                    traco.UsoCodigo,
                    traco.PedraCodigo,
                    traco.SlumpCodigo,
                    traco.ResistenciaTipoCodigo,
                    traco.Fck,
                    traco.Consumo, obra);

            if (tracoPreco is null)
                return 0;

            return ObterStatusPorTracoPreco(tracoPreco);

        }

        public TracoPreco ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo, Obra obra)
        {
            var numeroTabelaVigente = _tracoPrecoRepository.ObterNumeroTabelaVigentePorUsina(idUsina);
            var tracoPreco = _tracoPrecoRepository
                .ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(numeroTabelaVigente, idUsina, idUso, idPedra, idSlump, idResistenciaTipo, mpa, consumo);
            var utilizaCalculoPrecoTabelaPorUsina = _parametroRepository.ObterParametroN("web", "UtilizaCalculoPrecoTabelaPorUsina").Equals("1");

            if (utilizaCalculoPrecoTabelaPorUsina)
            {
                var custoServico = _custoServicoRepository.ObterPorUsina(idUsina);
                var usina = _usinaRepository.ObterPorId(idUsina);

                var totalMCC = tracoPreco.CustoMaterial;
                var volumePorCarga = obra.VolumePorCarga;

                if (volumePorCarga == 0) volumePorCarga = 8;

                float tempoCicloPorcarga = (int)Math.Round(obra.TempoAteAObra + obra.TempoAteAObra * (usina.PorcentagemRetornoObra / 100) + obra.TempoBtNaObra + usina.PrazoPesagem);
                var custoTransporte = (custoServico.CustoMedioHoraTransporte * (tempoCicloPorcarga / 60) * (obra.VolumeEstimado / volumePorCarga)) / obra.VolumeEstimado;

                var baseCalculoMarkup = totalMCC + custoTransporte + custoServico.CustoMedioServico;
                var valorMarkup = baseCalculoMarkup / (1 - custoServico.Markup / 100) - baseCalculoMarkup;
                var baseCalculoImposto = baseCalculoMarkup + valorMarkup;

                var empresa = _usinaService.ObterPorId<Empresa>(usina.EmpresaCodigo);
                var municipio = _usinaService.ObterPorId<Municipio>(obra.EnderecoMunicipioCodigo);

                var aliquota = empresa.SimplesNacional ? empresa.AliquotaSimplesNacional : municipio.AliquotaIss;
                var totalImpostos  = baseCalculoImposto / (1 - (custoServico.ImpostoEstadual + custoServico.ImpostoFederal + aliquota) / 100) - baseCalculoImposto;

                //Necessário Refatorar depois TC-4205
                //var impostosFedereaisEEstaduais = baseCalculoImposto / (1 - (custoServico.ImpostoEstadual + custoServico.ImpostoFederal + 5) / 100) - baseCalculoImposto;
                //var impostosMunicipais = _calculoImpostosService.CalcularIss(obra, baseCalculoImposto, totalMCC, false, true);
                //var totalImpostos = impostosFedereaisEEstaduais + impostosMunicipais;

                var precoSugerido = baseCalculoImposto + totalImpostos;

                tracoPreco.M3Preco = precoSugerido;
                tracoPreco.M3PrecoRecalculo = precoSugerido;
            }

            return tracoPreco;
        }

        public TracoPreco ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo, ObraVersao obra)
        {
            var numeroTabelaVigente = _tracoPrecoRepository.ObterNumeroTabelaVigentePorUsina(idUsina);
            var tracoPreco = _tracoPrecoRepository
                .ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(numeroTabelaVigente, idUsina, idUso, idPedra, idSlump, idResistenciaTipo, mpa, consumo);
            var utilizaCalculoPrecoTabelaPorUsina = _parametroRepository.ObterParametroN("web", "UtilizaCalculoPrecoTabelaPorUsina").Equals("1");

            if (utilizaCalculoPrecoTabelaPorUsina)
            {
                var custoServico = _custoServicoRepository.ObterPorUsina(idUsina);
                var usina = _usinaRepository.ObterPorId(idUsina);

                var totalMCC = tracoPreco.CustoMaterial;
                var volumePorCarga = obra.VolumePorCarga;

                if (volumePorCarga == 0) volumePorCarga = 8;

                var tempoCicloPorcarga = (obra.TempoAteAObra + obra.TempoAteAObra * (usina.PorcentagemRetornoObra / 100) + obra.TempoBtNaObra + usina.PrazoPesagem);
                var custoTransporte = (custoServico.CustoMedioHoraTransporte * (tempoCicloPorcarga / 60) * (obra.VolumeEstimado / volumePorCarga)) / obra.VolumeEstimado;

                var baseCalculoMarkup = totalMCC + custoTransporte + custoServico.CustoMedioServico;
                var valorMarkup = baseCalculoMarkup / (1 - custoServico.Markup / 100) - baseCalculoMarkup;
                var baseCalculoImposto = baseCalculoMarkup + valorMarkup;

                var impostosFedereaisEEstaduais = baseCalculoImposto / (1 - (custoServico.ImpostoEstadual + custoServico.ImpostoFederal) / 100) - baseCalculoImposto;
                var impostosMunicipais = _calculoImpostosService.CalcularIss(obra, baseCalculoImposto, totalMCC, false, true);
                var totalImpostos = impostosFedereaisEEstaduais + impostosMunicipais;

                var precoSugerido = baseCalculoImposto + totalImpostos;

                tracoPreco.M3Preco = precoSugerido;
                tracoPreco.M3PrecoRecalculo = precoSugerido;
            }

            return tracoPreco;
        }

        public TracoPreco ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo, bool tracking = false)
        {
            var numeroTabelaVigente = _tracoPrecoRepository.ObterNumeroTabelaVigentePorUsina(idUsina);

            return _tracoPrecoRepository.ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(numeroTabelaVigente, idUsina, idUso, idPedra, idSlump, idResistenciaTipo, mpa, consumo, tracking);
        }

        public IEnumerable<TracoPrecoNumeracaoProduto> ListarNumeracoesProdutoPorNumeroTabelaUsina(int idUsina, int idSegmentacao)
        {
            var numeroTabelaVigente = _tracoPrecoRepository.ObterNumeroTabelaVigentePorUsina(idUsina);

            return _tracoPrecoRepository.ListarNumeracoesProdutoPorNumeroTabelaUsina(numeroTabelaVigente, idUsina, idSegmentacao);
        }

        public IEnumerable<TracoPrecoNumeracaoProduto> ListarNumeracoesProduto()
        {
            return _tracoPrecoRepository.ListarNumeracoesProduto();
        }

        public int ObterStatusPorTracoPreco(TracoPreco tracoPreco)
        {
            return _tracoPrecoRepository.ObterStatusPorTracoPreco(tracoPreco);
        }

        public int ObterStatusPorNumeracaoProduto(int idUsina, int numeracaoProduto, Obra obra)
        {

            var tracoPreco = ObterPorNumeracaoProduto(idUsina, numeracaoProduto, obra);

            if (tracoPreco is null)
                return 0;

            return _tracoPrecoRepository.ObterStatusPorTracoPreco(tracoPreco);

        }

        public TracoPreco ObterPorNumeracaoProduto(int idUsina, int numeracaoProduto, Obra obra)
        {
            var numeroTabelaVigente = _tracoPrecoRepository.ObterNumeroTabelaVigentePorUsina(idUsina);

            var tracoPreco = _tracoPrecoRepository.ObterPorNumeracaoProduto(numeroTabelaVigente, idUsina, numeracaoProduto);

            var utilizaCalculoPrecoTabelaPorUsina = _parametroRepository.ObterParametroN("web", "UtilizaCalculoPrecoTabelaPorUsina").Equals("1");

            if (utilizaCalculoPrecoTabelaPorUsina)
            {
                var custoServico = _custoServicoRepository.ObterPorUsina(idUsina);
                var usina = _usinaRepository.ObterPorId(idUsina);

                var totalMCC = tracoPreco.CustoMaterial;
                var volumePorCarga = obra.VolumePorCarga;

                if (volumePorCarga == 0) volumePorCarga = 8;

                float tempoCicloPorcarga = (int)Math.Round(obra.TempoAteAObra + obra.TempoAteAObra * (usina.PorcentagemRetornoObra / 100) + obra.TempoBtNaObra + usina.PrazoPesagem);
                var custoTransporte = (custoServico.CustoMedioHoraTransporte * (tempoCicloPorcarga / 60) * (obra.VolumeEstimado / volumePorCarga)) / obra.VolumeEstimado;

                var baseCalculoMarkup = totalMCC + custoTransporte + custoServico.CustoMedioServico;
                var valorMarkup = baseCalculoMarkup / (1 - custoServico.Markup / 100) - baseCalculoMarkup;
                var baseCalculoImposto = baseCalculoMarkup + valorMarkup;

                var empresa = _usinaService.ObterPorId<Empresa>(usina.EmpresaCodigo);
                var municipio = _usinaService.ObterPorId<Municipio>(obra.EnderecoMunicipioCodigo);

                var aliquota = empresa.SimplesNacional ? empresa.AliquotaSimplesNacional : (municipio != null ? municipio.AliquotaIss : 0);
                var totalImpostos = baseCalculoImposto / (1 - (custoServico.ImpostoEstadual + custoServico.ImpostoFederal + aliquota) / 100) - baseCalculoImposto;

                //Necessário Refatorar depois TC-4205
                //var impostosFedereaisEEstaduais = baseCalculoImposto / (1 - (custoServico.ImpostoEstadual + custoServico.ImpostoFederal + 5) / 100) - baseCalculoImposto;
                //var impostosMunicipais = _calculoImpostosService.CalcularIss(obra, baseCalculoImposto, totalMCC, false, true);
                //var totalImpostos = impostosFedereaisEEstaduais + impostosMunicipais;

                var precoSugerido = baseCalculoImposto + totalImpostos;

                tracoPreco.M3Preco = precoSugerido;
                tracoPreco.M3PrecoRecalculo = precoSugerido;
            }

            return tracoPreco;
        }

        public IEnumerable<Uso> ListarUsosPorNumeroTabelaUsina(int idUsina, int idSegmentacao)
        {
            var numeroTabelaVigente = _tracoPrecoRepository.ObterNumeroTabelaVigentePorUsina(idUsina);

            return _tracoPrecoRepository.ListarUsosPorNumeroTabelaUsina(numeroTabelaVigente, idUsina, idSegmentacao);
        }

        public IEnumerable<Pedra> ListarPedrasPorNumeroTabelaUsinaUso(int idUsina, int idUso)
        {
            var numeroTabelaVigente = _tracoPrecoRepository.ObterNumeroTabelaVigentePorUsina(idUsina);

            return _tracoPrecoRepository.ListarPedrasPorNumeroTabelaUsinaUso(numeroTabelaVigente, idUsina, idUso);
        }

        public IEnumerable<SlumpReal> ListarSlumpsPorNumeroTabelaUsinaUsoPedra(int idUsina, int idUso, int idPedra)
        {
            var numeroTabelaVigente = _tracoPrecoRepository.ObterNumeroTabelaVigentePorUsina(idUsina);

            return _tracoPrecoRepository.ListarSlumpsPorNumeroTabelaUsinaUsoPedra(numeroTabelaVigente, idUsina, idUso, idPedra);
        }

        public IEnumerable<Slump> ListarSlumpsNominaisPorNumeroTabelaUsinaUsoPedra(int idUsina, int idUso, int idPedra)
        {
            var numeroTabelaVigente = _tracoPrecoRepository.ObterNumeroTabelaVigentePorUsina(idUsina);

            return _tracoPrecoRepository.ListarSlumpsNominaisPorNumeroTabelaUsinaUsoPedra(numeroTabelaVigente, idUsina, idUso, idPedra);
        }

        public IEnumerable<ResistenciaTipo> ListarResistenciaTiposPorNumeroTabelaUsinaUsoPedraSlump(int idUsina, int idUso, int idPedra, int idSlump)
        {
            var numeroTabelaVigente = _tracoPrecoRepository.ObterNumeroTabelaVigentePorUsina(idUsina);

            return _tracoPrecoRepository.ListarResistenciaTiposPorNumeroTabelaUsinaUsoPedraSlump(numeroTabelaVigente, idUsina, idUso, idPedra, idSlump);
        }

        public IEnumerable<float> ListarMpasPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipo(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo)
        {
            var numeroTabelaVigente = _tracoPrecoRepository.ObterNumeroTabelaVigentePorUsina(idUsina);

            return _tracoPrecoRepository.ListarMpasPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipo(numeroTabelaVigente, idUsina, idUso, idPedra, idSlump, idResistenciaTipo);
        }

        public IEnumerable<int> ListarConsumosPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipo(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo)
        {
            var numeroTabelaVigente = _tracoPrecoRepository.ObterNumeroTabelaVigentePorUsina(idUsina);

            return _tracoPrecoRepository.ListarConsumosPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipo(numeroTabelaVigente, idUsina, idUso, idPedra, idSlump, idResistenciaTipo);
        }

        public float ObterValorAdicionalM3PorUsinaVolumePrecoUnitarioTabela(int idUsina, float volume, float precoUnitarioTabela)
        {
            return _tracoPrecoRepository.ObterValorAdicionalM3PorUsinaVolumePrecoUnitarioTabela(idUsina, volume, precoUnitarioTabela);
        }

        public TracoParticularidades ObterParticularidadesPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo , float mpa, int consumo)
        {
            return _tracoPrecoRepository.ObterParticularidadesPorUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(idUsina, idUso, idPedra, idSlump, idResistenciaTipo, mpa, consumo);
        }

        public TracoPreco ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(int numeroTabela, int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo)
        {
            return _tracoPrecoRepository.ObterPorNumeroTabelaUsinaUsoPedraSlumpResistenciaTipoMpaConsumo(numeroTabela, idUsina, idUso, idPedra, idSlump, idResistenciaTipo, mpa, consumo, true);
        }

        public void SalvarLogUpdate(TracoPreco tracoPreco, string usuario)
        {
            _tracoPrecoRepository.SalvarLogUpdate(tracoPreco, usuario);
        }

        public IEnumerable<Uso> ListarUsosPorSegmentacao(int idSegmentacao)
        {
            return _tracoPrecoRepository.ListarUsosPorSegmentacao(idSegmentacao);
        }
        
        public int ObterNumeracaoFamilia(int idUsina, int idUso, int idPedra, int idSlump, int idResistenciaTipo, float mpa, int consumo)
        {
            return _tracoPrecoRepository.ObterNumeracaoFamilia(idUsina, idUso, idPedra, idSlump, idResistenciaTipo, mpa, consumo);
        }
    }
}
