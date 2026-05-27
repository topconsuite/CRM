import { Time } from "@angular/common";
import { Endereco, Municipio } from "../endereco/endereco";
import { Usina } from "../usina/usina";
import { Vendedor } from "../vendedor/vendedor";
import { VisitaContato } from "./visita-contato";
import { VisitaTipo } from "./visita-tipo";

export class Visita {

    usina: Usina;
    usinaCodigo: number;

    numero: number = 0;
    ano: number = 0;
    
    tipoVisita: VisitaTipo;
    visitaTipoCodigo: number = 0;
    
    data: Date = new Date();

    horaVisita: string = new Date().getHours().toString().padStart(2, '0') + ':' + new Date().getMinutes().toString().padStart(2, '0') + ':00';
    horaVisitaString: string = new Date().getHours().toString().padStart(2, '0') + new Date().getMinutes().toString().padStart(2, '0');

    cliente: string = '';
    dddTelefone: number = 0;
    telefone: number = 0;
    dddCelular: number = 0;
    celular: number = 0;
    email: string = '';

    vendedor: Vendedor;
    vendedorCodigo: number = 0;

    obraNome: string = ''
    observacaoInterna: string = '';
    
    endereco: Endereco = new Endereco();

    leadNumero: number = 0;
    leadAno: number = 0;

    contatoPrincipal: VisitaContato = new VisitaContato();
    contatoSecundario: VisitaContato = new VisitaContato();
}