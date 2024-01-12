import { Component, OnInit } from "@angular/core";
import { AppService } from "src/app/app.service";
import { Urls } from "src/app/models/consts";

@Component({
  selector: "app-frontend",
  templateUrl: "./frontend.component.html",
  styleUrls: ["./frontend.component.css"],
})
export class FrontendComponent implements OnInit {
  items;

  constructor(private appService: AppService) {}

  ngOnInit() {
    this.appService
      .get(`${Urls.Menu}/GetFrontendMenu`)
      .then((res: any) => {
        this.items = res;
      });
  }
}
