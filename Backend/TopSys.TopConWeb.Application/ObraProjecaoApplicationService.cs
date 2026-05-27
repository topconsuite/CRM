using System.Collections.Generic;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.Application.DTOS.Request.Obra.ObraProjecao;
using System.Transactions;
using System;
using TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraProjecao;

namespace TopSys.TopConWeb.Application
{
    public class ObraProjecaoApplicationService : ApplicationServiceBase<ObraProjecao>, IObraProjecaoApplicationService
    {

        private readonly IObraProjecaoService _obraProjecaoService;
        public readonly IObraService _obraService;
        private readonly IObraApplicationService _obraAppService;

        public ObraProjecaoApplicationService(
            IObraProjecaoService obraProjecaoService,
            IObraService obraService,
            IObraApplicationService obraAppService,
            IUnitOfWork unityOfWork) : base(obraProjecaoService, unityOfWork)
        {
            _obraProjecaoService = obraProjecaoService;
            _obraService = obraService;
            _obraAppService = obraAppService;
        }

        public IEnumerable<ObraProjecaoResponse> ListarPorObra(int obraUsina, int obraNumero)
        {
            return AutoMapper.Mapper.Map(_obraProjecaoService.ListarPorObra(obraUsina, obraNumero), new List<ObraProjecaoResponse>());
        }

        public void Adicionar(string usuario, ObraProjecaoRequest obraProjecaoRequest, string userRequest)
        {
            ValidarVolumeDisponivel(obraProjecaoRequest, true);

            var obraProjecao = AutoMapper.Mapper.Map(obraProjecaoRequest, new ObraProjecao());
            var saldoAnterior = _obraProjecaoService.GetSaldoProjecaoAnterior(obraProjecao.Usina, obraProjecao.NoObra, obraProjecao.AnoChamada, obraProjecao.NoChamada, obraProjecao.Periodo);
            obraProjecao.Periodo = new DateTime(obraProjecao.Periodo.Year, obraProjecao.Periodo.Month, 1);
            if (saldoAnterior == 0)
            {
                saldoAnterior = obraProjecaoRequest.Saldo;
            }
            obraProjecao.Saldo = saldoAnterior - obraProjecao.Volume;

            using (var scope = new TransactionScope())
            {
                _obraProjecaoService.Adicionar(obraProjecao);

                _obraService.Adicionar(new ObraLog
                {
                    UsinaCodigo = obraProjecaoRequest.Usina,
                    ObraCodigo = obraProjecaoRequest.NoObra,
                    AnoChamada = obraProjecaoRequest.AnoChamada,
                    NumChamada = obraProjecaoRequest.NoChamada,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "INCLUSÃO PROJEÇÃO CARTEIRA",
                    Complemento = $"Inclusão do Período: {obraProjecaoRequest.Periodo.ToString("MM/yyyy")} Volume: {obraProjecaoRequest.Volume} M3",
                    Observacao = "",
                    Sequencia = 1
                });

                Commit();
                scope.Complete();
            }
        }

        public void Atualizar(string usuario, ObraProjecaoRequest obraProjecaoRequest, string userRequest)
        {
            ValidarVolumeDisponivel(obraProjecaoRequest, false);

            var obraProjecao = AutoMapper.Mapper.Map(obraProjecaoRequest, new ObraProjecao());

            using (var scope = new TransactionScope())
            {
                _obraProjecaoService.AtualizarProjecao(obraProjecao, obraProjecaoRequest.VolumeAnterior, obraProjecaoRequest.PeriodoAnterior);
                
                _obraService.Adicionar(new ObraLog
                {
                    UsinaCodigo = obraProjecaoRequest.Usina,
                    ObraCodigo = obraProjecaoRequest.NoObra,
                    AnoChamada = obraProjecaoRequest.AnoChamada,
                    NumChamada = obraProjecaoRequest.NoChamada,
                    DataHora = DateTime.Now,
                    Usuario = usuario,
                    Evento = "ALTERAÇÃO PROJEÇÃO CARTEIRA",
                    Complemento = $"Alterado De Período: {obraProjecaoRequest.PeriodoAnterior.ToString("MM/yyyy")} Volume: {obraProjecaoRequest.VolumeAnterior} M3 Para Período: {obraProjecaoRequest.Periodo.ToString("MM/yyyy")} Volume: {obraProjecaoRequest.Volume} M3",
                    Observacao = "",
                    Sequencia = 1
                });

                Commit();
                scope.Complete();
            }
        }

        private void ValidarVolumeDisponivel(ObraProjecaoRequest request, bool novoCadastro)
        {
            var obra = _obraService.ObterPorId(request.Usina, request.NoObra);

            var volumeContratado = _obraAppService.ObterVolumePorContrato(request.Usina, request.NoObra, request.AnoChamada, request.NoChamada) ?? 0;
            var volumeConsumido = _obraAppService.ObterConsumoPorContrato(obra.UsinaCodigo, obra.NumContrato ?? 0, obra.AnoContrato ?? 0) ?? 0;
            var volumePrevisao = ObterPrevisaoSaldoProjecaoPorContrato(request.Usina, request.NoObra, request.AnoChamada, request.NoChamada) ?? 0;

            float saldoDisponivel;

            if (novoCadastro)
            {
                saldoDisponivel = volumeContratado - (volumeConsumido + request.Volume + volumePrevisao);
            }
            else
            {
                saldoDisponivel = volumeContratado - (volumeConsumido + (volumePrevisao - request.VolumeAnterior) + request.Volume);
            }

            if (saldoDisponivel < 0)
            {
                throw new Exception("Volume projetado maior que o saldo disponível!");
            }
        }

        public float? ObterSaldoProjecaoPorContrato(int usina, int noObra, int anoChamada, int noChamada)
        {
            var saldo = _obraProjecaoService.ObterSaldoProjecaoPorContrato(usina, noObra, anoChamada, noChamada);

            if (saldo == null)
                saldo = 0;

            return saldo;
        }

        public float? ObterPrevisaoSaldoProjecaoPorContrato(int usina, int noObra, int anoChamada, int noChamada)
        {
            var saldo = _obraProjecaoService.ObterPrevisaoSaldoProjecaoPorContrato(usina, noObra, anoChamada, noChamada);

            if (saldo == null)
                saldo = 0;

            return saldo;
        }

        public DateTime? GetProximoPeriodoPorContrato(int usina, int noObra, int? anoChamada, int? noChamada)
        {
            var proximoPeriodo = _obraProjecaoService.GetProximoPeriodoPorContrato(usina, noObra, anoChamada, noChamada);

            return proximoPeriodo;
        }
    }
}
