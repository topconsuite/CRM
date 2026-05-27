import { Conta } from './conta';

export class Portador {
    codigo: number = 0;
    descricao: string = '';
    conta: Conta = new Conta();
}