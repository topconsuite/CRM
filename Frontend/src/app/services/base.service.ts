import { Injectable, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { Response } from '@angular/http';

import { ApiBaseUrlService } from './api-base-url.service'
import { InterceptedHttpService } from './auth/intercepted-http.service'
import { Constants } from './../app-settings/constants'

import { PagedList } from '../classes/pagination/paged-list';
import { VersionService } from './version.service';
import { MatDialog } from '@angular/material';
import { AlertDialogComponent } from 'app/main/components/dialog/alert-dialog/alert-dialog.component';
import { Tasks } from 'app/classes/_tasks/tasks';

class Deferred<T> {
  promise: Promise<T>;
  resolve: (value?: T | PromiseLike<T>) => void;
  reject:  (reason?: any) => void;

  constructor() {
    this.promise = new Promise<T>((resolve, reject) => {
      this.resolve = resolve;
      this.reject  = reject;
    });
  }
}

@Injectable()
export class BaseService {

    protected http: InterceptedHttpService;
    protected router: Router;
    protected apiBaseUrlService: ApiBaseUrlService;
    protected versionService: VersionService;
    protected dialog: MatDialog;

    protected get constants() {
        return Constants;
    }
    
    constructor(
        private _injector: Injector
    ) {
        this.http = _injector.get(InterceptedHttpService);
        this.router = _injector.get(Router);
        this.apiBaseUrlService = _injector.get(ApiBaseUrlService);
        this.versionService = _injector.get(VersionService);
        this.dialog = _injector.get(MatDialog);
    }

    protected createDeferred<T>(): Deferred<T> {
        return new Deferred<T>();
    }

    protected getResponseData(response: Response): any {
        return response['_body'] ? JSON.parse(response['_body']) : response;
    }
    
    protected getErrorData(error: any): any {
        console.error(error);
        return error['_body'] && typeof error['_body'] === "string" ? JSON.parse(error['_body']) : error;
    }

    protected getPagedResponseData<T>(response: Response): PagedList<T> {
        var pagedList = new PagedList<T>(JSON.parse(response.headers.get('page-info')));
        let data: T[] = this.getResponseData(response);
        pagedList.records = data;
        return pagedList;
    }

    protected getApiDateVersion(response: Response): Date {
        var apiDateVersion = response.headers.get('api-date-version');
        return new Date(apiDateVersion);
    }

    protected validaVersoes(versaoApiFrontEnd: Date, versaoApiBackEnd: Date) {
        if ((versaoApiBackEnd > versaoApiFrontEnd)) {
            if (this.dialog.openDialogs.length > 1) return;
            const isMobile = /Android/i.test(navigator.userAgent) || Tasks.browserInfo.name.toUpperCase() === "MOBILE";
            
            this.dialog.open(AlertDialogComponent, {
                disableClose: true,
                data: {
                 title: 'TopConWeb - Versão',
                 message: 'Você está com uma versão desatualizada do TopConWeb, o sistema será recarregado!',
                 afterCloseCallback: async () => {
                    this.dialog.closeAll();
                    if (isMobile) {
                     this.router.navigateByUrl("pages/refreshapp").then(
                        reload => {
                            window.location.reload(true);
                        }
                     );
                    }
                    window.location.reload(true);
                 }
                }
              });
        }
    }

    protected makeGetPrommisse<TResult>(methodUrl: string, hideLoading?: boolean): Promise<TResult> {
        let deferred = this.createDeferred<TResult>();
        this.http.get(this.apiBaseUrlService.getUrl() + methodUrl, undefined, hideLoading)
            .subscribe(response => {
                this.validaVersoes(this.versionService.apiDateVersion, this.getApiDateVersion(response));
                let data: TResult = this.getResponseData(response);
                if (response['status'] === this.constants.HTTP_STATUS_CODE.Success.OK){
                    deferred.resolve(data);
                }else{
                    deferred.reject(data);
                }
            }, error => {
                let data = this.getErrorData(error);
                deferred.reject(data);
            });

        return deferred.promise;
    }

    protected makePagedGetPrommisse<TResult>(methodUrl: string, hideLoading?: boolean): Promise<PagedList<TResult>> {
        let deferred = this.createDeferred<PagedList<TResult>>();

        this.http.get(this.apiBaseUrlService.getUrl()+methodUrl, undefined, hideLoading)
            .subscribe(response => {
                this.validaVersoes(this.versionService.apiDateVersion, this.getApiDateVersion(response));
                let data: PagedList<TResult> = this.getPagedResponseData(response);
                if (response['status'] === this.constants.HTTP_STATUS_CODE.Success.OK){
                    deferred.resolve(data);
                }else{
                    deferred.reject(data);
                }
            }, error => {
                let data = this.getErrorData(error);
                deferred.reject(data);
            });

        return deferred.promise;
    }

    protected makePostPrommisse<TResult>(methodUrl: string, body: string, hideLoading?: boolean): Promise<TResult> {
        let deferred = this.createDeferred<TResult>();

        this.http.post(this.apiBaseUrlService.getUrl()+methodUrl, body, undefined, hideLoading)
            .subscribe(response => {
                this.validaVersoes(this.versionService.apiDateVersion, this.getApiDateVersion(response));
                let data: TResult = this.getResponseData(response);
                if (response['status'] === this.constants.HTTP_STATUS_CODE.Success.OK){
                    deferred.resolve(data);
                }else{
                    deferred.reject(data);
                }
            }, error => {
                let data = this.getErrorData(error);
                deferred.reject(data);
            });

        return deferred.promise;
    }

    protected makePatchPrommisse<TResult>(methodUrl: string, body: string, hideLoading?: boolean): Promise<TResult> {
        let deferred = this.createDeferred<TResult>();

        this.http.patch(this.apiBaseUrlService.getUrl()+methodUrl, body, undefined, hideLoading)
            .subscribe(response => {
                this.validaVersoes(this.versionService.apiDateVersion, this.getApiDateVersion(response));
                let data: TResult = this.getResponseData(response);
                if (response['status'] === this.constants.HTTP_STATUS_CODE.Success.OK){
                    deferred.resolve(data);
                }else{
                    deferred.reject(data);
                }
            }, error => {
                let data = this.getErrorData(error);
                deferred.reject(data);
            });

        return deferred.promise;
    }

    protected makePutPrommisse<TResult>(methodUrl: string, body: string, hideLoading?: boolean): Promise<TResult> {
        let deferred = this.createDeferred<TResult>();

        this.http.put(this.apiBaseUrlService.getUrl()+methodUrl, body, undefined, hideLoading)
            .subscribe(response => {
                this.validaVersoes(this.versionService.apiDateVersion, this.getApiDateVersion(response));
                let data: TResult = this.getResponseData(response);
                if (response['status'] === this.constants.HTTP_STATUS_CODE.Success.OK){
                    deferred.resolve(data);
                }else{
                    deferred.reject(data);
                }
            }, error => {
                let data = this.getErrorData(error);
                deferred.reject(data);
            });

        return deferred.promise;
    }

    protected makeDeletePrommisse<TResult>(methodUrl: string, hideLoading?: boolean): Promise<TResult> {
        let deferred = this.createDeferred<TResult>();

        this.http.delete(this.apiBaseUrlService.getUrl()+methodUrl, undefined, hideLoading)
            .subscribe(response => {
                this.validaVersoes(this.versionService.apiDateVersion, this.getApiDateVersion(response));
                let data: TResult = this.getResponseData(response);
                if (response['status'] === this.constants.HTTP_STATUS_CODE.Success.OK){
                    deferred.resolve(data);
                }else{
                    deferred.reject(data);
                }
            }, error => {
                let data = this.getErrorData(error);
                deferred.reject(data);
            });

        return deferred.promise;
    }

}