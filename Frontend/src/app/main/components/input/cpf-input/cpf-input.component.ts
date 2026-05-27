import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { InputBaseComponent } from '../input-base/input-base.component';

@Component({
  selector: 'cpf-input',
  templateUrl: './cpf-input.component.html',
  styleUrls: ['./cpf-input.component.scss']
})
export class CpfInputComponent extends InputBaseComponent implements OnInit {

  maskCPF = [/\d/, /\d/, /\d/, '.', /\d/, /\d/, /\d/, '.', /\d/, /\d/, /\d/, '-', /\d/, /\d/];

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
