using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Entities
{
    public interface IObraDemaisServicos<TObra, TContrato, TObraTraco, TObraBomba>
        where TContrato : IHasInterveniente, IHasVendedor, IContrato
        where TObraBomba : ObraBombaBase<TObra>
        where TObra : IObra<TObra, TContrato, TObraTraco, TObraBomba>
        where TObraTraco : ObraTracoBase<TObra>
    {
        float CalcularValorTotal(IObra<TObra, TContrato, TObraTraco, TObraBomba> obra);
    }
}
