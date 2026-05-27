import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { Tasks } from 'app/classes/_tasks/tasks';
import { ObraLog } from 'app/classes/obra/obra.classes';

@Component({
  selector: 'obra-log-dialog',
  templateUrl: './obra-log-dialog.component.html',
  styleUrls: ['./obra-log-dialog.component.scss']
})
export class ObraLogDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<ObraLogDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { obraLogs: ObraLog[], afterCloseCallback?: Function }
  ) { }

  ngOnInit() {
  }

  closeDialog() {
    this.dialogRef.close();
    if (this.data.afterCloseCallback) this.data.afterCloseCallback();
  }

  formataDataHora = Tasks.formataDataHora;

}
