import { Component, OnInit } from '@angular/core';
import { AppService } from 'src/app/app.service';
import { MenuType } from 'src/app/models/enums';

@Component({
  selector: 'app-frontend',
  templateUrl: './frontend.component.html',
  styleUrls: ['./frontend.component.css']
})
export class FrontendComponent implements OnInit {
  items;

  constructor(private appService: AppService) { }

  ngOnInit() {
    this.appService.getApi(this.appService.feMenu + "/GetType/" + MenuType.FrontEnd)
      .then((res: any) => {
        this.items = res.items;
      });
  }
}
