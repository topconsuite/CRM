import { Component, Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material';
import { Router } from '@angular/router';
import { XHRBackend, RequestOptions, Request, RequestOptionsArgs, Response, Http, Headers } from '@angular/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

import { AuthService } from './auth.service';
import { Constants } from './../../app-settings/constants'

const lsKeyLogged: string = 't.tcw.isLogged';

const UrlsUnauthorized: string[] = [
  '/api/v1/obra/alterar-status-cadastro-e-analista',
  '/api/v1/obra/pendente/aprovar',
  '/api/v1/obra/aprovar-engenharia',
  '/api/v1/obra/aprovar-pagamentos'
]

@Component({
  selector: 'loading-dialog',
  styleUrls: [],
  template: '<mat-spinner></mat-spinner>',
})
export class LoadingDialog {}

@Injectable()
export class InterceptedHttpService extends Http {

  private _dialogRef: MatDialogRef<any>;

  private _requestsInitiated = 0;

  constructor(
    private backend: XHRBackend,
    private defaultOptions: RequestOptions,
    public auth: AuthService,
    public dialog: MatDialog,
    public router: Router
  ) {
    super(backend, defaultOptions);
  }

  request(url: string | Request, options?: RequestOptionsArgs): Observable<Response> {
      return super.request(url, options);
  }

  get(url: string, options?: RequestOptionsArgs, hideLoading?: boolean): Observable<Response> {
      if (!hideLoading) this.showLoading(url);
      return super.get(url, this.getRequestOptionArgs(options))
      .pipe(tap( res => {
        if (!hideLoading) this.hideLoading(url);
      }, err => {
        if (!hideLoading) this.hideLoading(url);
        this.verifyResponseStatus(err);
      }));
  }

  patch(url: string, body: string, options?: RequestOptionsArgs, hideLoading?: boolean): Observable<Response> {
      if (!hideLoading) this.showLoading(url);
      return super.patch(url, body, this.getRequestOptionArgs(options))
      .pipe(tap( res => {
        if (!hideLoading) this.hideLoading(url);
      }, err => {
        if (!hideLoading) this.hideLoading(url);
        this.verifyResponseStatus(err);
      }));
  }

  post(url: string, body: string, options?: RequestOptionsArgs, hideLoading?: boolean): Observable<Response> {
      if (!hideLoading) this.showLoading(url);
      return super.post(url, body, this.getRequestOptionArgs(options))
      .pipe(tap( res => {
        if (!hideLoading) this.hideLoading(url);
      }, err => {
        if (!hideLoading) this.hideLoading(url);
        this.verifyResponseStatus(err);
      }));
  }

  put(url: string, body: string, options?: RequestOptionsArgs, hideLoading?: boolean): Observable<Response> {
      if (!hideLoading) this.showLoading(url);
      return super.put(url, body, this.getRequestOptionArgs(options))
      .pipe(tap( res => {
        if (!hideLoading) this.hideLoading(url);
      }, err => {
        if (!hideLoading) this.hideLoading(url);
        this.verifyResponseStatus(err);
      }));
  }

  delete(url: string, options?: RequestOptionsArgs, hideLoading?: boolean): Observable<Response> {
      if (!hideLoading) this.showLoading(url);
      return super.delete(url, this.getRequestOptionArgs(options))
      .pipe(tap( res => {
        if (!hideLoading) this.hideLoading(url);
      }, err => {
        if (!hideLoading) this.hideLoading(url);
        this.verifyResponseStatus(err);
      }));
  }

  private getRequestOptionArgs(options?: RequestOptionsArgs) : RequestOptionsArgs {
      if ( !options ) {
          options = new RequestOptions();
      }
      if ( !options.headers ) {
          options.headers = new Headers();
      }
      if ( !options.headers.get('Content-Type') ) {
          options.headers.append('Content-Type', 'application/json');
      }
      if ( this.auth.getToken() ) {
        options.headers.set('Authorization', `Bearer ${this.auth.getToken()}`);
      }

      return options;
  }

  private showLoading(url: string) {
      this._requestsInitiated++;
      if (!this._dialogRef) this._dialogRef = this.dialog.open(LoadingDialog, { disableClose: true });
  };

  private hideLoading(url: string){
      this._requestsInitiated--;
      if (this._requestsInitiated<=0){
        this._requestsInitiated = 0;
        if (this._dialogRef) this._dialogRef.close();
        this._dialogRef = null;
      }
  };

  private logout(): void {
      this.auth.deleteToken();
      localStorage.removeItem(lsKeyLogged);
      this.router.navigate(['pages/auth/login']);
  }

  private verifyResponseStatus(err: any) {
      if (!err.status) return;
      if (err.status === Constants.HTTP_STATUS_CODE.ClientError.Unauthorized
          || err.status === Constants.HTTP_STATUS_CODE.ClientError.Forbidden) {

        if (err.url && UrlsUnauthorized.some(url => err.url.includes(url))) {
          setTimeout(() => {
              this.router.navigate(['pages/sem-permissao']);
          }, 50);
        } else {
          this.logout();
        }
      }
  }

}
