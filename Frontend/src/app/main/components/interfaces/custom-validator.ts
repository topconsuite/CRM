export interface ICustomValidator {
    key: string;
    message: string;
    validatorFunction: (...params) => boolean;
    params?: any[];
}