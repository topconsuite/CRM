import { DadosPessoais } from "./solicitacao-dados-pessoais";

export class SolicitacaoAssinaturaClicksign {
    contratoUsina: number = 0;
    contratoNumero: number = 0;
    contratoAno: number = 0;
    dadosPessoaisAssinatura: DadosPessoais [] = [];
    emailResponsavelSolidario = '';
    nomeCompletoResponsavelSolidario: string = '';
    cpfResponsavelSolidario: string = '';
    dataNascimentoResponsavelSolidario: Date;
    metodoAutenticacaoResponsavelSolidario: EMetodoAutenticacao;
    metodoEnvioAssinaturaResponsavelSolidario: EMetodoEnvioAssinatura;
    telefoneDddResponsavelSolidario: number = 0;
    telefoneNumeroResponsavelSolidario: number = 0;       
    emailPrimeiraTestemunha = '';	
    nomeCompletoPrimeiraTestemunha: string = '';
    cpfPrimeiraTestemunha: string = '';
    dataNascimentoPrimeiraTestemunha: Date;
    metodoAutenticacaoPrimeiraTestemunha: EMetodoAutenticacao;
    metodoEnvioAssinaturaPrimeiraTestemunha: EMetodoEnvioAssinatura;
    telefoneDddPrimeiraTestemunha: number = 0;
    telefoneNumeroPrimeiraTestemunha: number = 0;   
    emailSegundaTestemunha = '';	
    nomeCompletoSegundaTestemunha: string = '';
    cpfSegundaTestemunha: string = '';
    dataNascimentoSegundaTestemunha: Date;
    metodoAutenticacaoSegundaTestemunha: EMetodoAutenticacao;
    metodoEnvioAssinaturaSegundaTestemunha: EMetodoEnvioAssinatura;
    telefoneDddSegundaTestemunha: number = 0;
    telefoneNumeroSegundaTestemunha: number = 0; 
}

export enum EMetodoAutenticacao {
	Email = 1,
	Whatsapp,
	Sms
}

export enum EMetodoEnvioAssinatura {
    Email = 1,
    Whatsapp
}

export enum EOpcoesTestemunhas {
	NaoEnvia = 0,
	Testemunha,
	Vendedor
}