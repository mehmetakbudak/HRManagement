import { Component, OnInit, TemplateRef } from "@angular/core";
import { Urls } from "src/app/models/consts";
import { environment } from "src/environments/environment";

@Component({
  selector: "app-users",
  templateUrl: "./users.component.html",
  styleUrls: ["./users.component.scss"],
})
export class UsersComponent implements OnInit {
  dataSource = [];

  constructor() {}

  ngOnInit() {
    this.bindGrid();
  }

  bindGrid() {

  }

  add() {}
}
