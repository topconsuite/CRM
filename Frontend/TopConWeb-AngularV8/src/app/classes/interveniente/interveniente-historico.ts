export class IntervenienteHistorico {
    codigoInterveniente: number = 0;
    sequenciaHistorico : number = 0;
    data: Date = new Date();
    hora: string ="";
    descricao : string = "";
    dataPrevistaDeRetorno: Date = null;
    horaPrevistaDeRetorno: string = "";
    horaPrevistaDeRetornoString: string = "";
    dataDeRetorno: Date = null;
    horaDeRetorno: string = "";
    vinculo: string = ""
    empresaCodigo : number = 0;
    documentoTipo : number = 0;
    documentoSerie : string = "";
    documentoNumero : number = 0;
    documentoSequencia : string = "";
    idCadastro : string = "";
    idAtual : string = "";

}