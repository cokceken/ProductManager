import { EntityDto } from '../../../services/dto/entityDto';

export default class ProductDto extends EntityDto {
  code!: string;
  name!: string;
  photo!: string;
  price!: number;
}
