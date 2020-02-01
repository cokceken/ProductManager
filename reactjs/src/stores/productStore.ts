import { action, observable } from 'mobx';

import { EntityDto } from '../services/dto/entityDto';
import { PagedResultDto } from '../services/dto/pagedResultDto';
import ProductDto from '../services/product/dto/productDto';
import productService from '../services/product/productService';
import productDto from '../services/product/dto/productDto';
import { PagedProductRequestDto } from '../services/product/dto/pagedProductRequestDto';
import ProductEditModel from '../models/Products/productEditModel';

class ProductStore {
  @observable products!: PagedResultDto<ProductDto>;
  @observable editModel: ProductEditModel = new ProductEditModel();
  @observable counter: number = 0;

  @action
  async create(createInput: productDto) {
    await productService.create(createInput);
  }

  @action
  async resetEditModel() {
    this.editModel = {
      product: {
        code: '',
        name: '',
        photo: '',
        price: 0,
        id: 0,
      },
    };
  }

  @action
  async update(updateInput: ProductDto) {
    await productService.update(updateInput);
    this.products.items
      .filter(x => x.id === updateInput.id)
      .map(x => {
        return x = updateInput;
      });
  }

  @action
  async delete(entityDto: EntityDto) {
    await productService.delete(entityDto);
    this.products.items = this.products.items.filter(x => x.id !== entityDto.id);
  }

  @action
  async get(entityDto: EntityDto) {
    let result = await productService.get(entityDto);
    this.resetEditModel();
    this.editModel.product = result;
  }

  @action
  async getAll(pagedFilterAndSortedRequest: PagedProductRequestDto) {
    let result = await productService.getAll(pagedFilterAndSortedRequest);
    this.products = result;
  }

  async getAllAsExcel() {
    return await productService.getAllAsExcel();
  }

  @action
  async putFromEvent(data: ProductDto) {
    console.log(data);
    if (this.products.items.some(x => x.id === data.id)) {
      this.products.items.forEach((value, index) => {
        if (value.id === data.id) this.products.items[index] = data;
        console.log('some');
      });
    } else {
      this.products.items.push(data);
    }
    this.counter = this.counter + 1;
  }

  @action
  async deleteFromEvent(data: ProductDto) {
    console.log(data);
    if (this.products.items.some(x => x.id === data.id)) {
      this.products.items.forEach((value, index) => {
        if (value.id === data.id) this.products.items.splice(index, 1);
        console.log('some');
      });
    }

    this.counter = this.counter + 1;
  }
}

export default ProductStore;
