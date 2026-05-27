import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.scss']
})
export class ConfirmDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<ConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {
      title: string,
      message: string,
      confirmText?: string,
      confirmCallback?: Function,
      cancelText?: string,
      cancelCallback?: Function
    }
  ) { }

  ngOnInit() {
  }

  confirm() {
    this.dialogRef.close();
    if (this.data.confirmCallback) this.data.confirmCallback();
  }

  cancel() {
    this.dialogRef.close();
    if (this.data.cancelCallback) this.data.cancelCallback();
  }

}
