export interface SearchCriteria {
  pageNumber: number;
  pageSize?: number;
  orderBy?: string;
  isAscending?: boolean;
  searchBy?: string;
  searchValue?: string;
}
