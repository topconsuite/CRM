import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, AbstractControl, ValidatorFn } from '@angular/forms';

import { ICustomValidator } from '../../interfaces/custom-validator';

@Component({
  template: ''
})
export class InputBaseComponent {

  @Input() formGroup: FormGroup;
  @Input() placeholder: String = "Placeholder";
  @Input() name: string = "name";
  @Input() model: any;
  
  private _required: boolean = false;
  @Input() set required(value: boolean) { this._required = (value!==false && value!==undefined && value!==null); };
           get required(): boolean { return this._required };
  
  private _disabled: boolean = false;
  @Input() set disabled(value: boolean) { this._disabled = (value!==false && value!==undefined && value!==null); };
           get disabled(): boolean { return this._disabled };

  private _noCase: boolean = false;
  @Input() set noCase(value: boolean) { this._noCase = (value!==false && value!==undefined && value!==null); };
           get noCase(): boolean { return this._noCase };

  private _upperCase: boolean = true;
  @Input() set upperCase(value: boolean) { this._upperCase = (value!==false && value!==undefined && value!==null); };
           get upperCase(): boolean { return this._upperCase };

  private _lowerCase: boolean = false;
  @Input() set lowerCase(value: boolean) { this._lowerCase = (value!==false && value!==undefined && value!==null); };
           get lowerCase(): boolean { return this._lowerCase };

  private _bold: boolean = false;
  @Input() set bold(value: boolean) { this._bold = (value!==false && value!==undefined && value!==null); };
          get bold(): boolean { return this._bold };

  @Input() validator: ICustomValidator = null;

  @Output() modelChange: EventEmitter<any> = new EventEmitter<any>();
  @Output() focus: EventEmitter<any> = new EventEmitter<any>();
  @Output() focusout: EventEmitter<any> = new EventEmitter<any>();

  formControl: AbstractControl;

  constructor() { }

  createValidator(validator: ICustomValidator): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } => {
      var validation = {};
      validation[validator.key] = validator.message;
      return validator.validatorFunction(...validator.params) ? validation : null;
    };
  }

  replaceAll(str: string, oldValue: string, newValue: string): string{
    var result = (str);
    var pos = result.indexOf(oldValue);
    while (pos > -1){
      result = result.replace(oldValue, newValue);
      pos = result.indexOf(oldValue);
    }
    return result;
  }

}
