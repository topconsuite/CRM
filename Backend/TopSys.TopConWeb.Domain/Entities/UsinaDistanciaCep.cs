using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class UsinaDistanciaCep
    {
        public int UsinaEntrega { get; set; }

        public string Cep { get; set; }

        public int DistanciaKm { get; set; }

        public string IdCadastro { get; set; }

        public string IdAtualizacao { get; set; }

        public string IdAprovacao { get; set; }

        public void AprovarNovaDistanciaUsinaCEP(string usuario, int novaDistancia)
        {
            DistanciaKm = novaDistancia;
            IdAprovacao = StringHelper.GetIDD(usuario);
        }
    }
}
