import React from 'react';
// @ts-ignore
import Upload from 'rc-upload';
import { Icon } from 'antd';

export interface IImageUploadProps {
  action: string;
  multiple: boolean;
  onstart: (file: any) => void;
  onSuccess: (ret: any) => void;
  onError: (err: any) => void;
  beforeUpload: (file: any, fileList: any) => void;
  imageUrl: string;
  imageLoading: boolean
}

const ImageUpload = (props: IImageUploadProps) => {
  return (
    <div>
      <div>
        <Upload {...props}>
          <a>
            {
              props.imageUrl ?
                <img src={props.imageUrl} alt="avatar"/> :
                <div>
                  <Icon type={props.imageLoading ? 'loading' : 'plus'}/>
                  <div className="ant-upload-text">Upload</div>
                </div>
            }
          </a>
        </Upload>
      </div>
    </div>
  );
};

export default ImageUpload;
