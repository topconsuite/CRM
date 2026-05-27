using System.Collections.Generic;

namespace TopSys.TopConWeb.Application.DTOS.Response.Obra.ObraTracoStatus
{

    public class ObraTracosStatusResponse
    {

        public bool PossuiCustoVirtual { get; set; } = false;

        public List<ObraTracoStatusResponse> Tracos { get; set; } = new List<ObraTracoStatusResponse>();


    }

    public class ObraTracoStatusResponse
    {

        public int Sequencia { get; set; }
        public int Status { get; set; }

    }
}
