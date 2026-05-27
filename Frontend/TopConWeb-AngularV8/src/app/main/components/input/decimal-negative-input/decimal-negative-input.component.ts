import { Component, OnInit, Input, Output, EventEmitter, HostListener } from '@angular/core';
import { InputBaseComponent } from '../input-base/input-base.component';

@Component({
  selector: 'decimal-negative-input',
  templateUrl: './decimal-negative-input.component.html',
  styleUrls: ['./decimal-negative-input.component.scss']
})
export class DecimalNegativeInputComponent extends InputBaseComponent implements OnInit {

  @Input() maxIntegerChars: number = 10;
  @Input() decimalChars: number = 2;
  @Input() allowNegativeNumbers: boolean = false;

  private _showThousandSeparator: boolean = false;
  @Input() set showThousandSeparator(value: boolean) { this._showThousandSeparator = (value!==false && value!==undefined && value!==null); };
           get showThousandSeparator(): boolean { return this._showThousandSeparator };


  private _negativeNumber: boolean = false;

  @HostListener('keydown', ['$event'])
  onKeyDown(event: KeyboardEvent) {
    if (!this.allowNegativeNumbers) {
      return;
    }
    const key = event.key;

    if (key === '-' && !this._negativeNumber) this._negativeNumber = true;
    if (key === '+' && this._negativeNumber) this._negativeNumber = false;
  }
                  
  constructor() { super(); }

  ngOnInit() {
  }

  mask() {
    let numberMask= [];
    let model = this.clearModel(this.model);
    
    let maxChars = this.maxIntegerChars + this.decimalChars;
    if (this._negativeNumber) {
      maxChars += 1;

      if (model.length <= this.decimalChars+1) {
        numberMask.push(0);
      }
      if (model.length < this.decimalChars+1) {
        numberMask.push(',');
        for (var i = model.length; i < this.decimalChars+1; i++) {
        numberMask.push(0);
        }
      }
    } else {
  
    if (model.length <= this.decimalChars) {
      numberMask.push(0);
    }
    if (model.length < this.decimalChars) {
      numberMask.push(',');
      for (var i = model.length; i < this.decimalChars; i++) {
        numberMask.push(0);
      }
    }
  }
    for (var i = Math.min(maxChars,model.length); i >= 0; i--) {
      if (i===0 && maxChars===model.length) break;
      if (i===this.decimalChars && !this._negativeNumber) numberMask.push(',');
      if (i===this.decimalChars + 1 && this._negativeNumber && this.allowNegativeNumbers) numberMask.push(',');
      numberMask.push(/\d/);
      
      if(!this._negativeNumber && this.allowNegativeNumbers) var j = i - this.decimalChars;
      else var j = i - this.decimalChars -1;
      if ( this.showThousandSeparator && j>3 && (j-1)%3===0) numberMask.push('.');
    }

    if (this._negativeNumber && this.allowNegativeNumbers && model.length > 0 && model[0] != '-' && model != '0') model = '-' + model;
    if (this._negativeNumber && this.allowNegativeNumbers && model.length > 0 && numberMask[0] != '-') numberMask.unshift('-');
    if (!this._negativeNumber && model[0] === '-') numberMask.shift();

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

    if (this._negativeNumber && this.allowNegativeNumbers && resultModel[0] != '-') {
      resultModel = '-' + resultModel;
      this.model = parseFloat(resultModel);
    }
    
    this.modelChange.emit(parseFloat(resultModel)); 
  };

  clearModel(model: number) {
    return (model * Math.pow(10,this.decimalChars)).toFixed(0).toString();
  }

  decimalModel(model: string) {
    return ((parseFloat(model) || 0) / Math.pow(10,this.decimalChars)).toFixed(this.decimalChars);
  }

}
