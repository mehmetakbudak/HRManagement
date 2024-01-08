import { Component, OnInit, TemplateRef } from "@angular/core";
import { createStore } from "devextreme-aspnet-data-nojquery";
import CustomStore from "devextreme/data/custom_store";
import { Urls } from "src/app/models/consts";
import { environment } from "src/environments/environment";

@Component({
  selector: "app-users",
  templateUrl: "./users.component.html",
  styleUrls: ["./users.component.scss"],
})
export class UsersComponent implements OnInit {
  dataSource: CustomStore;

  constructor() {}

  ngOnInit() {
    this.bindGrid();
  }

  bindGrid() {
    this.dataSource = createStore({
      key: "id",

      loadUrl: `${environment.apiUrl}${Urls.User}`,
      onBeforeSend(method, ajaxOptions) {
        ajaxOptions.xhrFields = { withCredentials: true };
      },
    });
  }

  add() {}
}
