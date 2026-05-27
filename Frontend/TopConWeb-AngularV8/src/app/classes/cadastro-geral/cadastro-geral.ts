export class CadastroGeral {
    codigo: number = 0;
    descricao: string = '';
    viaCaptacao: CadastroGeralViaCaptacao = new CadastroGeralViaCaptacao();
}

export class CadastroGeralViaCaptacao {
    codigo: number = 0;
    ativo: string = 'S';
    tipoIndicacao: ECadastroGeralViaCaptacaoTipoIndicacao = ECadastroGeralViaCaptacaoTipoIndicacao.Nenhum;
}

export enum ECadastroGeralViaCaptacaoTipoIndicacao {
    Nenhum = 0,
    Cliente = 1,
    Vendedor = 2
}