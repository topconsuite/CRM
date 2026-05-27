import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { InputBaseComponent } from './../input-base/input-base.component';
import { ICustomValidator } from '../../interfaces/custom-validator';

@Component({
  selector: 'phone-input',
  templateUrl: './phone-input.component.html',
  styleUrls: ['./phone-input.component.scss']
})
export class PhoneInputComponent extends InputBaseComponent implements OnInit {

  @Input() modelDdd: string;
  @Input() modelNumber: string;

  @Output() modelDddChange: EventEmitter<number> = new EventEmitter<number>();
  @Output() modelNumberChange: EventEmitter<number> = new EventEmitter<number>();

  get modelPhone(): string {
    let ddd = parseInt(this.modelDdd) === 0 || isNaN(parseInt(this.modelDdd)) ? '' : this.modelDdd.toString();
    let number = parseInt(this.modelNumber) === 0 || isNaN(parseInt(this.modelNumber)) ? '' : this.modelNumber.toString();
    if (ddd.length === 0 && number.length > 0) ddd = '00';
    else if (ddd.length === 1 && number.length > 0) ddd = '0'+ddd;
    return ddd +''+ number;
  }

  constructor() { super(); }

  ngOnInit() {
  }

  maskTelefone() {
    if (this.modelPhone.length < 11) return ['(', /\d/, /\d/, ')', /\d/, /\d/, /\d/, /\d/, '-',/\d/, /\d/, /\d/, /\d/, /\d/];
    return ['(', /\d/, /\d/, ')', /\d/, /\d/, /\d/, /\d/, /\d/, '-',/\d/, /\d/, /\d/, /\d/];
  }

  onFocus() {
    this.focus.emit();
  }
  onFocusout() {
    this.focusout.emit();
  }

  setModel(newModel: string) {
    
    let ddd: any = newModel.substr(0, 2);
    let number: any = newModel.substr(2);

    ddd = parseInt(ddd) === 0 || isNaN(parseInt(ddd)) ? 0 : parseInt(ddd);
    number = parseInt(number) === 0 || isNaN(parseInt(number)) ? 0 : parseInt(number);

    this.modelDddChange.emit(ddd);
    this.modelNumberChange.emit(number);
  };

  get dddValidator(): ICustomValidator {
    var _self = this;

    var message = 'Obrigatório informar o DDD!';

    return {
      key: 'dddInvalido',
      message: message,
      validatorFunction: (ddd: number, number: number): boolean => {
        var invalido = number > 0 && ddd < 10;
        if (invalido) {
          var control = _self.formGroup.controls[_self.name];
          if (control) {
            control.markAsDirty();
            control.markAsTouched();
          }
        }
        return invalido;
      },
      params: [_self.modelDdd, _self.modelNumber]
    }
  }

}
