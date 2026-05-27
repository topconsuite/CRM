import { Component, OnInit, OnDestroy, Input, Output, EventEmitter, OnChanges, SimpleChanges,
      ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormControl, AbstractControl, Validators, ValidatorFn } from '@angular/forms';
import { InputBaseComponent } from './../input-base/input-base.component';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'simple-input',
  templateUrl: './simple-input.component.html',
  styleUrls: ['./simple-input.component.scss']
})
export class SimpleInputComponent extends InputBaseComponent implements OnInit, OnDestroy, OnChanges {

  @Input() mask?: any;
  
  private _numeric: boolean = false;
  @Input() set numeric(value: boolean) { this._numeric = (value!==false && value!==undefined && value!==null); };
           get numeric(): boolean { return this._numeric };
  
  @Input() requiredErrorMessage: string = 'Campo Obrigatório!';
  
  private _multiline: boolean = false;
  @Input() set multiline(value: boolean) { this._multiline = (value!==false && value!==undefined && value!==null); };
           get multiline(): boolean { return this._multiline };
  
  private _email: boolean = false;
  @Input() set email(value: boolean) { this._email = (value!==false && value!==undefined && value!==null); };
           get email(): boolean { return this._email };

  private _logradouro: boolean = false;
  @Input() set logradouro(value: boolean) { this._logradouro = (value!==false && value!==undefined && value!==null); };
          get logradouro(): boolean { return this._logradouro };

  @Input() cpf: boolean;
  @Input() cnpj: boolean;
  @Input() minLength: number = 0;
  @Input() maxLength: number = 0;
  @Input() errorMessages: {key: string, message: string}[] = [];

  unsmaskChars: string[] = [];

  constructor(private cdr:ChangeDetectorRef) { super(); }

  setFormControl() {
    if (!this.formGroup.contains(this.name)) this.formGroup.addControl(this.name, new FormControl());
    this.formControl = this.formGroup.controls[this.name];
  }

  ngOnInit() {
    this.setFormControl();
    this.setValidators();
  }

  ngOnDestroy() {
    this.formControl.clearValidators();
    this.formControl.updateValueAndValidity();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.formGroup || changes.name)
      this.setFormControl();
    
    if (changes.validator || changes.required || changes.numeric || changes.email || changes.logradouro || changes.cpf || changes.cnpj || changes.minLength || changes.maxLength)
      this.setValidators();
    
    if (changes.disabled) {
      if (this.disabled) { this.formControl.disable(); }
      else { this.formControl.enable(); }
    }

    this.cdr.detectChanges();
  }

  errorMessagesFiltered() {
    return this.errorMessages.filter(item => this.formControl.hasError(item.key));
  }

  defaultErrorMessages() {
    let _defaultErrorMessages: {key: string, message: string}[] = [];
    
    Object.keys(this.formControl.errors || {}).forEach(key => {
    if (!this.errorMessages.map<string>(t => t.key).includes(key))
        _defaultErrorMessages.push({ key: key, message: this.formControl.errors[key] });
    });
    
    return _defaultErrorMessages;
  }
  
  onFocus(element) {
    this.focus.emit();
    if (this.model) element.setSelectionRange( 0, 9999); //String(this.model).length
  }
  onFocusout() {
    this.model = this.sanitize(this.model);
    this.cdr.detectChanges();
    this.focusout.emit();
  }

  get caseStyle(): string {
    if (this.noCase) return 'none';
    if (this.lowerCase) return 'lowercase';
    if (this.upperCase) return 'uppercase';
    return 'none';
  }

  sanitize(value) {
    var sanitized = (value);
    
    if (this.email) {
      sanitized = value.split(';').map(t => t.trim()).join('; ');
    } else if (typeof(value) === 'string') {
      sanitized = this.replaceAll(value, "'", "").trim();
    }

    return sanitized;
  }

  setModel(newModel: any) {
    let resultModel = this.mask ? this.unmask(newModel.toString()) : newModel;
    if (typeof(resultModel) === 'string' && !this.noCase) {
      if (this.upperCase) resultModel = resultModel.toUpperCase();
      if (this.lowerCase) resultModel = resultModel.toLowerCase();
    }

    // Removendo o caracter apostrofo para não poder ser informado em nenhum campo
    if (typeof(resultModel) === 'string') {
      resultModel = resultModel.replace("'", "");
    }//*/

    if (this.email) {
      resultModel = this.sanitize(resultModel);
    }

    this.modelChange.emit(resultModel);
  };

  setValidators() {
    let validators = [];
    validators.push(this.aspaSimplesValidator(this));
    if (this.validator) validators.push(this.createValidator(this.validator));
    if (this.required) {
      validators.push(!this.numeric ? Validators.required : this.requiredNumericValidator(this));
      if (!this.numeric) validators.push(this.requiredTrimValidator(this));
    }
    if (this.email) validators.push(this.emailValidator(this));
    if (this.logradouro) validators.push(this.logradouroValidator(this));
    if (this.cpf) validators.push(this.cpfValidator(this));
    if (this.cnpj) validators.push(this.cnpjValidator(this));
    if (this.minLength > 0) validators.push(this.minLengthValidator(this.minLength, this));
    if (this.maxLength > 0) validators.push(this.maxLengthValidator(this.maxLength, this));
    this.formControl.setValidators(validators);
    this.formControl.updateValueAndValidity();
  }

  private _unmask(stringMasked: string, exclude: string[]): string {
    let result = stringMasked;
    exclude.forEach(element => {
      result = result.split(element).join('');
    });
    return result;
  }

  unmask(maskedString: string): string {
    this.unsmaskChars = [];
    this.mask.mask.forEach(item => {
      if ((typeof item) === 'string') this.unsmaskChars.push(item);
    });
    this.unsmaskChars.push(this.mask.placeholderChar || '_');
    return this._unmask(maskedString, this.unsmaskChars);
  }

  aspaSimplesValidator(self: SimpleInputComponent): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } => {
      if (typeof(self.model) === 'string') {
        return self.model.includes("'") ? { 'invalidchar': 'Caractere apóstrofo não é permitido!' } : null;
      } else {
        return null;
      }
    };
  }

  requiredTrimValidator(self: SimpleInputComponent): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } => {
      var model = (self.model || '').toString();
      return (model.trim() === '') ? { 'emptystring': 'Valor inválido, foi digitado apenas espaços!' } : null;
    };
}

  requiredNumericValidator(self: SimpleInputComponent): ValidatorFn {
      return (control: AbstractControl): { [key: string]: any } => {
        let value = parseInt(self.model || '0');
        return value===0 ? { 'required': 'Campo obrigatório!' } : null;
      };
  }

  minLengthValidator(minLength: number, self: SimpleInputComponent): ValidatorFn {
      return (control: AbstractControl): { [key: string]: any } => {
        var len = (control.value || '').length;
        return (len>0 && len<minLength) ? { 'minLength': 'tamanho mínimo: '+minLength } : null;
      };
  }
  maxLengthValidator(maxLength: number, self: SimpleInputComponent): ValidatorFn {
      return (control: AbstractControl): { [key: string]: any } => {
        var len = (control.value || '').length;
        return len > maxLength ? { 'maxLength': 'tamanho máximo: '+maxLength } : null;
      };
  }

  emailValidator(self: SimpleInputComponent): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } => {
        let value: string = control.value || '';
        if (value === '') return null;
        if (!self.multiline) return !self.isValidEmail(value.trim()) ? { 'email': 'email inválido!' } : null;
        let strEmails: string = value.replace(/(\r\n|\n|\r)/gm,'').trim();
        let valid = strEmails.split(';').filter(item => !self.isValidEmail(item.trim())).length === 0;
        return !valid ? { 'email': 'email inválido!' } : null;
      };
  }
  isValidEmail(strEmail: string): boolean {
    //let re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    let re = /^[-a-zA-Z0-9][-_.a-zA-Z0-9]*@[-.a-zA-Z0-9]+(\.[-.a-zA-Z0-9]+)*\.(com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|tv|live|haus|energy|casa|online|solar|top|inc|tech|[a-zA-Z]{2})$/
    let result = strEmail.match(re);
    return (result !== null && result.length > 0);
  }

  logradouroValidator(self: SimpleInputComponent): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } => {
        let value: string = control.value || '';
        if (value === '') return null;
        let p = value.indexOf(" ") + 1;
        if (p === 0 && value.length > 0) return { 'logradouro': 'endereço sem tipo de logradouro!' };
        return (p > 4) ? { 'logradouro': 'tipo de logradouro deverá ter no máximo 3 dígitos!' } : null;
      };
  }

  cpfValidator(self: SimpleInputComponent): ValidatorFn {
      return (control: AbstractControl): { [key: string]: any } => {
        return (control.value || '').length>0
          && !self.isValidCPF(self.model.toString()) ? { 'cpf': 'cpf inválido!' } : null;
      };
  }
  isValidCPF(strCPF: string): boolean {
    var Soma = 0;
    var Resto;

    if (strCPF.length < 11) return false;
    if (strCPF === "00000000000") return false;
      
    for (var i=1; i<=9; i++) Soma += (parseInt(strCPF.substring(i-1, i)) * (11 - i));
    Resto = (Soma * 10) % 11;
    
    if ((Resto ===10) || (Resto === 11)) Resto = 0;
    if (Resto !== parseInt(strCPF.substring(9, 10)) ) return false;
    
    Soma = 0;
    for (var i = 1; i <= 10; i++) Soma += (parseInt(strCPF.substring(i-1, i)) * (12 - i));
    Resto = (Soma * 10) % 11;
  
    if ((Resto === 10) || (Resto === 11)) Resto = 0;
    if (Resto !== parseInt(strCPF.substring(10, 11) ) ) return false;
    return true;
  }

  cnpjValidator(self: SimpleInputComponent): ValidatorFn {
      return (control: AbstractControl): { [key: string]: any } => {
        return (control.value || '').length>0
          && !self.isValidCNPJ(self.model.toString()) ? { 'cnpj': 'cnpj inválido!' } : null;
      };
  }
  isValidCNPJ(cnpj: string): boolean {
    // 1. Remove caracteres não alfanuméricos e converte para maiúsculo
    // Importante: O novo CNPJ diferencia maiúsculas na validação se não padronizar.
    const strCNPJ = cnpj.replace(/[^a-zA-Z0-9]/g, '').toUpperCase();

    // 2. Verifica tamanho (deve ter 14 caracteres)
    if (strCNPJ.length !== 14) return false;

    // 3. Verifica se todos os caracteres são iguais (ex: 0000... ou AAAA...)
    // Embora raro no alfanumérico, ainda é uma verificação válida de segurança
    if (/^([a-zA-Z0-9])\1+$/.test(strCNPJ)) return false;

    // Função auxiliar para converter Char em Valor Numérico (Regra ASCII - 48)
    const getCharValue = (char: string): number => {
        return char.charCodeAt(0) - 48;
    };

    // --- CÁLCULO DO PRIMEIRO DÍGITO VERIFICADOR (13º caractere) ---
    let tamanho = strCNPJ.length - 2;
    let numeros = strCNPJ.substring(0, tamanho);
    let digitos = strCNPJ.substring(tamanho);
    let soma = 0;
    let pos = tamanho - 7;

    for (let i = tamanho; i >= 1; i--) {
        // AQUI ESTÁ A MUDANÇA PRINCIPAL: charCodeAt - 48
        soma += getCharValue(numeros.charAt(tamanho - i)) * pos--;
        if (pos < 2) pos = 9;
    }

    let resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    
    // Verifica se o 1º dígito calculado bate com o 13º caractere
    // Nota: Os dígitos verificadores (pos 13 e 14) continuam sendo NUMÉRICOS
    if (resultado !== parseInt(digitos.charAt(0))) return false;


    // --- CÁLCULO DO SEGUNDO DÍGITO VERIFICADOR (14º caractere) ---
    tamanho = tamanho + 1;
    numeros = strCNPJ.substring(0, tamanho);
    soma = 0;
    pos = tamanho - 7;

    for (let i = tamanho; i >= 1; i--) {
        soma += getCharValue(numeros.charAt(tamanho - i)) * pos--;
        if (pos < 2) pos = 9;
    }

    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;

    // Verifica se o 2º dígito calculado bate com o 14º caractere
    if (resultado !== parseInt(digitos.charAt(1))) return false;

    return true;
}

}
