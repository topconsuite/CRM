import { EMetodoAutenticacao, EMetodoEnvioAssinatura, EOpcoesTestemunhas } from "./solicitacao-assinatura-clicksing";

export class ClicksignParametro {
    id: string;
    corpoEmail: string;
    obrigaDocumentoOficial: boolean = false;
    obrigaSelfie: boolean = false;
    obrigaAssinaturaManuscrita: boolean = false;
    obrigaReconhecimentoFacial: boolean = false;
    notificaClienteNaConfirmacaoDeAssinatura: boolean = true;
    prazoLimiteAssinatura: number;
    enviaAssinaturaContratada: boolean = false;
    enviaAssinaturaResponsavelSolidario: boolean = false;
    emailContratada: string = "";
    dddContratada: number = 0;
    telefoneContratada: number = 0;
    metodoEnvioAssinaturaContratada: EMetodoEnvioAssinatura;
    metodoAutenticacaoContratada: EMetodoAutenticacao;
    permiteAssinaturaWhatsApp: boolean = false;
    primeiraTestemunha: EOpcoesTestemunhas;
    segundaTestemunha: EOpcoesTestemunhas;
}