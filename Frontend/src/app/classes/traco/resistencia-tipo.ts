export enum ETipoVinculoMpaConsumo {
  SEM_VINCULO = 0,
  MPA = 1,
  CONSUMO = 2,
}

export class ResistenciaTipo {
    codigo: number = 0;
    descricao: string = '';
    abreviatura: string = '';
    vinculo: ETipoVinculoMpaConsumo = ETipoVinculoMpaConsumo.MPA;
}