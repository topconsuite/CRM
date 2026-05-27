import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { Tasks } from 'app/classes/_tasks/tasks';
import { ProgramacaoLog } from 'app/classes/programacao/programacao.classes';

@Component({
  selector: 'app-programacao-log-dialog',
  templateUrl: './programacao-log-dialog.component.html',
  styleUrls: ['./programacao-log-dialog.component.scss']
})
export class ProgramacaoLogDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<ProgramacaoLogDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { programacaoLogs: ProgramacaoLog[], afterCloseCallback?: Function }
  ) { }

  ngOnInit() {
  }

  closeDialog() {
    this.dialogRef.close();
    if (this.data.afterCloseCallback) this.data.afterCloseCallback();
  }

  formataDataHora = Tasks.formataDataHora;

}
