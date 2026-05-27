using Microsoft.Practices.Unity;
using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.CrossCuting;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Presentation
{
    class Program
    {
        static void Main(string[] args)
        {

            //ObraRepository obraRepository = new ObraRepository();

            //Obra obraNova = obraRepository.BuscaComContrato(999, 673, 14, 675);

            //List<Obra> obras = obraRepository.ListarComTracoPendenteDeAprovacao("ADMIN");

            //List<Obra> obra2 = obraRepository.ListarComBombaPendenteDeAprovacao ("ADMIN");


            var container = new UnityContainer();
            DependencyResolver.Resolve(container);


           

            var propostaRepository = container.Resolve<IPropostaRepository>();
         
            var proposta = propostaRepository.ObterPorUsinaAnoNumero(999, 14, 2649,true);

            proposta.Obra.ContatoPrincipalNome = "Ronaldo!";

            proposta.Contato = "charles";

            propostaRepository.SaveChanges();

            //var obraRepository = container.Resolve<IObraRepository>();
            //IEnumerable<Obra> obra3 = obraRepository.ListarComDemaisAprovacoesPendentes("ADMIN");

            //var taxaService = container.Resolve<IObraTaxaRepository>();


            //ObraTaxa taxa = taxaService.ObterPorId(11, 117777, 1);

            // AprovacaoComercial testex = obraRepository.BuscarAprovacaoComercialByIdObra(999, 83244);


            //obraNova.ObraBombas.ElementAt(0).DescontoPercentual = 666;


            var contratoService = container.Resolve<ICadastroGeralRepository>();

            ICollection<CadastroGeral> motivosBloqueio = contratoService.ListarMotivosBloqueioInterveniente();


            /*
            AppDataContext db = new  AppDataContext();

            var taxa = db.ObraTaxas.Find(11, 117777, 1);

            db.ObraTaxas.Attach(taxa);

            taxa.AprovacaoCiente = "S";
            
            
  
            db.SaveChanges();
            */

            //db.Entry<Obra>(obraNova). State = System.Data.Entity.EntityState.Modified;


            // var teste = db.AprovacoesComerciais.Find("00003B85107F8F241B1910E422E36689");
            /// db.Entry
            ///db.SaveChanges();

        }
    }
}
