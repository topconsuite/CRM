import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

declare var $port: any;
declare var $host: any;

@Injectable()
export class ApiBaseUrlService {

  constructor() { }

  getNomeCliente(){
    return 'DEV.TELLURIA';
  };
  getHost(){
    return `${window.location.protocol}//${window.location.hostname}`;
    //return 'http://201.91.136.58'; //CORTESIA
    //return 'http://52.229.63.213'; // TOPSYS AZURE
    //return 'http://topsys.dyndns.biz'; // TOPSYS DYNDNS
    //return 'http://terraminas.ddns.net'; // TERRA MINAS
  }
  
  getApiPort(){
    return $port;
  }
  getUrlAlmoxarifado(){
    if (environment.production) {
      return `${this.getHost()}:${this.getApiPort()}/topmobile/api/`;
    } else {
      return `${this.getHost()}:51632/api/`;
    }
  };
  getUrl(){
    if(!!$host) return `${$host}/topconweb/api/`;
    if (environment.production) {
      return `${this.getHost()}:${this.getApiPort()}/topconweb/api/`;
    } else {
      return `${this.getHost()}:65412/api/`;
    }
  };

}
