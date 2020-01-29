import { PagedFilterAndSortedRequest } from '../../dto/pagedFilterAndSortedRequest';

export interface PagedProductRequestDto extends PagedFilterAndSortedRequest  {
    keyword: string
}
