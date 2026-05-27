import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { InputBaseComponent } from '../input-base/input-base.component';

@Component({
  selector: 'cnpj-input',
  templateUrl: './cnpj-input.component.html',
  styleUrls: ['./cnpj-input.component.scss']
})
export class CnpjInputComponent extends InputBaseComponent implements OnInit {

  alphanum = /[A-Z0-9]/;
  digit = /\d/;

  maskCNPJ = [
    this.alphanum, this.alphanum, '.', 
    this.alphanum, this.alphanum, this.alphanum, '.', 
    this.alphanum, this.alphanum, this.alphanum, '/', 
    this.alphanum, this.alphanum, this.alphanum, this.alphanum, 
    '-', 
    this.digit, this.digit // Os dígitos verificadores continuam sendo apenas números
  ];
  
  // maskCNPJ = [/\d/, /\d/, '.', /\d/, /\d/, /\d/, '.', /\d/, /\d/, /\d/, '/', /\d/, /\d/, /\d/, /\d/,'-', /\d/, /\d/];

  constructor() { super(); }

  ngOnInit() {
  }

  setModel(newModel: any) {
    this.modelChange.emit(newModel);
  };

  onFocus() {
    this.focus.emit();
  }
  onFocusout() {
    this.focusout.emit();
  }

}
