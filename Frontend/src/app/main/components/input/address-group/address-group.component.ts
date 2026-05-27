import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';

import { Endereco, Municipio } from 'app/classes/endereco/endereco'

import { EnderecoService } from 'app/services/endereco.service';
import { ICustomValidator } from '../../interfaces/custom-validator';

@Component({
  selector: 'address-group',
  templateUrl: './address-group.component.html',
  styleUrls: ['./address-group.component.scss']
})
export class AddressGroupComponent implements OnInit {

  @Input() formGroup: FormGroup;
  @Input() alias: string = "group";
  @Input() model: Endereco;
  private _required: boolean = false;
  @Input() set required(value: boolean) { this._required = (value!==false && value!==undefined && value!==null); };
           get required(): boolean { return this._required };

  private _disabled: boolean = false;
  @Input() set disabled(value: boolean) { this._disabled = (value!==false && value!==undefined && value!==null); };
           get disabled(): boolean { return this._disabled };

  private _obrigaMunicipio: boolean = false;
  @Input() set obrigaMunicipio(value: boolean) { this._obrigaMunicipio = (value!==false && value!==undefined && value!==null); };
           get obrigaMunicipio(): boolean { return this._obrigaMunicipio };

  private _obrigaBairro: boolean = false;
  @Input() set obrigaBairro(value: boolean) { this._obrigaBairro = (value!==false && value!==undefined && value!==null); };
          get obrigaBairro(): boolean { return this._obrigaBairro };

  private _cepErrorMessagesDefault = {key: 'minLength', message: 'cep deve conter 8 dígitos!'};
  private _cepErrorMessages: {key: string, message: string}[] = [this._cepErrorMessagesDefault];
  @Input() set cepErrorMessages(value: {key: string, message: string}[]) {
              if (!value) value = [];
              value.push(this._cepErrorMessagesDefault);
              this._cepErrorMessages = value;
           };
           get cepErrorMessages() { return this._cepErrorMessages; };

  @Output() modelChange: EventEmitter<Endereco> = new EventEmitter<Endereco>();
  @Output() logradouroFocus: EventEmitter<any> = new EventEmitter<any>();
  @Output() logradouroFocusOut: EventEmitter<any>= new EventEmitter<any>();
  @Output() numeroFocus: EventEmitter<any> = new EventEmitter<any>();
  @Output() numeroFocusOut: EventEmitter<any> = new EventEmitter<any>();
  @Output() cepFocus: EventEmitter<any> = new EventEmitter<any>();
  @Output() cepFocusOut: EventEmitter<any> = new EventEmitter<any>();
  @Output() onCepRequestSuccess: EventEmitter<any> = new EventEmitter<any>();


  maskCEP = [/\d/, /\d/, /\d/, /\d/, /\d/,'-', /\d/, /\d/, /\d/];

  private _cepInexistente: boolean = false;

  private _uf: string = '';
  get uf(): string {
    if (!this._uf && this.model && this.model.municipio) this._uf = this.model.municipio.uf;
    return this._uf;
  };
  set uf(value: string) {
    if (this.model && this.model.municipio && this.model.municipio.uf!==value) {
      this.model.municipio = null;
    }
    this._uf = value;
  };
  estados = {
    'AC':'Acre',
    'AL':'Alagoas',
    'AP':'Amapá',
    'AM':'Amazonas',
    'BA':'Bahia',
    'CE':'Ceará',
    'DF':'Distrito Federal',
    'ES':'Espírito Santo',
    'GO':'Goiás',
    'MA':'Maranhão',
    'MT':'Mato Grosso',
    'MS':'Mato Grosso do Sul',
    'MG':'Minas Gerais',
    'PA':'Pará',
    'PB':'Paraíba',
    'PR':'Paraná',
    'PE':'Pernambuco',
    'PI':'Piauí',
    'RJ':'Rio de Janeiro',
    'RN':'Rio Grande do Norte',
    'RS':'Rio Grande do Sul',
    'RO':'Rondônia',
    'RR':'Roraima',
    'SC':'Santa Catarina',
    'SP':'São Paulo',
    'SE':'Sergipe',
    'TO':'Tocantins'
  };
  siglasEstados = ['AC','AL','AP','AM','BA','CE','DF','ES','GO','MA','MT','MS','MG',
                    'PA','PB','PR','PE','PI','RJ','RN','RS','RO','RR','SC','SP','SE','TO']

  municipios: Municipio[] = [];

  isCepMunicipio: boolean = false;

  constructor(private _enderecoService: EnderecoService) { }

  ngOnInit() {
    setTimeout( () => {
      this.carregaMunicipiosFiltros();
    }, 300);
      
  }

  setModel(newModel: Endereco) {
    this.modelChange.emit(newModel);
  };

  private _focused: boolean = false;
  onFocus() {
    this._focused = true;
    this.cepFocus.emit();
  }
  onFocusout() {
    this._focused = false;
    this.cepFocusOut.emit();
  }
  onLogradouroFocus(){
    this.logradouroFocus.emit();
  }
  onLogradouroFocusOut(){
    this.logradouroFocusOut.emit();
  }
  onNumeroFocus(){
    this.numeroFocus.emit();
  }
  onNumeroFocusOut(){
    this.numeroFocusOut.emit();
  }



  private _initialized: boolean = false;
  private _cepAnterior: string = '';
  cepChange(model: Endereco, cep: string) {
    if (!this._initialized) {
      this._cepAnterior = cep;
      this._initialized = true;
      return;
    }

    if (cep.length===8 && (!this._focused || this._cepAnterior === cep)) {
      this._enderecoService.obterPorCep(cep, true)
      .then(endereco => {
        this.isCepMunicipio = (!endereco || endereco.logradouro === '');
      });

      this.carregaMunicipiosFiltros();
    }
    
    if (cep.length===8 && this._focused && this._cepAnterior !== cep) {
      this._cepAnterior = cep;
      this._enderecoService.obterPorCep(cep)
      .then(endereco => {
        if (endereco) {
          this._cepInexistente = false;
          this.isCepMunicipio = (endereco.logradouro === '');
          model = endereco;
          this.uf = endereco.municipio ? endereco.municipio.uf : '';
        } else {
          this._cepInexistente = true;
          this.isCepMunicipio = true;
          model = new Endereco();
          model.cep = cep;
          this.uf = '';
        }
        this.modelChange.emit(model);
        this.onCepRequestSuccess.emit();
      });
    } else {
      this._cepAnterior = cep;
      this.modelChange.emit(model);
    }
  }

  municipiosPorEstado: Municipio[] = [];
  private getMunicipiosPorEstado(uf: string): Municipio[] {
    if (!uf) return [];
    return this.municipios.filter(mun => mun.uf===uf);
  }

  private _ufAnterior: string = '';
  setUf(newUf: string, newModel: Endereco) {
    this.setModel(newModel);
    if (this._ufAnterior !== newUf) {
      this._ufAnterior = newUf;
      this.municipiosPorEstado = this.getMunicipiosPorEstado(newUf);
    }
  };

  estadoFormatter = (sigla: string): string => sigla && this.estados[sigla] ? ('('+sigla+') '+this.estados[sigla]).toUpperCase() : '';
  municipioFormatter = (municipio: Municipio): string => municipio && municipio.nome ? municipio.nome.toUpperCase() : '';

  get cepInexistenteValidator(): ICustomValidator {
    var _self = this;

    return {
      key: 'cepInexistente',
      message: 'CEP Inexistente! Verifique o cadastro de município e de CEP.',
      validatorFunction: (): boolean => {
        return _self.model.cep.length === 8 && _self._cepInexistente;
      },
      params: []
    }
  }

  carregaMunicipiosFiltros(){
    this._enderecoService.listarMunicipios()
      .then(municipios => {
        this.municipios = municipios;
        this.municipiosPorEstado = this.getMunicipiosPorEstado(this.uf);
      });
  }

}
