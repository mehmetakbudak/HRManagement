import { Component, OnInit } from "@angular/core";
import { AlertService, defaultMessageType, alertType } from "../../services/alert.service";
import { ConfirmService } from "../../services/confirm.service";
import { AppService } from "../../app.service";
import notify from 'devextreme/ui/notify';

export class BlogCategory {
  deleted: boolean;
  isActive: boolean = true;
  id: number = 0;
  name: string;
  url: string;
}

@Component({
  selector: "app-blog-categories",
  templateUrl: "./blog-categories.component.html"
})
export class BlogCategoriesComponent implements OnInit {
  screenWidth = "40vw";
  blogCategories;
  isVisible = false;
  showErrorPopup = false;
  title;

  blogCategory: BlogCategory;

  constructor(
    private alertService: AlertService,
    private confirmService: ConfirmService,
    private appService: AppService
  ) { }

  ngOnInit() {
    this.blogCategory = new BlogCategory();
    this.bindGrid();
    if (screen.width < 768) {
      this.screenWidth = '90vw';
    }
  }

  bindGrid() {
    this.appService.getApi(this.appService.blogCategory)
      .then(res => {
        this.blogCategories = res;
      });
  }

  add() {
    this.blogCategory = new BlogCategory();
    this.blogCategory.id = 0;
    this.isVisible = true;
    this.title = "Yeni Blog Kategori Ekle";
  }

  edit(e: any) {
    this.isVisible = true;
    this.title = "Blog Kategori Düzenle";
    this.blogCategory = e.data;
  }

  delete(e) {
    this.confirmService.delete().then((res: boolean) => {
      if (res) {
        this.appService.delete(this.appService.blogCategory, e.data.id).then(() => {
          this.alertService.showDefaultMessage(defaultMessageType.delete, alertType.success)
          this.bindGrid();
        })
      }
    });
  }

  save(e) {
    const result = e.validationGroup.validate();
    if (result.isValid) {
      if (this.blogCategory.id) {
        this.appService.put(this.appService.blogCategory, this.blogCategory)
          .then(res => {
            this.isVisible = false;
            this.bindGrid();
            this.alertService.showDefaultMessage(defaultMessageType.update, alertType.success);
          });
      } else {
        this.blogCategory.id = 0;
        this.appService.post(this.appService.blogCategory, this.blogCategory)
          .then(res => {
            this.isVisible = false;
            this.bindGrid();
            this.alertService.showDefaultMessage(defaultMessageType.save, alertType.success);
          });
      }
    }
  }
}
