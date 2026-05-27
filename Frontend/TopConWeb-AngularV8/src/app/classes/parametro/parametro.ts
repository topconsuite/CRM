import * as internal from "assert";

export abstract class Parametro {
    dataInicioVigencia: Date = new Date();
}

export class ParametroProposta extends Parametro {
    obrigaProfissao: boolean = false;
    obrigaVolumeEstimadoEPrevisaoInicioTermino: boolean = false;
    obrigaAprovacaoDistanciaUsinaEntrega: boolean = false;
    obrigaEmailPessoaFisica: boolean = false;
    bloqueiaImpressaoPropostaContratoPendenteAprovacao: boolean = false;
    percentualDescontoLimite: number = 0.0;
    obrigaTipoObra: boolean = false;
    obrigaPorteObra: boolean = false;
    mensagemReajustePadrao: string = "";
    validadeProposta: number = 0;
    informarBombaTerceiros: boolean = false;
    obrigaFimVigencia: boolean = false;
    obrigaNumeroContratoAnterior: boolean = false;
}