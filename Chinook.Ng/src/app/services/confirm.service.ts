import { Injectable } from '@angular/core';
import { custom } from 'devextreme/ui/dialog';

@Injectable({
  providedIn: 'root'
})
export class ConfirmService {

  constructor() { }

  delete(title?, content?) {
    title = title ? title : "Silme Onayı";
    content = content ? content : "Silmek istediğinize emin misiniz?";

    let dialog = custom({
      title: title,
      messageHtml: content,
      buttons: [{
        text: "Vazgeç",
        onClick: () => { return false; }
      }, {
        text: "Sil",
        type: "danger",
        onClick: () => { return true; }
      }]
    });
    return dialog.show();
  }
}
