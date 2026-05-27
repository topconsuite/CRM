export enum EObraTracoStatus {

    Erro = 0,
    Homologado = 7101,
    Arquivo = 7102,
    HomologadoVirtual = 7103,
    EmExperimento = 7104,
    CustoVirtual = 7105

}

export class ObraTracoStatus {
    sequencia: number = 0;
    status: EObraTracoStatus = EObraTracoStatus.Erro;
}

export class ObraTracoStatusResponse {
    possuiCustoVirtual: boolean = false;
    tracos: ObraTracoStatus[] = [];
}