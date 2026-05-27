import { Component, OnInit, OnDestroy, Input, Output, EventEmitter, ViewChild, OnChanges, SimpleChanges,
        ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormControl, AbstractControl, Validators, ValidatorFn } from '@angular/forms';
import { MatOptionSelectionChange } from '@angular/material';

import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';

import { InputBaseComponent } from './../input-base/input-base.component';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'suggest-combo',
  templateUrl: './suggest-combo.component.html',
  styleUrls: ['./suggest-combo.component.scss']
})
export class SuggestComboComponent extends InputBaseComponent implements OnInit, OnDestroy, OnChanges {

  @Input() formatter: (model) => string = model => (model ? ''+model : '');
  
  @Input() orderBy: { field: string, desc: boolean } = null;

  private _mustBeValid: boolean = true;
  @Input() set mustBeValid(value: boolean) { this._mustBeValid = (value!==false && value!==undefined && value!==null); };
           get mustBeValid(): boolean { return this._mustBeValid; };

  private _list: any[] = [];
  @Input() set list(value: any[]) { this._list = value; };
           get list(): any[] {
            if (!this.orderBy)
              return this._list;
            else {
              var self = this;
              return this._list.sort((a, b) => {
                let ae = a[ self.orderBy.field ];
                let be = b[ self.orderBy.field ];
                if ( ae == undefined && be == undefined ) return 0;
                if ( ae == undefined && be != undefined ) return self.orderBy.desc ? 1 : -1;
                if ( ae != undefined && be == undefined ) return self.orderBy.desc ? -1 : 1;
                if ( ae == be ) return 0;
                return self.orderBy.desc ? 
                  (ae.toString().toLowerCase() > be.toString().toLowerCase() ? -1 : 1) : 
                  (be.toString().toLowerCase() > ae.toString().toLowerCase() ? -1 : 1);
              });
            }
           };
  
  @Input() requiredErrorMessage: string = 'Campo Obrigatório!';
  
  @Input() errorMessages: {key: string, message: string}[] = [];

  @Output() textChange: EventEmitter<string> = new EventEmitter<string>();

  @ViewChild('input', { static: false }) input;

  listFiltered: Observable<any[]>

  constructor(private cdr:ChangeDetectorRef) { super(); }

  setFormControl() {
    if (!this.formGroup.contains(this.name)) this.formGroup.addControl(this.name, new FormControl());
    this.formControl = this.formGroup.controls[this.name];
  }

  ngOnInit() {
    this.setFormControl();
    this.setListFiltered();
    this.setValidators();
  }

  ngOnDestroy() {
    this.formControl.clearValidators();
    this.formControl.updateValueAndValidity();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.formGroup || changes.name) this.setFormControl();
    if (changes.required || changes.mustBeValid) this.setValidators();

    if (changes.disabled) {
      if (this.disabled) { this.formControl.disable(); }
      else { this.formControl.enable(); }
    }

    if (changes.model && this.input) this.input.nativeElement.value = this.formatter(this.model);

    if (changes.list) this.setListFiltered();

    this.cdr.detectChanges();
  }

  get caseStyle(): string {
    if (this.noCase) return 'none';
    if (this.lowerCase) return 'lowercase';
    if (this.upperCase) return 'uppercase';
    return 'none';
  }

  private _mapSearch(list: any[], formatter: (model) => string) {
    return (term: string) => {
      return !term ? list : list.filter(v => formatter(v).toLowerCase().indexOf(term.toLowerCase()) > -1);
    }
  }

  errorMessagesFiltered() {
    return this.errorMessages.filter(item => this.formControl.hasError(item.key) && !this.formControl.hasError('required') && !this.formControl.hasError('mustBeValid'));
  }

  inputChange(text: string) {
    
    if (!this.noCase) {
      if (this.upperCase) text = text.toUpperCase();
      if (this.lowerCase) text = text.toUpperCase();
    }
    this.textChange.emit(text);
    
    if (text !== this.formatter(this.model)) {
      let f = this.list.filter(item => text === this.formatter(item));
      f = (f.length === 1) ? f[0] : null;      
      
      // if (f !== null) {
        this.model = f;
        this.modelChange.emit(f);
      // }
    }
    this.formControl.updateValueAndValidity();
  }

  onFocus(element) {
    this.focus.emit();
    this.setListFiltered();
    if (this.model) element.setSelectionRange( 0, 9999); //String(this.model).length
  }
  onFocusout() {
    this.focusout.emit();
  }

  setModel(evt: MatOptionSelectionChange, newModel: any) {
    if (evt.isUserInput) {
      this.modelChange.emit(newModel);
      this.setListFiltered();
    }
    this.formControl.updateValueAndValidity();
  };

  setListFiltered() {
    this.listFiltered = this.formControl.valueChanges
        .pipe(startWith(''), map(this._mapSearch(this.list, this.formatter)));
  }

  setValidators() {
    let validators = [];
    if (this.required) validators.push(this.requiredValidator(this));
    if (this.mustBeValid) validators.push(this.invalidValidator(this));
    this.formControl.setValidators(validators);
    this.formControl.updateValueAndValidity();
  }

  requiredValidator(self: SuggestComboComponent): ValidatorFn {
      return (control: AbstractControl): { [key: string]: any } => {
        return !self.model ? { 'required': 'campo obrigatório!' } : null;
      };
  }

  invalidValidator(self: SuggestComboComponent): ValidatorFn {
      return (control: AbstractControl): { [key: string]: any } => {
        var invalid = !self.input ? false : self.input.nativeElement.value && self.formatter(self.model) != self.input.nativeElement.value;
        return (invalid ? { 'mustBeValid': 'selecione uma opção válida na lista!' } : null);
      };
  }

}
