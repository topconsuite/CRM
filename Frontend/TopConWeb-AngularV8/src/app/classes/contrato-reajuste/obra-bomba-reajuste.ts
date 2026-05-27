import { CadastroGeral } from "../cadastro-geral/cadastro-geral";

export class ObraBombaReajuste {
    usinaCodigo: number;
    contratoAno: number;
    contratoNumero: number;
    bombaTipo: CadastroGeral;
    valorVigente: number;
    vigenteAteM3: number;
    m3ExcedenteVigente: number;
    valorReajustado: number;
    reajustadoAteM3: number;
    m3ExcedenteReajustado: number;
}