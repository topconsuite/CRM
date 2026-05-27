using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Integrations
{
    public interface IViaCepService
    {
        Endereco ObterEnderecoPorCep(string cep);
    }
}
