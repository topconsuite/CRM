using System;
using System.Collections.Generic;
using System.Linq;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class ObraTaxaService : ServiceBase<ObraTaxa>, IObraTaxaService
    {
        private readonly float _volumePorCargaDefault = 8f;

        private readonly IObraTaxaRepository _obraTaxaRepository;
        private readonly IObraRepository _obraRepository;

        public ObraTaxaService(IObraTaxaRepository taxaExtraRepository, IObraRepository obraRepository) : base(taxaExtraRepository)
        {
            _obraTaxaRepository = taxaExtraRepository;
            _obraRepository = obraRepository;
        }

        public void AprovarTaxas(string usuario, ICollection<ObraTaxa> obraTaxas)
        {

            ObraTaxa _obraTaxa;

            foreach(var obraTaxa in obraTaxas)
            {
                switch (obraTaxa.StatusAprovacao)
                {
                    case (EStatusAprovacao.Aprovado):
                        _obraTaxa = _obraTaxaRepository.ObterPorId(obraTaxa.UsinaCodigo, obraTaxa.ObraCodigo, obraTaxa.Sequencia);
                        _obraTaxaRepository.AtualizarObraTaxa(_obraTaxa);
                        _obraTaxa.Aprovar(usuario);
                        break;
                    case (EStatusAprovacao.Reprovado):
                        _obraTaxa = _obraTaxaRepository.ObterPorId(obraTaxa.UsinaCodigo, obraTaxa.ObraCodigo, obraTaxa.Sequencia);
                        _obraTaxaRepository.AtualizarObraTaxa(_obraTaxa);
                        _obraTaxa.Reprovar(usuario);
                        break;
                }
            }

        }

        public void AprovarTaxas(string usuario, ICollection<ObraTaxaVersao> obraTaxas, int numVersao)
        {
            ObraTaxaVersao _obraTaxa;

            foreach (var obraTaxa in obraTaxas)
            {
                switch (obraTaxa.StatusAprovacao)
                {
                    case (EStatusAprovacao.Aprovado):
                        _obraTaxa = _obraTaxaRepository.ObterPorId<ObraTaxaVersao>(numVersao, obraTaxa.UsinaCodigo, obraTaxa.ObraCodigo, obraTaxa.Sequencia);
                        _obraTaxaRepository.AtualizarObraTaxa(_obraTaxa);
                        _obraTaxa.Aprovar(usuario);
                        break;
                    case (EStatusAprovacao.Reprovado):
                        _obraTaxa = _obraTaxaRepository.ObterPorId<ObraTaxaVersao>(numVersao, obraTaxa.UsinaCodigo, obraTaxa.ObraCodigo, obraTaxa.Sequencia);
                        _obraTaxaRepository.AtualizarObraTaxa(_obraTaxa);
                        _obraTaxa.Reprovar(usuario);
                        break;
                }
            }

        }

        public ICollection<ObraTaxa> ListarByIdObra(int usinaEntregaCodigo, int obraCodigo)
        {

            var segmentacaoProposta = _obraRepository.ObterSegmentacaoPropostaPorObra(usinaEntregaCodigo, obraCodigo);

            var obraTaxas = _obraTaxaRepository.ListarByIdObra(usinaEntregaCodigo, obraCodigo, segmentacaoProposta);

            foreach (var obraTaxa in obraTaxas)
                obraTaxa.AtualizaStatusAprovacao();

            return obraTaxas;
        }

        public ICollection<ObraTaxaVersao> ListarByIdObra(int usinaEntregaCodigo, int obraCodigo, int numVersao)
        {
            var segmentacaoProposta = _obraRepository.ObterSegmentacaoPropostaPorObra(usinaEntregaCodigo, obraCodigo);

            var obraTaxas = _obraTaxaRepository.ListarByIdObra(usinaEntregaCodigo, obraCodigo, numVersao, segmentacaoProposta);

            foreach (var obraTaxaVersao in obraTaxas)
                obraTaxaVersao.AtualizaStatusAprovacao();

            return obraTaxas;
        }

        public ICollection<ObraTaxaVersao> ListarByIdObraVersao(int versao, int usinaEntregaCodigo, int obraCodigo)
        {
            var segmentacaoProposta = _obraRepository.ObterSegmentacaoPropostaPorObra(usinaEntregaCodigo, obraCodigo);

            var obraTaxas = _obraTaxaRepository.ListarByIdObra(versao, usinaEntregaCodigo, obraCodigo, segmentacaoProposta);

            foreach (var obraTaxa in obraTaxas)
                obraTaxa.AtualizaStatusAprovacao();

            return obraTaxas;
        }

        public ICollection<ObraTaxa> ListarTaxaPadraoByIdUsinaSegmento(int usinaEntregaCodigo, int idSegmentacao)
        {
            var taxasPadrao = _obraTaxaRepository.ListarTaxaPadraoByIdUsinaAndSegmento(usinaEntregaCodigo, idSegmentacao);

            return taxasPadrao;
        }

        public ICollection<ObraTaxa> ListarTaxaPadraoByIdUsina(int usinaEntregaCodigo)
        {
            var taxasPadrao = _obraTaxaRepository.ListarTaxaPadraoByIdUsina(usinaEntregaCodigo);

            return taxasPadrao;
        }

        public float ObterValorM3Faltante(bool temBomba, float volumeTotal, float volumePorCarga, int idUsina)
        {
            var taxas = ListarTaxaPadraoByIdUsina(idUsina);

            return ObterValorM3Faltante(temBomba, volumeTotal, volumePorCarga, taxas);
        }

        public float ObterValorM3Faltante(bool temBomba, float volumeTotal, float volumePorCarga, int idUsina, int obraNumero)
        {
            var taxas = obraNumero > 0 ? ListarByIdObra(idUsina, obraNumero) : ListarTaxaPadraoByIdUsina(idUsina);

            return ObterValorM3Faltante(temBomba, volumeTotal, volumePorCarga, taxas);
        }

        public float ObterValorM3Faltante(bool temBomba, float volumeTotal, float volumePorCarga, ICollection<ObraTaxa> obraTaxas)
        {
            var calculoPorM3FaltanteBombeado = obraTaxas.Where(t => t.Tipo == "M3 FALTANTE BOMBEADO").Count() > 0 && temBomba;

            var taxaM3Faltante = obraTaxas.FirstOrDefault(t => t.Tipo == (calculoPorM3FaltanteBombeado ? "M3 FALTANTE BOMBEADO" : "M3 FALTANTE"));

            if (taxaM3Faltante != null)
            {
                if (taxaM3Faltante.Selecionada == "S" && volumeTotal > 0)
                {
                    var volumeMinimo = float.Parse(taxaM3Faltante.Volume, System.Globalization.CultureInfo.InvariantCulture);

                    var menorVolume = (volumePorCarga > 0f ? Math.Min(volumePorCarga, volumeTotal) : 0f);
                    var volumeUltimaCarga = volumeTotal % volumePorCarga;

                    if (volumeUltimaCarga > 0 && volumeUltimaCarga < menorVolume)
                        menorVolume = volumeUltimaCarga;

                    if (menorVolume == 0f) menorVolume = volumePorCarga;

                    if (volumePorCarga > 0f && menorVolume < volumeMinimo)
                    {
                        var quantidadeCargas = (int)(volumeTotal / volumePorCarga);

                        var valorTaxa = (volumeMinimo > volumePorCarga ? (volumeMinimo - volumePorCarga) * taxaM3Faltante.Valor * quantidadeCargas : 0f);

                        if (volumeUltimaCarga > 0.1f)
                        {
                            valorTaxa += (volumeMinimo > volumeUltimaCarga ? (volumeMinimo - volumeUltimaCarga) * taxaM3Faltante.Valor : 0f);
                        }

                        return valorTaxa;
                    }
                    else if (volumePorCarga == 0f && volumeTotal < volumeMinimo)
                    {
                        return (volumeMinimo - volumeTotal) * taxaM3Faltante.Valor;
                    }
                }
            }

            return 0f;
        }

        public float ObterValorM3Faltante(bool temBomba, float volumeTotal, float volumePorCarga, ICollection<ObraTaxaVersao> obraTaxas)
        {
            var calculoPorM3FaltanteBombeado = obraTaxas.Where(t => t.Tipo == "M3 FALTANTE BOMBEADO").Count() > 0 && temBomba;

            var taxaM3Faltante = obraTaxas.FirstOrDefault(t => t.Tipo == (calculoPorM3FaltanteBombeado ? "M3 FALTANTE BOMBEADO" : "M3 FALTANTE"));

            if (taxaM3Faltante != null)
            {
                if (taxaM3Faltante.Selecionada == "S" && volumeTotal > 0)
                {
                    var volumeMinimo = float.Parse(taxaM3Faltante.Volume, System.Globalization.CultureInfo.InvariantCulture);

                    var menorVolume = (volumePorCarga > 0f ? Math.Min(volumePorCarga, volumeTotal) : 0f);
                    if (menorVolume == 0f) menorVolume = volumePorCarga;

                    if (volumePorCarga > 0f && menorVolume < volumeMinimo)
                    {
                        var quantidadeCargas = (int)(volumeTotal / volumePorCarga);
                        var volumeUltimaCarga = volumeTotal % volumePorCarga;

                        var valorTaxa = (volumeMinimo > volumePorCarga ? (volumeMinimo - volumePorCarga) * taxaM3Faltante.Valor * quantidadeCargas : 0f);

                        if (volumeUltimaCarga > 0.1f)
                        {
                            valorTaxa += (volumeMinimo > volumeUltimaCarga ? (volumeMinimo - volumeUltimaCarga) * taxaM3Faltante.Valor : 0f);
                        }

                        return valorTaxa;
                    }
                    else if (volumePorCarga == 0f && volumeTotal < volumeMinimo)
                    {
                        return (volumeMinimo - volumeTotal) * taxaM3Faltante.Valor;
                    }
                }
            }

            return 0f;
        }

        public float ObterValorAdicionalPorKmRodado(int distanciaUsina, float volumeTotal, float volumePorCarga, int idUsina, bool possuiBomba)
        {
            var taxas = ListarTaxaPadraoByIdUsina(idUsina);

            return ObterValorAdicionalPorKmRodado(distanciaUsina, volumeTotal, volumePorCarga, taxas, possuiBomba);
        }

        public float ObterValorAdicionalPorKmRodado(int distanciaUsina, float volumeTotal, float volumePorCarga, int idUsina, int obraNumero, bool possuiBomba)
        {
            var taxas = obraNumero > 0 ? ListarByIdObra(idUsina, obraNumero) : ListarTaxaPadraoByIdUsina(idUsina);

            return ObterValorAdicionalPorKmRodado(distanciaUsina, volumeTotal, volumePorCarga, taxas, possuiBomba);
        }

        public float ObterValorAdicionalPorKmRodado(int distanciaUsina, float volumeTotal, float volumePorCarga, ICollection<ObraTaxa> obraTaxas, bool possuiBomba)
        {
            var taxaAdicionalKmRodado = obraTaxas.FirstOrDefault(t => t.Tipo == "ADICIONAL KM RODADO");

            if (taxaAdicionalKmRodado != null)
            {
                if (taxaAdicionalKmRodado.Selecionada == "S" && volumeTotal > 0)
                {
                    var kmRodadoPorCarga = (distanciaUsina * 2);

                    if (kmRodadoPorCarga > taxaAdicionalKmRodado.AcimaDe)
                    {
                        var volumeCarga = (volumePorCarga > 0 ? volumePorCarga : _volumePorCargaDefault);
                        var quantidadeCargas = (int)(volumeTotal / volumeCarga);
                        if (possuiBomba) quantidadeCargas++;

                        var volumeUltimaCarga = volumeTotal % volumeCarga;
                        if (volumeUltimaCarga > 0) quantidadeCargas++;

                        return (quantidadeCargas * kmRodadoPorCarga * taxaAdicionalKmRodado.Valor);
                    }
                }
            }

            return 0f;
        }

        public float ObterValorAdicionalPorKmRodado(int distanciaUsina, float volumeTotal, float volumePorCarga, ICollection<ObraTaxaVersao> obraTaxas, bool possuiBomba)
        {
            var taxaAdicionalKmRodado = obraTaxas.FirstOrDefault(t => t.Tipo == "ADICIONAL KM RODADO");

            if (taxaAdicionalKmRodado != null)
            {
                if (taxaAdicionalKmRodado.Selecionada == "S" && volumeTotal > 0)
                {
                    var kmRodadoPorCarga = (distanciaUsina * 2);

                    if (kmRodadoPorCarga > taxaAdicionalKmRodado.AcimaDe)
                    {
                        var volumeCarga = (volumePorCarga > 0 ? volumePorCarga : _volumePorCargaDefault);
                        var quantidadeCargas = (int)(volumeTotal / volumeCarga);
                        if (possuiBomba) quantidadeCargas++;

                        var volumeUltimaCarga = volumeTotal % volumeCarga;
                        if (volumeUltimaCarga > 0) quantidadeCargas++;

                        return (quantidadeCargas * kmRodadoPorCarga * taxaAdicionalKmRodado.Valor);
                    }
                }
            }

            return 0f;
        }

        public void SalvarPersonalizada(ObraTaxa obraTaxa)
        {
            _obraTaxaRepository.SalvarPersonalizada(obraTaxa);
        }

        public void SalvarPersonalizada(ObraTaxaVersao obraTaxa)
        {
            _obraTaxaRepository.SalvarPersonalizada(obraTaxa);
        }

        public void DeletarPersonalizada(ObraTaxa obraTaxa)
        {
            _obraTaxaRepository.DeletarPersonalizada(obraTaxa);
        }

        public void DeletarPersonalizada(ObraTaxaVersao obraTaxa)
        {
            _obraTaxaRepository.DeletarPersonalizada(obraTaxa);
        }

        public float ObterValorAdicionalDomingosEFeriados(float valorConcretoTotal, int idUsina)
        {
            var taxas = ListarTaxaPadraoByIdUsina(idUsina);

            return ObterValorAdicionalDomingosEFeriados(valorConcretoTotal, taxas);
        }

        public float ObterValorAdicionalDomingosEFeriados(float valorConcretoTotal, int idUsina, int obraNumero)
        {
            var taxas = obraNumero > 0 ? ListarByIdObra(idUsina, obraNumero) : ListarTaxaPadraoByIdUsina(idUsina);

            return ObterValorAdicionalDomingosEFeriados(valorConcretoTotal, taxas);
        }

        public float ObterValorAdicionalDomingosEFeriados(float valorConcretoTotal, ICollection<ObraTaxa> obraTaxas)
        {
            var taxaAdicionalDomingosEFeriados = obraTaxas.FirstOrDefault(t => t.Tipo == "ADICIONAL DOMINGOS E FERIANDOS");

            if (taxaAdicionalDomingosEFeriados != null && taxaAdicionalDomingosEFeriados.Selecionada == "S")
            {
                // O VALOR DESTA TAXA É UM PERCENTUAL
                return valorConcretoTotal * taxaAdicionalDomingosEFeriados.Valor / 100;
            }

            return 0f;
        }

        public float ObterValorAdicionalNoturno(string horario, float volume, float valorConcretoUnitario, string[] tiposPessoa, int idUsina)
        {
            var taxas = ListarTaxaPadraoByIdUsina(idUsina);

            return ObterValorAdicionalNoturno(horario, volume, valorConcretoUnitario, tiposPessoa, idUsina);
        }

        public float ObterValorAdicionalNoturno(string horario, float volume, float valorConcretoUnitario, string[] tiposPessoa, int idUsina, int obraNumero)
        {
            var taxas = obraNumero > 0 ? ListarByIdObra(idUsina, obraNumero) : ListarTaxaPadraoByIdUsina(idUsina);

            return ObterValorAdicionalNoturno(horario, volume, valorConcretoUnitario, tiposPessoa, idUsina);
        }


        public float ObterValorAdicionalNoturno(string horario, DateTime dataConcretagem, float volume, float valorConcretoUnitario, string[] tiposPessoa, ICollection<ObraTaxa> obraTaxas)
        {
            var taxaAdicionalNoturnoCollection = obraTaxas.Where(t => t.Tipo == "ADICIONAL NOTURNO" && tiposPessoa.Contains(t.TipoPessoa));
            ObraTaxa taxaAdicionalNoturno = null;

            var diasDaSemana = new Dictionary<string, int>();
            diasDaSemana.Add("Domingo", 1);
            diasDaSemana.Add("Segunda feira", 2);
            diasDaSemana.Add("Terca feira", 3);
            diasDaSemana.Add("Quarta feira", 4);
            diasDaSemana.Add("Quinta feira", 5);
            diasDaSemana.Add("Sexta feira", 6);
            diasDaSemana.Add("Sábado", 7);

            var programacaoDiaDaSemana = ((int)dataConcretagem.DayOfWeek) + 1;

            foreach (var taxa in taxaAdicionalNoturnoCollection)
            {

                if (!(diasDaSemana.ContainsKey(taxa.QuandoDe) && diasDaSemana.ContainsKey(taxa.QuandoAte)))
                    continue;

                var taxaQuandoDe = diasDaSemana[taxa.QuandoDe];
                var taxaQuandoAte = diasDaSemana[taxa.QuandoAte];

                var programaoDentroDaFaixaDeDiasDaTaxa = 
                    taxa.QuandoOperacao.Equals("E/OU") ? 
                    programacaoDiaDaSemana == taxaQuandoDe || programacaoDiaDaSemana == taxaQuandoAte : // Caso for E/OU
                    programacaoDiaDaSemana >= taxaQuandoDe && programacaoDiaDaSemana <= taxaQuandoAte; // Caso for dentro do intervalo

                if(programaoDentroDaFaixaDeDiasDaTaxa)
                    taxaAdicionalNoturno = taxa;
                

            }

            if (taxaAdicionalNoturno != null && taxaAdicionalNoturno.Selecionada == "S")
            {
                int.TryParse(taxaAdicionalNoturno.HorarioAntesDas, out var horarioAntesDas);
                int.TryParse(taxaAdicionalNoturno.HorarioAposAs, out var horarioAposAs);

                // necessário pois na tabela guarda apenas hora cheia. Exemplo: "18" (18:00h)
                horarioAntesDas *= 100;
                horarioAposAs *= 100;

                if (horarioAntesDas == 0) horarioAntesDas = 500;
                if (horarioAposAs == 0) horarioAposAs = 1800;

                int.TryParse(horario, out var hora);

                if (hora < horarioAntesDas || hora > horarioAposAs)
                {
                    // O VALOR DESTA TAXA É UM PERCENTUAL
                    return volume * valorConcretoUnitario * taxaAdicionalNoturno.Valor / 100;
                }
            }

            return 0f;
        }

        public void AdicionarVersaoContrato(int codUsina, int numVersao, int numObra)
        {
            _obraTaxaRepository.AdicionarVersaoContrato(codUsina, numVersao, numObra);
        }

        public void ExcluirVersaoContrato(int codUsina, int numVersao, int numObra)
        {
            _obraTaxaRepository.ExcluirVersaoContrato(codUsina, numVersao, numObra);
        }

        public void AdicionarContrato(int codUsina, int numVersao, int numObra)
        {
            _obraTaxaRepository.AdicionarContrato(codUsina, numVersao, numObra);
        }

        public void ExcluirContrato(int codUsina, int numObra)
        {
            _obraTaxaRepository.ExcluirContrato(codUsina, numObra);
        }
        public void MarcarTaxa(int numVersao, int usinaCodigo, int numeroObra, int seq)
        {
            var _obraTaxa = _obraTaxaRepository.ObterPorId<ObraTaxaVersao>(numVersao, usinaCodigo, numeroObra, seq);
            _obraTaxa.Selecionada = "S";
            _obraTaxaRepository.SaveChanges();

        }

        public void MarcarTaxa(int usinaCodigo, int numeroObra, int seq)
        {
            var _obraTaxa = _obraTaxaRepository.ObterPorId<ObraTaxa>(usinaCodigo, numeroObra, seq);
            _obraTaxa.Selecionada = "S";
            _obraTaxaRepository.SaveChanges();

        }
    }
}
