import * as React from 'react';

import { Form, Input, InputNumber, Modal, Tabs } from 'antd';

import { FormComponentProps } from 'antd/lib/form';
import FormItem from 'antd/lib/form/FormItem';
import { L } from '../../../lib/abpUtility';
import rules from './createOrUpdateProduct.validation';
import ImageUpload from './imageUpload';

const TabPane = Tabs.TabPane;

export interface ICreateOrUpdateProductProps extends FormComponentProps {
  visible: boolean;
  onCancel: () => void;
  modalType: string;
  onCreate: () => void;
  priceConfirm: (callback: any) => void;
  productPhotoChanged: (value:string) => void;
  productPhotoUrl: string;
}

class CreateOrUpdateProduct extends React.Component<ICreateOrUpdateProductProps> {
  state = {
    priceValidateLimit: 999,
    imageLoading: false,
  };

  onImageUploadSuccess = (ret: any) => {
    let imageUrl = process.env.REACT_APP_REMOTE_SERVICE_BASE_URL + '/uploads/' + ret.result.imageId;
    this.setState({ imageLoading: false });
    this.props.productPhotoChanged(imageUrl);
    this.props.form.setFieldsValue({ photo: imageUrl });
  };

  onImageUploadStart = (file: any) => {
    this.setState({ imageLoading: true });
  };

  onImageUploadError = (err: any) => {
    this.setState({ imageLoading: false });
  };

  beforeImageUpload = (file: any, fileList: any) => {
    return new Promise((resolve) => {
      resolve(file);
    });
  };

  validatePrice = (rule: any, value: any, callback: any) => {
    if (!value || value < this.state.priceValidateLimit) callback();
    else this.props.priceConfirm(callback);
  };

  render() {
    const formItemLayout = {
      labelCol: {
        xs: { span: 6 },
        sm: { span: 6 },
        md: { span: 6 },
        lg: { span: 6 },
        xl: { span: 6 },
        xxl: { span: 6 },
      },
      wrapperCol: {
        xs: { span: 18 },
        sm: { span: 18 },
        md: { span: 18 },
        lg: { span: 18 },
        xl: { span: 18 },
        xxl: { span: 18 },
      },
    };

    const { getFieldDecorator } = this.props.form;
    const { visible, onCancel, onCreate, productPhotoUrl } = this.props;

    return (
      <Modal visible={visible} cancelText={L('Cancel')} okText={L('OK')} onCancel={onCancel} onOk={onCreate}
             title={'Product'}>
        <Tabs defaultActiveKey={'productInfo'} size={'small'} tabBarGutter={64}>
          <TabPane tab={'Product'} key={'Product'}>
            <FormItem label={L('Code')} {...formItemLayout}>
              {getFieldDecorator('code', { rules: rules.code, initialValue: '' })(<Input/>)}
            </FormItem>
            <FormItem label={L('Name')} {...formItemLayout}>
              {getFieldDecorator('name', { rules: rules.name, initialValue: '' })(<Input/>)}
            </FormItem>
            <div className="ant-row ant-form-item">
              <div
                className="ant-col ant-form-item-label ant-col-xs-6 ant-col-sm-6 ant-col-md-6 ant-col-lg-6 ant-col-xl-6 ant-col-xxl-6">
                <label htmlFor="photo" title={L('Photo')}>{L('Photo')}</label>
              </div>
              <div
                className="image-upload ant-col ant-form-item-control-wrapper ant-col-xs-18 ant-col-sm-18 ant-col-md-18 ant-col-lg-18 ant-col-xl-18 ant-col-xxl-18">
                <ImageUpload
                  multiple={false}
                  onstart={this.onImageUploadStart}
                  beforeUpload={this.beforeImageUpload}
                  onError={this.onImageUploadError}
                  onSuccess={this.onImageUploadSuccess}
                  action={process.env.REACT_APP_REMOTE_SERVICE_BASE_URL + '/api/Image/CreateImage'}
                  imageUrl={productPhotoUrl}
                  imageLoading={this.state.imageLoading}
                />
              </div>
            </div>
            <FormItem label={L('Photo')} {...formItemLayout} style={{ display: 'none' }}>
              {getFieldDecorator('photo', { rules: rules.photo, initialValue: '' })(<Input />)}
            </FormItem>
            <FormItem label={L('Price')} {...formItemLayout}>
              {getFieldDecorator('price', {
                rules: [
                  {
                    required: true,
                    message: L('ThisFieldIsRequired'),
                  },
                  {
                    validator: this.validatePrice,
                  },
                ], initialValue: 0, validateTrigger: 'onsubmit',
              })(<InputNumber/>)}
            </FormItem>
          </TabPane>
        </Tabs>
      </Modal>
    );
  }
}

export default Form.create<ICreateOrUpdateProductProps>()(CreateOrUpdateProduct);
