using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Domain.Services
{
    public class UsinaDistanciaCepService : ServiceBase<UsinaDistanciaCep>, IUsinaDistanciaCepService
    {
        private readonly IUsinaDistanciaCepRepository _usinaDistanciaCepRepository;

        public UsinaDistanciaCepService(IUsinaDistanciaCepRepository usinaDistanciaCepRepository) : base(usinaDistanciaCepRepository)
        {
            _usinaDistanciaCepRepository = usinaDistanciaCepRepository;
        }

        public void AdicionarCasoNaoCadastrado(string usuario, int idUsina, string cep, int distanciaKm)
        {
            if (idUsina != 0 && cep != "" && distanciaKm > 0)
            {
                var distanciaUsina = ObterPorId(idUsina, cep);

                if ((distanciaUsina?.DistanciaKm ?? 0) == 0)
                {
                    if (distanciaUsina == null)
                    {
                        Adicionar(new UsinaDistanciaCep()
                        {
                            UsinaEntrega = idUsina,
                            Cep = cep,
                            DistanciaKm = distanciaKm,
                            IdCadastro = StringHelper.GetIDD(usuario),
                            IdAtualizacao = "",
                            IdAprovacao = ""
                        });
                    }
                    else
                    {
                        distanciaUsina.UsinaEntrega = idUsina;
                        distanciaUsina.Cep = cep;
                        distanciaUsina.DistanciaKm = distanciaKm;
                        distanciaUsina.IdCadastro = StringHelper.GetIDD(usuario);
                        distanciaUsina.IdAtualizacao = "";
                        distanciaUsina.IdAprovacao = "";

                        Atualizar(distanciaUsina);
                    }
                }
            }
        }
    }
}