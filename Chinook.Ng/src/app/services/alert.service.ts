import { Injectable } from '@angular/core';
import notify from 'devextreme/ui/notify';

@Injectable({
  providedIn: 'root'
})
export class AlertService {

  constructor() { }

  showMessage(message, type: alertType) {
    notify(message, alertType[type], 1000);
  }

  showDefaultMessage(messageType: defaultMessageType, type: alertType) {
    notify(this.getDefaultMessage(messageType), alertType[type], 1000);
  }

  private getAlertType(type: alertType) {
    switch (type) {
      case alertType.success:
        return "success";
      case alertType.info:
        return "info";
      case alertType.error:
        return "error";
      case alertType.warning:
        return "warn";
    }
  }

  private getHeader(type: alertType): string {
    switch (type) {
      case alertType.error:
        return "İşlem Başarısız";
      case alertType.info:
        return "Bilgilendirme";
      case alertType.success:
        return "İşlem Başarılı";
      case alertType.warning:
        return "Dikkat";
    }
  }

  private getDefaultMessage(type: defaultMessageType): string {
    switch (type) {
      case defaultMessageType.save:
        return 'Kaydetme işlemi başarıyla gerçekleşti.';
      case defaultMessageType.update:
        return 'Güncelleme işlemi başarıyla gerçekleşti.';
      case defaultMessageType.delete:
        return 'Silme işlemi başarıyla gerçekleşti.';
      case defaultMessageType.error:
        return 'Beklenmeyen bir hata oluştu.';
    }
  }
}

export enum alertType {
  success,
  info,
  error,
  warning
}

export enum defaultMessageType {
  save,
  update,
  delete,
  error
}
