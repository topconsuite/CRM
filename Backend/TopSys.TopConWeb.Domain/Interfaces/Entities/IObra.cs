using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Interfaces.Entities
{
    public interface IObra <TObra, TContrato, TObraTraco, TObraBomba>
        where TContrato : IHasInterveniente, IHasVendedor, IContrato
        where TObraBomba : ObraBombaBase<TObra>
        where TObraTraco : ObraTracoBase<TObra>
        where TObra : IObra<TObra, TContrato, TObraTraco, TObraBomba>
    {
        EObraStatusComercial StatusComercial { get; set; }

        float VolumeEstimado { get; set; }

        float VolumePorCarga { get; set; }

        TContrato Contrato { get; set; }

        ICollection<TObraTraco> ObraTracos { get; set; }

        ICollection<TObraBomba> ObraBombas { get; set; }
    }
}
