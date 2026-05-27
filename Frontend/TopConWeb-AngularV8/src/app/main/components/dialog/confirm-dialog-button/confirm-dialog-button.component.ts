import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material';

@Component({
  selector: 'confirm-dialog-button',
  templateUrl: './confirm-dialog-button.component.html',
  styleUrls: ['./confirm-dialog-button.component.scss']
})
export class ConfirmDialogButtonComponent implements OnInit {

  @Input() dialogMessage: string = 'Confirma?';
  @Input() buttonText: string = '';
  @Input() buttonColor: string = 'basic';
  @Input() icon: string;
  @Input() toolTip: string = '';

  private _disabled: boolean = false;
  @Input() set disabled(value: boolean) { this._disabled = (value!==false && value!==undefined && value!==null); };
           get disabled(): boolean { return this._disabled };

  private _onlyIcon: boolean = false;
  @Input() set onlyIcon(value: boolean) { this._onlyIcon = (value!==false && value!==undefined && value!==null); };
          get onlyIcon(): boolean { return this._onlyIcon };

  @Output() confirm: EventEmitter<any> = new EventEmitter<any>();
  @Output() cancel: EventEmitter<any> = new EventEmitter<any>();

  @ViewChild('confirmTemplate', { static: false }) confirmTemplate;

  private _confirmed: boolean = false;
  private _canceled: boolean = false;
  private _dialogRef: MatDialogRef<any>;
  
  constructor(public dialog: MatDialog) { }

  ngOnInit() {
  }

  openDialog() {
    if (!this.dialog) console.error('dialog property not set');

    this._confirmed = false;
    this._canceled = false;
    this._dialogRef = this.dialog.open(this.confirmTemplate);

    this._dialogRef.afterClosed().subscribe( () => {
      if (!this._confirmed && !this._canceled) this.cancel.emit();
    });

  }

  cancelDialog() {
    this._canceled = true;
    this.cancel.emit();
    this.closeModal();
  }

  confirmDialog() {
    this._confirmed = true;
    this.confirm.emit();
    this.closeModal();
  }

  closeModal() {
    if (this._dialogRef) this._dialogRef.close();
  }

}
