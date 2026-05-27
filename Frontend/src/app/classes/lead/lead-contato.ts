import { CadastroGeral } from "../cadastro-geral/cadastro-geral";
import { Usina } from "../usina/usina";

export class LeadContato {
    usina: Usina;
    numeroLead: number = 0;
    anoLead: number = 0;
    sequencia: number = 0;
    nome: string = '';
    funcao: CadastroGeral;
    ddd: number = 0;
    telefone: number = 0;
    dddCelular: number = 0;
    celular: number = 0;
    email: string = '';
}