import * as React from 'react';

import { Button, Card, Col, Dropdown, Input, Menu, Modal, Row, Table } from 'antd';
import { inject, observer } from 'mobx-react';

import AppComponentBase from '../../components/AppComponentBase';
import { EntityDto } from '../../services/dto/entityDto';
import { L } from '../../lib/abpUtility';
import Stores from '../../stores/storeIdentifier';
import './index.less';
import ProductStore from '../../stores/productStore';
import CreateOrUpdateProduct from './components/createOrUpdateProduct';
import { saveAs } from 'file-saver';

export interface IProductProps {
  productStore: ProductStore
}

export interface IProductState {
  modalVisible: boolean;
  maxResultCount: number;
  skipCount: number;
  productId: number;
  productPhotoUrl: string;
  filter: string;
}

const confirm = Modal.confirm;
const Search = Input.Search;

@inject(Stores.ProductStore)
@observer
class Product extends AppComponentBase<IProductProps, IProductState> {
  formRef: any;

  state = {
    modalVisible: false,
    maxResultCount: 10,
    skipCount: 0,
    productId: 0,
    filter: '',
    productPhotoUrl: '',
  };

  async componentDidMount() {
    await this.getAll();
  }

  async getAll() {
    await this.props.productStore.getAll({
      maxResultCount: this.state.maxResultCount,
      skipCount: this.state.skipCount,
      keyword: this.state.filter,
    });
  }

  handleTableChange = (pagination: any) => {
    this.setState({
      skipCount: (pagination.current - 1) * this.state.maxResultCount!,
    }, async () => await this.getAll());
  };

  Modal = () => {
    this.setState({
      modalVisible: !this.state.modalVisible,
    });
  };

  async createOrUpdateModalOpen(entityDto: EntityDto) {
    if (entityDto.id === 0) {
      await this.props.productStore.resetEditModel();
    } else {
      await this.props.productStore.get(entityDto);
    }

    await this.setState({ productId: entityDto.id, productPhotoUrl: this.props.productStore.editModel.product.photo });
    this.Modal();

    this.formRef.props.form.setFieldsValue({ ...this.props.productStore.editModel.product });
  }

  delete(input: EntityDto) {
    const self = this;
    confirm({
      title: 'Do you Want to delete these items?',
      onOk() {
        self.props.productStore.delete(input);
      },
      onCancel() {
        console.log('Cancel');
      },
    });
  }

  handleCreate = () => {
    const form = this.formRef.props.form;

    form.validateFields(async (err: any, values: any) => {
      if (err) {
        return;
      } else {
        if (this.state.productId === 0) {
          await this.props.productStore.create(values);
        } else {
          await this.props.productStore.update({ id: this.state.productId, ...values });
        }
      }

      await this.getAll();
      this.setState({ modalVisible: false });
      form.resetFields();
    });
  };

  saveFormRef = (formRef: any) => {
    this.formRef = formRef;
  };

  handleSearch = (value: string) => {
    this.setState({ filter: value }, async () => await this.getAll());
  };

  handleExcelSearch = () => {
    this.props.productStore.getAllAsExcel().then(x => {
      let filename = 'result.xlsx';
      let url = window.URL
        .createObjectURL(new Blob([x.data]));
      saveAs(url, filename);
    });
  };

  public render() {
    const { products } = this.props.productStore;
    const columns = [
      {
        title: L('ProductCode'),
        dataIndex: 'code',
        key: 'code',
        width: 150,
        render: (text: string) => <div>{text}</div>,
      },
      {
        title: L('ProductName'),
        dataIndex: 'name',
        key: 'name',
        width: 150,
        render: (text: string) => <div>{text}</div>,
      },
      {
        title: L('ProductPhoto'),
        dataIndex: 'photo',
        key: 'photo',
        width: 150,
        render: (text: string) => <img src={text} style={{ height: '100%' }}/>,
      },
      {
        title: L('ProductPrice'),
        dataIndex: 'price',
        key: 'price',
        width: 150,
        render: (text: number) => <div>{text}</div>,
      },
      {
        title: L('Actions'),
        width: 150,
        render: (text: string, item: any) => (
          <div>
            <Dropdown
              trigger={['click']}
              overlay={
                <Menu>
                  <Menu.Item onClick={() => this.createOrUpdateModalOpen({ id: item.id })}>{L('Edit')}</Menu.Item>
                  <Menu.Item onClick={() => this.delete({ id: item.id })}>{L('Delete')}</Menu.Item>
                </Menu>
              }
              placement="bottomLeft"
            >
              <Button type="primary" icon="setting">
                {L('Actions')}
              </Button>
            </Dropdown>
          </div>
        ),
      },
    ];

    return (
      <Card>
        <Row>
          <Col
            xs={{ span: 4, offset: 0 }}
            sm={{ span: 4, offset: 0 }}
            md={{ span: 4, offset: 0 }}
            lg={{ span: 2, offset: 0 }}
            xl={{ span: 2, offset: 0 }}
            xxl={{ span: 2, offset: 0 }}
          >
            {' '}
            <h2>{L('Products')}</h2>
          </Col>
          <Col
            xs={{ span: 14, offset: 0 }}
            sm={{ span: 15, offset: 0 }}
            md={{ span: 15, offset: 0 }}
            lg={{ span: 1, offset: 21 }}
            xl={{ span: 1, offset: 21 }}
            xxl={{ span: 1, offset: 21 }}
          >
            <Button type="primary" shape="circle" icon="plus" onClick={() => this.createOrUpdateModalOpen({ id: 0 })}/>
            <Button type="danger" shape="circle-outline" icon="file-excel" onClick={() => this.handleExcelSearch()}/>
          </Col>
        </Row>

        <Row>
          <Col sm={{ span: 10, offset: 0 }}>
            <Search placeholder={this.L('Filter')} onSearch={this.handleSearch}/>
          </Col>
        </Row>
        <Row style={{ marginTop: 20 }}>
          <Col
            xs={{ span: 24, offset: 0 }}
            sm={{ span: 24, offset: 0 }}
            md={{ span: 24, offset: 0 }}
            lg={{ span: 24, offset: 0 }}
            xl={{ span: 24, offset: 0 }}
            xxl={{ span: 24, offset: 0 }}
          >
            <Table
              rowKey={record => record.id.toString()}
              size={'default'}
              bordered={true}
              columns={columns}
              pagination={{ pageSize: 10, total: products === undefined ? 0 : products.totalCount, defaultCurrent: 1 }}
              loading={products === undefined}
              dataSource={products === undefined ? [] : products.items}
              onChange={this.handleTableChange}
            />
          </Col>
        </Row>
        <CreateOrUpdateProduct
          wrappedComponentRef={this.saveFormRef}
          visible={this.state.modalVisible}
          onCancel={() =>
            this.setState({
              modalVisible: false,
            })
          }
          modalType={this.state.productId === 0 ? 'edit' : 'create'}
          onCreate={this.handleCreate}
          productPhotoUrl={this.state.productPhotoUrl}
          priceConfirm={(callback: any) => {
            confirm({
              title: L('PriceConfirmLabel'),
              onOk() {
                callback();
              },
              onCancel() {
                return;
              },
            });
          }}
          productPhotoChanged={value => this.setState({ productPhotoUrl: value })}
        />
      </Card>
    );
  }
}

export default Product;
