import { EntityDto } from '../dto/entityDto';
import { PagedResultDto } from '../dto/pagedResultDto';
import http from '../httpService';
import ProductDto from './dto/productDto';
import { PagedProductRequestDto } from './dto/pagedProductRequestDto';

class ProductService {
  public async create(createProductInput: ProductDto): Promise<PagedResultDto<ProductDto>> {
    let result = await http.post('api/services/app/Product/Create', createProductInput);
    return result.data.result;
  }

  public async update(updateProductInput: ProductDto): Promise<ProductDto> {
    let result = await http.put('api/services/app/Product/Update', updateProductInput);
    return result.data.result as ProductDto;
  }

  public async delete(entityDto: EntityDto) {
    let result = await http.delete('api/services/app/Product/Delete', { params: entityDto });
    return result.data;
  }

  public async get(entityDto: EntityDto) : Promise<ProductDto> {
    let result = await http.get('api/services/app/Product/Get', { params: entityDto });
    return result.data;
  }

  public async getAll(pagedFilterAndSortedRequest: PagedProductRequestDto): Promise<PagedResultDto<ProductDto>> {
    let result = await http.get('api/services/app/Product/GetAll', { params: pagedFilterAndSortedRequest });
    return result.data.result;
  }
}

export default new ProductService();
