import { Injectable, Injector, Version } from '@angular/core';

@Injectable()
export class VersionService {

    private _version: Version = new Version('2.1.2026-05-21/01');

    get major() {
        return this._version.major;
    }
    get minor() {
        return this._version.minor;
    }
    get patch() {
        return this._version.patch;
    }
    get full() {
        return this._version.full;
    }
    get apiDateVersion() : Date {
        return new Date(this.patch.substring(0,10));
    }

}