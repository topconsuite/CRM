export class PagedList<T> {
    currentPage: number = 0;
    pageCount: number = 0;
    pageSize: number = 0;
    recordCount: number = 0;
    records: T[] = [];

    constructor(pageInfo?) {
        if (!pageInfo) return;
        if (pageInfo.currentPage) this.currentPage = pageInfo.currentPage;
        if (pageInfo.pageCount) this.pageCount = pageInfo.pageCount;
        if (pageInfo.pageSize) this.pageSize = pageInfo.pageSize;
        if (pageInfo.recordCount) this.recordCount = pageInfo.recordCount;
    }
}