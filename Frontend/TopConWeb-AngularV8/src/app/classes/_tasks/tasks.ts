import { FormGroup } from '@angular/forms';

import { Endereco } from '../endereco/endereco';

export class Tasks {

    private static _browserInfo: BrowserInfo;

    static addDays(date: Date, days: number): Date {
        //date.setHours(0, 0, 0, 0);
        date.setDate(date.getDate() + days);
        return date;
    }

    static dataAtual(): Date {
        let date = new Date();
        date.setHours(0, 0, 0, 0);
        return date;
    }
    
    static ederecoToString(e: Endereco): string {
        if (!e) return '';
        if (!e.municipio) return e.logradouro+', '+e.numero+' '+e.complemento+' - '+e.bairro+' - CEP: '+e.cep;
        return e.logradouro+', '+e.numero+' '+e.complemento+' - '+e.bairro
                +' - '+e.municipio.nome+'/'+e.municipio.uf+' - CEP: '+e.cep;
    }

    static formataHora(hora : any) : string {
        return hora.toString().substring(0,2) + ':' + hora.toString().substring(2);
    }

    static formataData(data: Date): string {
        data = new Date(data);
        return data.getDate().toString().padStart(2,'0')
            +'/'+(data.getMonth()+1).toString().padStart(2,'0')
            +'/'+data.getFullYear();
    }

    static formataDataNullavel(data: Date): string {
        if (data === null) return '';
        data = new Date(data);
        return data.getDate().toString().padStart(2,'0')
            +'/'+(data.getMonth()+1).toString().padStart(2,'0')
            +'/'+data.getFullYear();
    }

    static formataDataHora(data: Date): string {
        data = new Date(data);
        return data.getDate().toString().padStart(2,'0')
            +'/'+(data.getMonth()+1).toString().padStart(2,'0')
            +'/'+data.getFullYear().toString().padStart(4,'0')
            +' '+data.getHours().toString().padStart(2,'0')
            +':'+data.getMinutes().toString().padStart(2,'0')
            +':'+data.getSeconds().toString().padStart(2,'0');
    }

    static formataTelefone(ddd: number, numero: number): string {
        let strNumero = numero.toString();
        if (strNumero.length === 9) {
            strNumero = strNumero.substring(0, 5) + '-' + strNumero.substring(5);
        } else if (strNumero.length >= 5) {
            strNumero = strNumero.substring(0, 4) + '-' + strNumero.substring(4);
        }
        let strDdd = (ddd===0 ? '' : '(' + ddd + ') ');
        return strDdd + strNumero;
    }

    static formataCpfCnpj(cpfCnpj): string {
        if (cpfCnpj.length <= 11)
            return cpfCnpj.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/g,"\$1.\$2.\$3\-\$4");
        else
            return cpfCnpj.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/g,"\$1.\$2.\$3\/\$4\-\$5");
    }

    static formataValor(valor: number, decimalDigits: number, thousandSeparator: boolean): string {
        if (isNaN(valor)) 
            valor = 0;
        let strValor = (valor || 0).toFixed(decimalDigits);
        let strValorResult = strValor.replace('.',',');
        if (thousandSeparator) {
            for (var i = (strValor.replace('-','').length - (decimalDigits + 2)); i > 0; i--) {
                let j = (strValor.replace('-','').length - (decimalDigits + 1)) - i;
                if (j%3 ===0 && j>1) {
                    if (strValor.startsWith('-')) {
                        strValorResult = strValorResult.substring(0, i+1)+'.'+strValorResult.substring(i+1);
                    } else {
                        strValorResult = strValorResult.substring(0, i)+'.'+strValorResult.substring(i);
                    }
                }
            }
        }
        return strValorResult;
    }
    static formataMoeda(valor: number): string {
        return 'R$' + Tasks.formataValor(valor, 2 ,true);
    }

    static horarioValido(horario: string): boolean {
        var h = Tasks.horarioPropriedades(horario);
        return (h && h.horas >= 0 && h.horas < 24 && h.minutos >= 0 && h.minutos < 60);
    }
    static horarioIntervaloValido(horario: string, intervalo: number): boolean {
        var h = Tasks.horarioPropriedades(horario);
        if (!h) return false;
        var mod = (intervalo ? h.totalMinutos % intervalo : 0);
        return (mod === 0);
    }
    private static horarioPropriedades(horario: string) {
        var _horario = (horario || '').replace(':', '');
        if (_horario.length < 4) return null;
        var h = parseInt(_horario.substring(0, _horario.length - 2));
        var m = parseInt(_horario.substring(_horario.length - 2, _horario.length));
        var mTotal = (h * 60) + m;

        return {
            horas: h,
            minutos: m,
            totalMinutos: mTotal
        }
    }
    static horarioMinutos(horario: string): number {
        var _horario = (horario || '').replace(':', '');
        if (_horario.length < 4) return 0;
        return parseInt(_horario.substring(_horario.length - 2, _horario.length));
    }

    static convertMinutosParaHora(minutos: number): string {
        const horas = Math.floor(minutos/ 60);          
        const min = minutos % 60;
        const textoHoras = (`00${horas}`).slice(-2);
        const textoMinutos = (`00${min}`).slice(-2);
        
        return `${textoHoras }:${textoMinutos}`;
    }

    static setValidationErrors(errorMessagesFrom: {key: string, message: string}[],
        errorMessagesTo, fieldName: string, formGroup: FormGroup) {
        
        let errors = formGroup.controls[fieldName].errors || {};

        errorMessagesTo[fieldName] = errorMessagesFrom.filter(t => t.key.startsWith(fieldName));
        
        errorMessagesTo[fieldName].forEach(t => {
            errors[t.key] = t.message;
            formGroup.controls[fieldName].setErrors(errors);
            formGroup.controls[fieldName].markAsTouched();
        });

    }

    static formataErrosApi(error: any): string {
        var errors = error['errors'];
        if (errors)     
            return errors.map(t => t.message ? t.message : t).filter(Tasks.onlyUniqueElementsFilter).join('\n');
        else
            return 'OCORREU UM ERRO ' + errors;
    }

    static replaceAll(source: string, strFind: string, strReplace: string) : string {
        return source.split(strFind).join(strReplace);
    }

    static onlyUniqueElementsFilter(value, index, self) { 
        return self.indexOf(value) === index;
    }

    static get browserInfo(): BrowserInfo {
        if (Tasks._browserInfo)
            return Tasks._browserInfo;

        var navUserAgent = navigator.userAgent;
        var browserName = navigator.appName;
        var browserVersion = ''+parseFloat(navigator.appVersion); 
        var majorVersion = parseInt(navigator.appVersion,10);
        var tempNameOffset, tempVersionOffset, tempVersion;

        if ((tempVersionOffset=navUserAgent.indexOf("Opera"))!=-1) {
            browserName = "Opera";
            browserVersion = navUserAgent.substring(tempVersionOffset+6);
            if ((tempVersionOffset=navUserAgent.indexOf("Version"))!=-1) 
                browserVersion = navUserAgent.substring(tempVersionOffset+8);
        } else if ((tempVersionOffset=navUserAgent.indexOf("MSIE"))!=-1) {
            browserName = "Microsoft Internet Explorer";
            browserVersion = navUserAgent.substring(tempVersionOffset+5);
        } else if ((tempVersionOffset=navUserAgent.indexOf("Chrome"))!=-1) {
            browserName = "Chrome";
            browserVersion = navUserAgent.substring(tempVersionOffset+7);
        } else if ((tempVersionOffset=navUserAgent.indexOf("Safari"))!=-1) {
            browserName = "Safari";
            browserVersion = navUserAgent.substring(tempVersionOffset+7);
            if ((tempVersionOffset=navUserAgent.indexOf("Version"))!=-1) 
                browserVersion = navUserAgent.substring(tempVersionOffset+8);
        } else if ((tempVersionOffset=navUserAgent.indexOf("Firefox"))!=-1) {
            browserName = "Firefox";
            browserVersion = navUserAgent.substring(tempVersionOffset+8);
        } else if ( (tempNameOffset=navUserAgent.lastIndexOf(' ')+1) < (tempVersionOffset=navUserAgent.lastIndexOf('/')) ) {
            browserName = navUserAgent.substring(tempNameOffset,tempVersionOffset);
            browserVersion = navUserAgent.substring(tempVersionOffset+1);
            if (browserName.toLowerCase()==browserName.toUpperCase()) {
                browserName = navigator.appName;
            }
        }

        // trim version
        if ((tempVersion=browserVersion.indexOf(";"))!=-1)
            browserVersion=browserVersion.substring(0,tempVersion);
        if ((tempVersion=browserVersion.indexOf(" "))!=-1)
            browserVersion=browserVersion.substring(0,tempVersion);
        
            Tasks._browserInfo = { name: browserName, version: browserVersion };

        return Tasks._browserInfo;
    }

    static openPdf(url: string) {
        var target = "_blank";

        if (Tasks.browserInfo.name.toUpperCase() === "MOBILE")
            target = "_self";
        return window.open(url, target, "location=no,menubar=no,titlebar=no;toolbar=no");
    }

    static openBase64File(base64: string, fileName: string, mimeType: string){
        const byteCharacters = atob(base64);
        const byteNumbers = new Array(byteCharacters.length);
        for (let i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        const byteArray = new Uint8Array(byteNumbers);
        const blob = new Blob([byteArray], { type: mimeType });
        const blobUrl = URL.createObjectURL(blob);
        const newTab = window.open(blobUrl, '_blank');
    
        if (newTab) {
            const link = newTab.document.createElement('a');
            link.href = blobUrl;
            link.click();
    
            setTimeout(() => {
                URL.revokeObjectURL(blobUrl);
            }, 100);
        } else {
            console.error('Failed to open new tab.');
        }
    }
}

export class BrowserInfo {
    name: string = '';
    version: string= '';
}