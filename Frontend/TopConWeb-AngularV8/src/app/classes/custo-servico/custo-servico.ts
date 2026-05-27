import { Usina } from "../usina/usina";

export class CustoServico {
    usina: Usina;
    usinaCodigo: number = 0;
    usinaTabelapreco: number = 0;
    dataInicioVigencia: Date;
    custoMedioHoraTransporte: number = 0;
    custoMedioProducao: number = 0;
    custoMedioComercial: number = 0;
    custoMedioLaboratorio: number = 0;
    custoMedioAdministrativo: number = 0;
    custoMedioBombagem: number = 0;
    custoMedioServico: number = 0;
    custoMedioCombustivel: number = 0;
    custoMedioImpostos: number = 0;
    custoMedioLubrificantes: number = 0;
    custoMedioManutencao: number = 0;
    custoMedioMaoDeObra: number = 0;
    outrosCustosMateriais: number = 0;
    formaMedidaAditivo: number = 1;
    lucro: number = 0;
    markup: number = 0;
    impostoEstadual: number = 0;
    impostoFederal: number = 0;
}