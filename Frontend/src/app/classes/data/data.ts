export class Data {
    year: number;
    month: number;
    day: number;

    constructor(_date?: Date) {
        let date = _date || new Date();

        this.year = date.getFullYear();
        this.month = date.getMonth() + 1;
        this.day = date.getDate();
    }
}