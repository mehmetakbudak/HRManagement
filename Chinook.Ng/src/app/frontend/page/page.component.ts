import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from 'src/app/app.service';

@Component({
  selector: 'app-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.scss']
})
export class PageComponent implements OnInit {
  items;
  page;
  menuName;

  constructor(private router: ActivatedRoute, private appService: AppService) { }

  ngOnInit() {
    this.router.params.subscribe(params => {
      const url = params['url'];
      this.appService.getOdata(this.appService.fePage + "?$filter=url eq '" + url + "'")
        .then((res: any) => {
          const data = res.value[0];
          if (data) {
            this.page = data;
          }
          if (data.menuId) {
            this.appService.getApi(this.appService.feMenu + "/" + data.menuId)
              .then((resMenu: any) => {
                this.menuName = resMenu.label;
                this.items = resMenu.items;
              });
          }
        });
    });
  }
}
