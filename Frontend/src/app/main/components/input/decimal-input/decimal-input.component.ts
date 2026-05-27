import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { InputBaseComponent } from '../input-base/input-base.component';

@Component({
  selector: 'decimal-input',
  templateUrl: './decimal-input.component.html',
  styleUrls: ['./decimal-input.component.scss']
})
export class DecimalInputComponent extends InputBaseComponent implements OnInit {

  @Input() maxIntegerChars: number = 10;
  @Input() decimalChars: number = 2;

  private _showThousandSeparator: boolean = false;
  @Input() set showThousandSeparator(value: boolean) { this._showThousandSeparator = (value!==false && value!==undefined && value!==null); };
           get showThousandSeparator(): boolean { return this._showThousandSeparator };

  constructor() { super(); }

  ngOnInit() {
  }

  mask() {
    let numberMask= [];
    let model = this.clearModel(this.model);
    let maxChars = this.maxIntegerChars + this.decimalChars;

    if (model.length<=this.decimalChars) numberMask.push(0);
    if (model.length<this.decimalChars) {
      numberMask.push(',');
      for (var i = model.length; i < this.decimalChars; i++) {
        numberMask.push(0);
      }
    }

    for (var i = Math.min(maxChars,model.length); i >= 0; i--) {
      if (i===0 && maxChars===model.length) break;
      if (i===this.decimalChars) numberMask.push(',');
      numberMask.push(/\d/);
      var j = i - this.decimalChars;
      if ( this.showThousandSeparator && j>3 && (j-1)%3===0) numberMask.push('.');
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
    let resultModel = this.decimalModel(newModel.toString());
    this.modelChange.emit(parseFloat(resultModel));
  };

  clearModel(model: number) {
    return (model * Math.pow(10,this.decimalChars)).toFixed(0).toString();
  }

  decimalModel(model: string) {
    return ((parseInt(model) || 0) / Math.pow(10,this.decimalChars)).toFixed(this.decimalChars);
  }

}
