import { Component, OnInit, OnDestroy, Input, OnChanges, SimpleChanges,
  ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { FormControl } from '@angular/forms';

import { InputBaseComponent } from './../input-base/input-base.component';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'radio-input',
  templateUrl: './radio-input.component.html',
  styleUrls: ['./radio-input.component.scss']
})
export class RadioInputComponent extends InputBaseComponent implements OnInit, OnDestroy, OnChanges {

  @Input() title: string = '';
  @Input() formatter: (model) => string = model => (model ? ''+model : '');
  @Input() valueFormatter: (model) => any = model => model;
  @Input() valueDisable: (model) => any = model => false;
  @Input() list: any[] = [];
  
  private _noneOption: boolean = false;
  @Input() set noneOption(value: boolean) { this._noneOption = (value!==false && value!==undefined && value!==null); };
           get noneOption(): boolean { return this._noneOption };
  
  @Input() noneDescription: string = '-';

  constructor(private cdr:ChangeDetectorRef) { super(); }

  setFormControl() {
    if (!this.formGroup.contains(this.name)) this.formGroup.addControl(this.name, new FormControl());
    this.formControl = this.formGroup.controls[this.name];
  }

  ngOnInit() {
    this.setFormControl();
  }

  ngOnDestroy() {
    this.formControl.clearValidators();
    this.formControl.updateValueAndValidity();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.formGroup || changes.name) this.setFormControl();

    if (changes.disabled) {
      if (this.disabled) { this.formControl.disable(); }
      else { this.formControl.enable(); }
    }

    this.cdr.detectChanges();
  }

  setModel(newModel: any) {
    this.modelChange.emit(newModel);
    this.formControl.updateValueAndValidity();
  };

}
