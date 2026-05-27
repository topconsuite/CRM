import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';

import { InputBaseComponent } from './../input-base/input-base.component';

@Component({
  selector: 'integer-input',
  templateUrl: './integer-input.component.html',
  styleUrls: ['./integer-input.component.scss']
})
export class IntegerInputComponent extends InputBaseComponent implements OnInit {

  @Input() errorMessages: {key: string, message: string}[] = [];

  @Input() maxIntegerChars: number = 10;

  private _showThousandSeparator: boolean = false;
  @Input() set showThousandSeparator(value: boolean) { this._showThousandSeparator = (value!==false && value!==undefined && value!==null); };
           get showThousandSeparator(): boolean { return this._showThousandSeparator };

  constructor() { super(); }

  ngOnInit() {
  }

  mask() {
    let numberMask= [];
    
    //for (var i = Math.min(this.maxIntegerChars,this.model.toString().length); i >= 0; i--) {
    for (var i = this.maxIntegerChars; i >= 0; i--) {
      if (i===0 && this.maxIntegerChars===this.model.toString().length) break;
      numberMask.push(/\d/);
      if ( this.showThousandSeparator && i>3 && (i-1)%3===0) numberMask.push('.');
    }
    
    return numberMask;
  }

  onFocus() {
    this.focus.emit();
  }
  onFocusout() {
    this.focusout.emit();
  }

  setModel(newModel: string) {
    this.modelChange.emit(parseInt(newModel.substr(0, this.maxIntegerChars)) || 0);
  };

}
