using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class BancoService : ServiceBase<Conta>, IBancoService
    {
        private readonly IBancoRepository _bancoRepository;

        public BancoService(IBancoRepository bancoRepository) : base(bancoRepository)
        {
            _bancoRepository = bancoRepository;
        }

        public ICollection<Conta> Listar()
        {
            return _bancoRepository.ListarBanco();
        }

        public Conta ObterPorId(int cod, int emp, bool tracking = false)
        {
            return _bancoRepository.ObterPorIdBanco(cod, emp, tracking);
        }

        public bool EstaEmUsoBanco(int cod, int emp)
        {
            return _bancoRepository.EstaEmUsoBanco(cod, emp);
        }
    }
}
