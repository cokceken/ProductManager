import { L } from '../../../lib/abpUtility';

const rules = {
  code: [{ required: true, message: L('ThisFieldIsRequired') }],
  name: [{ required: true, message: L('ThisFieldIsRequired') }],
  photo: [{ required: false, message: L('ThisFieldIsRequired') }],
  price: [{ required: true, message: L('ThisFieldIsRequired') }],
};

export default rules;
