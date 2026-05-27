import { CadastroGeral } from "../cadastro-geral/cadastro-geral";
import { Usina } from "../usina/usina";

export class OportunidadeContato {
    usina: Usina;
    numeroOportunidade: number = 0;
    anoOportunidade: number = 0;
    sequencia: number = 0;
    nome: string = '';

    funcao: CadastroGeral;
    funcaoCodigo: number = 0;

    dddTelefone: number = 0;
    telefone: number = 0;
    dddCelular: number = 0;
    celular: number = 0;

    email: string ='';
}