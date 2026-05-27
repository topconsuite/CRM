using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface ICadastroDiversoRepository : IRepositoryBase<CadastroDiverso>
    {
        ICollection<CadastroDiverso> ListarAndares();

        ICollection<CadastroDiverso> ListarCondicoesPagamento();

        ICollection<CadastroDiverso> ListarDiasDaSemanaFixo();

        ICollection<CadastroDiverso> ListarOpcoesDeVencimentoEmDiaNaoUtil();

        ICollection<CadastroDiverso> ListarQuantidadeDeCorposDeProva();

        ICollection<CadastroDiverso> ListarModeloDocumentoRemessaConcreto();
    }
}
