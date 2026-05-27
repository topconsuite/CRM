import { CadastroDiverso } from "../cadastro-geral/cadastro-diverso";
import { Usina } from "../usina/usina";
import {LiberacaoAcesso } from "./liberacao-acesso";

export class GrupoAcesso {
    codigo: number = 0;
    usina: number = 0;
    descricao: string = '';
    criadoEm: Date = new Date();
    atualizadoEm: Date = new Date();
    liberacoesAcessos: LiberacaoAcesso[] = [];
    usuarios: CadastroDiverso[] = [];
    usinaInf: Usina;
}