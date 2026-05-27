import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'alert-dialog',
  templateUrl: './alert-dialog.component.html',
  styleUrls: ['./alert-dialog.component.scss']
})
export class AlertDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<AlertDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { title: string, message: string, afterCloseCallback?: Function }
  ) { }

  ngOnInit() {
  }

  closeDialog() {
    this.dialogRef.close();
    if (this.data.afterCloseCallback) this.data.afterCloseCallback();
  }

}
