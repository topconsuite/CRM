export class Endereco {
    cep: string = '';
    logradouro: string = '';
    numero: number = 0;
    complemento: string = '';
    bairro: string = '';
    municipio: Municipio; // = new Municipio()
}

export class Municipio {
    codigo: number = 0;
    nome: string = '';
    uf: string = '';
    ibgeCodigo: number = 0;
}

export class DistanciaViaGoogleApi {
    distanciaEmKm: number = 0;
    utilizaGoogleApi: boolean = true;
}