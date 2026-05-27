import { EMetodoAutenticacao, EMetodoEnvioAssinatura } from "./solicitacao-assinatura-clicksing";

export class DadosPessoais{
    email: string = '';
    nomeCompleto: string = '';
    cpf: string = '';
    dataNascimento: Date;
    metodoAutenticacao: EMetodoAutenticacao;
    metodoEnvioAssinatura: EMetodoEnvioAssinatura;
    telefoneDdd: number = 0;
    telefoneNumero: number = 0;
}