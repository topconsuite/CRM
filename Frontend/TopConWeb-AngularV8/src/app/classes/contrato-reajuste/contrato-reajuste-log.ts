export class ContratoReajusteLog {
    usina: number = 0;
    anoContrato: number = 0;
    numeroContrato: number = 0;
    dataVigencia: Date = new Date();
    sequencia: number = 0;
    tipo: string = '';
    dataHoraEvento: Date = new Date();
    usuario: string = '';
    evento: string = '';
    complemento: string = '';
}