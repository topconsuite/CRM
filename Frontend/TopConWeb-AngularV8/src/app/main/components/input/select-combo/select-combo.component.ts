import { Component, OnInit, OnDestroy, Input, Output, EventEmitter, OnChanges, SimpleChanges,
        ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormControl, Validators, AbstractControl } from '@angular/forms';
import { MatOptionSelectionChange } from '@angular/material';

import { InputBaseComponent } from './../input-base/input-base.component';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'select-combo',
  templateUrl: './select-combo.component.html',
  styleUrls: ['./select-combo.component.scss']
})
export class SelectComboComponent extends InputBaseComponent implements OnInit, OnDestroy, OnChanges {

  @Input() formatter: (model) => string = model => (model ? ''+model : '');
  @Input() list: any[] = [];
  @Input() colorList: string[] = [];

  private _multiple: boolean = false;
  @Input() set multiple(value: boolean) { this._multiple = (value!==false && value!==undefined && value!==null); };
           get multiple(): boolean { return this._multiple };

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
    if (changes.formGroup || changes.name) this.setFormControl();
    if (changes.required || changes.email) this.setValidators();

    if (changes.disabled) {
      if (this.disabled) { this.formControl.disable(); }
      else { this.formControl.enable(); }
    }

    this.cdr.detectChanges();
  }

  getColor(model: any): string {
    if (model===null || model===undefined) return '';
    return this.list.includes(model) ? this.colorList[this.list.indexOf(model)] : '';
  }

  setModel(evt: MatOptionSelectionChange, newModel: any) {
    if (evt.isUserInput) {
      this.modelChange.emit(newModel);
    }
  };

  setModelMultiple(evt: MatOptionSelectionChange, newModel: any) {
    if (evt.isUserInput) {
      
      if (this.model.map(t => this.formatter(t)).includes(this.formatter(newModel)))
        this.model = this.model.filter(t => this.formatter(t) !== this.formatter(newModel));
      else
        this.model.push(newModel);
      
      this.modelChange.emit(this.model);
    }
  };

  selectAll(evt: MatOptionSelectionChange) {
    if (evt.isUserInput) {
      let allSelected = (this.list.length !== this.model.length);

      if (allSelected) {
        this.model = JSON.parse(JSON.stringify(this.list));
      } else {
        this.model = [];
      }

      this.modelChange.emit(this.model);
    }    
  }

  listValue(): string[] {
    if (!this.model)
      return [];

    return this.model.map(t => this.formatter(t));
  }

  setValidators() {
    let validators = [];
    if (this.required) this.formControl.setValidators(Validators.required);
    this.formControl.setValidators(validators);
    this.formControl.updateValueAndValidity();
  }

}
