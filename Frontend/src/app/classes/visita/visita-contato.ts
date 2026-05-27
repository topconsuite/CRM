import { CadastroGeral } from "../cadastro-geral/cadastro-geral";

export class VisitaContato {
    usina: number = 0;
    numeroVisita: number = 0;
    anoVisita: number = 0;
    sequencia: number = 0;
    nome: string = '';
    
    funcao: CadastroGeral;
    funcaoCodigo: number = 0;

    dddTelefone: number = 0;
    telefone: number = 0;
    dddCelular: number = 0;
    celular: number = 0;
    email: string = '';
}