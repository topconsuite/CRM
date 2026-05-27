import { CadastroGeral } from "../cadastro-geral/cadastro-geral";
import { Interveniente } from "./../interveniente/interveniente";

export class GrupoEconomico {
    codigo: number = 0;
    descricao: string = '';
    limiteValor: number = 0.00;
    limiteData: Date;
    bloqueioMotivo: CadastroGeral;
    bloqueioObservacao: string = '';
    clientes: Interveniente[] = [];
    idCadastro: string = '';
    idAtualizacao: string = ''; 
}