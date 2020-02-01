import productDto from '../services/product/dto/productDto';

declare var abp: any;

class NotificationDistributor {
  Initialize() {
    // @ts-ignore
    abp.event.on('abp.notifications.received', (notification: abp.notifications.IUserNotification) => {
      if (notification.notification.notificationName === 'ProductData.New') {
        this.DistributeProductData(notification.notification.data.properties, 'productData.new');
      }
      if (notification.notification.notificationName === 'ProductData.Delete') {
        this.DistributeProductData(notification.notification.data.properties, 'productData.delete');
      }
    });
  }

  DistributeProductData(props: any, event: string) {
    let productData: productDto = {
      ...props,
    };

    abp.event.trigger(event, productData);
  }
}

export default new NotificationDistributor();