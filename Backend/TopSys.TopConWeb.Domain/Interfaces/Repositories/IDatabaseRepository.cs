using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IDatabaseRepository
    {
        string ObterColunasEmComumEntreTabelas(string origem, string destino);
        IEnumerable<string> ObterColunasTabela(string tabela);
    }
}
