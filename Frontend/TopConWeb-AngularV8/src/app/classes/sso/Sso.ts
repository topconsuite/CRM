export class Sso{
    id : string = '';
    habilitado : boolean = false;
    dominio : string = '';
    tenantId : string = '';
    clientId : string = '';
    urlRedirecionamento : string = '';
    tipoProvedor : ETipoProvedor = ETipoProvedor.Microsoft;    
}

export enum ETipoProvedor
{
    Microsoft = 0
}