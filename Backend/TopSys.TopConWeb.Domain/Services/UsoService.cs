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
    public class UsoService : ServiceBase<Uso>, IUsoService
    {
        private readonly IUsoRepository _usoRepository;
        private const string _descricaoTracoPadrao = "#TIPORESISTENCIA #MPA/CONSUMO #BRITA #SLUMP #USO";

        public UsoService(IUsoRepository usoRepository) : base(usoRepository)
        {
            _usoRepository = usoRepository;
        }

        public bool PossuiDescricaoPersonalizada(int idUso)
        {
            var descricaoPersonalizada = _usoRepository.ObterDescricaoPersonalizada(idUso);

            return descricaoPersonalizada != null ? descricaoPersonalizada != _descricaoTracoPadrao : false;
        }
    }
}
