using Dapper;
using System.Linq;
using System.Text;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class EquipamentoRepository : RepositoryBase<Equipamento>, IEquipamentoRepository
    {

        public EquipamentoRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public Equipamento ObterPorPlaca(string placa, bool tracking = false)
        {
            return _context
                .Equipamentos
                .Where(x => x.Placa == placa)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public Equipamento ObterPorId(string codigo, bool tracking = false)
        {
            return _context
                .Equipamentos
                .Where(x => x.Codigo == codigo)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public Equipamento ObterPorExternalId(string externalId, bool tracking = false)
        {
            return _context
                .Equipamentos
                .Where(x => x.ExternalID == externalId)
                .Tracking(tracking)
                .FirstOrDefault();
        }

        public bool JaFoiUtilizado(string codEquipamento)
        {

            var query = new StringBuilder();
            var contagem = 0;

            query.AppendLine($"SELECT SUM(b.Contagem) Contagem FROM");
            query.AppendLine($"(SELECT COUNT(*) Contagem FROM con_programacao WHERE cod_equip_bomba = '{codEquipamento}'");
            query.AppendLine($"UNION SELECT COUNT(*) Contagem FROM con_reserva_bomba WHERE cod_bomba = '{codEquipamento}'");
            query.AppendLine($"UNION SELECT COUNT(*) Contagem FROM con_veiculo_localizacao_fleet WHERE cod = '{codEquipamento}'");
            query.AppendLine($"UNION SELECT COUNT(*) Contagem FROM man_manutencoes WHERE equipamento = '{codEquipamento}'");
            query.AppendLine($"UNION SELECT COUNT(*) Contagem FROM con_nf WHERE no_betoneira = '{codEquipamento}') AS B;");

            contagem = _context.Database.Connection.QueryFirstOrDefault<int>(query.ToString());

            return (contagem > 0);

        }

    }
}
