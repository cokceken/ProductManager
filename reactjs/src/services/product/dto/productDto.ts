import { EntityDto } from '../../../services/dto/entityDto';

export default class ProductDto extends EntityDto {
  code!: string;
  name!: string;
  photo!: string;
  price!: number;
  isDeleted!: boolean;
  deleterUserId!: number;
  deletionTime!: Date;
  lastModificationTime!: Date;
  lastModifierUserId!: number;
  creationTime!: Date;
  creatorUserId!: number;
}
