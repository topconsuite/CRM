import { Component, OnInit, OnDestroy, Input, OnChanges, SimpleChanges,
        ChangeDetectionStrategy, ChangeDetectorRef, ViewChild } from '@angular/core';
import { AbstractControl, FormControl, ValidatorFn, Validators } from '@angular/forms';
import { MAT_MOMENT_DATE_FORMATS, MomentDateAdapter } from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { InputBaseComponent } from '../input-base/input-base.component';

export class CustomDateAdapter extends MomentDateAdapter {
  constructor() {
    super("pt-BR");
  }

  getFirstDayOfWeek(): number {
    return 0;
  }

  format(date, displayFormat): string {
    this.setWeekendColor();
    
    var d = date._d.getDay();
    var weekendTag = (displayFormat === 'LL' && (d === 0 || d === 6)) ? ' (weekend)' : '';
    return super.format(date, displayFormat) + weekendTag;
  }

  private _timeoutWeekendColor = null;
  setWeekendColor(): void {
    if (this._timeoutWeekendColor) {
      clearTimeout(this._timeoutWeekendColor);
    }

    this._timeoutWeekendColor = setTimeout(() => {
      var cells = document.getElementsByClassName('mat-calendar-body-cell');

      if (cells && cells.length > 0) {
        for (let i = 0; i < cells.length; i++) {
          var element = cells[i];
          if (element.getAttribute('aria-label').includes(' (weekend)')) {
            var cell = element.firstElementChild;
            cell.setAttribute("style", "color: red;");
          }
        }
      }

      this._timeoutWeekendColor = null;
    }, 50);

  }
}

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'date-picker',
  templateUrl: 'date-picker.component.html',
  styleUrls: ['date-picker.component.scss'],
  providers: [
    {provide: MAT_DATE_LOCALE, useValue: 'pt-BR'},
    {provide: DateAdapter, useClass: CustomDateAdapter, deps: [MAT_DATE_LOCALE]},
    {provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS}
  ],
})
export class DatePickerComponent extends InputBaseComponent implements OnInit, OnDestroy, OnChanges {

  @Input() requiredErrorMessage: string = 'Campo Obrigatório!';
  @Input() errorMessages: {key: string, message: string}[] = [{ key: 'minDate', message: 'Data deve ser maior ou igual à data atual.' }, { key: 'maxDate', message: 'Data deve ser menor ou igual à data atual.' }];

  @Input() minDate: Date = null;
  @Input() maxDate: Date = null;

  @ViewChild('dp', { static: false }) private _dp: any;

  convertedModel: Date;

  constructor(private adapter: DateAdapter<any>, private cdr:ChangeDetectorRef) { super(); }

  setFormControl() {
    if (!this.formGroup.contains(this.name)) this.formGroup.addControl(this.name, new FormControl());
    this.formControl = this.formGroup.controls[this.name];
  }

  ngOnInit() {
    this.convertedModel = this.convertDate(this.model);
    this.adapter.setLocale('pt');
    this.setFormControl();
    this.setValidators();
  }

  ngOnDestroy() {
    this.formControl.clearValidators();
    this.formControl.updateValueAndValidity();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.formGroup || changes.name) this.setFormControl();
    if (changes.required || changes.maxDate || changes.minDate) this.setValidators();
    if (changes.model && !this._isFocused) this.convertedModel=this.convertDate(this.model);

    if (changes.disabled) {
      if (this.disabled) { this.formControl.disable(); }
      else { this.formControl.enable(); }
    }
    
    //this.cdr.detectChanges();
  }

  errorMessagesFiltered() {
    return this.errorMessages.filter(item => this.formControl.hasError(item.key) && !this.formControl.hasError('required'));
  }

  defaultErrorMessages() {
    let _defaultErrorMessages: {key: string, message: string}[] = [];
    
    Object.keys(this.formControl.errors || {}).forEach(key => {
    if (!this.errorMessages.map<string>(t => t.key).includes(key))
        _defaultErrorMessages.push({ key: key, message: this.formControl.errors[key] });
    });
    
    return _defaultErrorMessages;
  }

  private _isFocused: boolean = false;
  onFocus(element) {
    this._isFocused = true;
    this._dp.open();
    this.focus.emit();
    if (this.model) element.setSelectionRange( 0, 9999); //String(this.model).length
  }
  onFocusout() {
    this._isFocused = false;
    this.focusout.emit();
  }

  setModel(newModel: any) {
    let newDate = null;
    let modelAux = newModel;
    if (newModel && newModel.hasOwnProperty('_d')) modelAux = new Date(newModel._d);
    //if (modelAux && modelAux.getFullYear) newDate = { year: modelAux.getFullYear(), month: modelAux.getMonth()+1, day: modelAux.getDate() };
    //this.modelChange.emit(newDate);
    this.modelChange.emit(modelAux);

    this.cdr.markForCheck();
  };

  setValidators() {
    let validators = [];
    if (this.required) validators.push(Validators.required);
    if (this.minDate != null) validators.push(this.minDateValidator(this.minDate, this));
    if (this.maxDate != null) validators.push(this.maxDateValidator(this.maxDate, this));
    this.formControl.setValidators(validators);
    this.formControl.updateValueAndValidity();
  }

  minDateValidator(minDate: Date, self: DatePickerComponent): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } => {
      if (control.value == null) return null;
      var date = (new Date(control.value) || null);
      minDate = new Date(minDate);
      return (date < minDate) ? { 'minDate': 'Data deve ser maior ou igual a '+minDate } : null;
    }
  }

  maxDateValidator(maxDate: Date, self: DatePickerComponent): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } => {
      if (control.value == null) return;
      var date = (new Date(control.value) || null);
      maxDate = new Date(maxDate);
      return (date > maxDate) ? { 'maxDate': 'Data deve ser menor ou igual a '+maxDate } : null;
    }
  }

  convertDate(date) {
    //if (!date || !date.hasOwnProperty('year')) return null;
    //return new Date(date.year, date.month - 1, date.day);
    let modelAux = date;
    if (date && date.hasOwnProperty('_d')) modelAux = new Date(date._d);
    return modelAux;
  }

}