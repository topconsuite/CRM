using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Application.DTOS.Response.Distancia
{
    public class EstimativaKmEnderecoResponse
    {
          public bool UtilizaGoogleApi { get; set; }
          public int? DistanciaEmKm { get; set; } 
    }
}
