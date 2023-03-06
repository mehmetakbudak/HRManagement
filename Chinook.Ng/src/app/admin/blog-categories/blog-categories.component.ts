import { Component, OnInit } from "@angular/core";
import {
  AlertService,
  defaultMessageType,
  alertType,
} from "../../services/alert.service";
import { ConfirmService } from "../../services/confirm.service";
import { AppService } from "../../app.service";
import { Urls } from "src/app/models/consts";

export class BlogCategory {
  deleted: boolean;
  isActive: boolean = true;
  id: number = 0;
  name: string;
  url: string;
}

@Component({
  selector: "app-blog-categories",
  templateUrl: "./blog-categories.component.html",
})
export class BlogCategoriesComponent implements OnInit {
  screenWidth = "40vw";
  blogCategories;
  isVisible = false;
  showErrorPopup = false;
  title;
  settings = [];
  blogCategory: BlogCategory;

  constructor(
    private alertService: AlertService,
    private confirmService: ConfirmService,
    private appService: AppService
  ) {}

  ngOnInit() {
    this.blogCategory = new BlogCategory();
    this.settings = this.appService.settingEditDelete();
    this.bindGrid();
    if (screen.width < 768) {
      this.screenWidth = "90vw";
    }
  }

  bindGrid() {
    this.appService.getApi(Urls.BlogCategory).then((res) => {
      this.blogCategories = res;
    });
  }

  add() {
    this.blogCategory = new BlogCategory();
    this.blogCategory.id = 0;
    this.isVisible = true;
    this.title = "Yeni Blog Kategori Ekle";
  }

  onSelectBlogCategory(e, c) {
    if (e.item.value === "edit") {
      this.isVisible = true;
      this.title = "Blog Kategori Düzenle";
      this.blogCategory = c.data;
    } else if (e.item.value === "delete") {
      this.confirmService.delete().then((res: boolean) => {
        if (res) {
          this.appService.delete(Urls.BlogCategory, c.data.id).then(() => {
            this.alertService.showDefaultMessage(
              defaultMessageType.delete,
              alertType.success
            );
            this.bindGrid();
          });
        }
      });
    }
  }

  save(e) {
    const result = e.validationGroup.validate();
    if (result.isValid) {
      if (this.blogCategory.id) {
        this.appService
          .put(Urls.BlogCategory, this.blogCategory)
          .then((res) => {
            this.isVisible = false;
            this.bindGrid();
            this.alertService.showDefaultMessage(
              defaultMessageType.update,
              alertType.success
            );
          });
      } else {
        this.blogCategory.id = 0;
        this.appService
          .post(Urls.BlogCategory, this.blogCategory)
          .then((res) => {
            this.isVisible = false;
            this.bindGrid();
            this.alertService.showDefaultMessage(
              defaultMessageType.save,
              alertType.success
            );
          });
      }
    }
  }
}
