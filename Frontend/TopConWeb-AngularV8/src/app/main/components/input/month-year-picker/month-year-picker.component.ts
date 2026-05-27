import { Component, OnInit, Input, OnDestroy, OnChanges, SimpleChanges, ChangeDetectorRef } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { InputBaseComponent } from '../input-base/input-base.component';
import * as moment from 'moment';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import 'moment/locale/pt-br';

export const MONTH_YEAR_FORMATS = {
  parse: {
    dateInput: 'MM/YYYY',
  },
  display: {
    dateInput: 'MM/YYYY',
    monthYearLabel: 'MMMM [de] YYYY',
    dateA11yLabel: 'MMMM [de] YYYY',
    monthYearA11yLabel: 'MMMM [de] YYYY',
  },
};

@Component({
  selector: 'month-year-picker',
  templateUrl: './month-year-picker.component.html',
  styleUrls: ['./month-year-picker.component.scss'],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'pt-BR' },
    { provide: MAT_DATE_FORMATS, useValue: MONTH_YEAR_FORMATS },
  ],
})
export class MonthYearPickerComponent extends InputBaseComponent implements OnInit, OnDestroy, OnChanges {
  @Input() requiredErrorMessage: string = 'Campo Obrigatório!';
  @Input() errorMessages: { key: string, message: string }[] = [];

  formControl: FormControl = new FormControl();
  convertedModel: Date;

  private _isFocused: boolean = false;

  constructor(private cdr: ChangeDetectorRef, private adapter: DateAdapter<any>) {
    super();
    moment.locale('pt-br');
    this.adapter.setLocale('pt-BR');
  }

  ngOnInit() {
    this.setFormControl();
    this.setValidators();
    this.adapter.setLocale('pt-BR');
    this.convertedModel = this.convertDate(this.model);
  }

  ngOnDestroy() {
    this.formControl.clearValidators();
    this.formControl.updateValueAndValidity();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.formGroup || changes.name) this.setFormControl();
    if (changes.required) this.setValidators();
    if (changes.disabled) {
      this.disabled ? this.formControl.disable() : this.formControl.enable();
    }
    if (changes.model && !this._isFocused) {
      this.convertedModel = this.convertDate(this.model);
    }
    this.cdr.detectChanges();
  }

  setFormControl() {
    if (this.formGroup && this.name) {
      if (!this.formGroup.contains(this.name)) {
        this.formGroup.addControl(this.name, this.formControl);
      } else {
        this.formControl = this.formGroup.controls[this.name] as FormControl;
      }
    } else {
      console.warn('formGroup or name is not defined in MonthYearPickerComponent');
    }
  }

  setValidators() {
    const validators = [];
    if (this.required) validators.push(Validators.required);
    this.formControl.setValidators(validators);
    this.formControl.updateValueAndValidity();
  }

  errorMessagesFiltered() {
    if (!this.formControl) return [];
    return this.errorMessages.filter(
      (item) => this.formControl.hasError(item.key) && !this.formControl.hasError('required')
    );
  }

  onFocus(element) {
    this._isFocused = true;
    this.focus.emit();
  }

  onFocusout() {
    this._isFocused = false;
    this.focusout.emit();
  }

  setModel(newModel: any) {
    let modelAux = newModel;
    if (moment.isMoment(newModel)) {
      modelAux = newModel.toDate();
    }
    this.modelChange.emit(modelAux);
  }

  convertDate(date: any) {
    if (moment.isMoment(date)) {
      return date.toDate();
    }
    return date;
  }

  yearSelected(normalizedYear: moment.Moment) {
    const ctrlValue = this.formControl.value ? moment(this.formControl.value) : moment();
    ctrlValue.year(normalizedYear.year());
    this.formControl.setValue(ctrlValue);
    this.convertedModel = this.convertDate(ctrlValue);
    this.modelChange.emit(this.convertedModel);
  }

  monthSelected(normalizedMonth: moment.Moment, datepicker) {
    const ctrlValue = this.formControl.value ? moment(this.formControl.value) : moment();
    ctrlValue.month(normalizedMonth.month());
    this.formControl.setValue(ctrlValue);
    this.convertedModel = this.convertDate(ctrlValue);
    this.modelChange.emit(this.convertedModel);
    datepicker.close();
  }
}
