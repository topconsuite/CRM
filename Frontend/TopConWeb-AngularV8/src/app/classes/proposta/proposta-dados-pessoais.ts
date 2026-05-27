import { intervenienteTipos } from './../interveniente/interveniente';
import { Endereco } from './../endereco/endereco';

export class PropostaDadosPessoais {
    usinaCodigo: number = 0;
    propostaAno: number = 0;
    propostaNumero: number = 0;
    intervenienteTipo: string = intervenienteTipos[0].codigo;
    cpfCnpj: string = '';
    rg: string = '';
    orgaoExpedidor: string = '';
    inscricaoEstadual: string ='';
    inscricaoMunicipal: string = '';
    nome: string = '';
    razao: string = '';
    endereco: Endereco = new Endereco();
    email: string = '';
    telefoneDdd: number = 0;
    telefoneNumero: number = 0;   
}