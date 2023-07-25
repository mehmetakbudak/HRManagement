import { Component, Input, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { AppService } from "src/app/app.service";
import { Urls } from "src/app/models/consts";

@Component({
  selector: "app-blog-category",
  templateUrl: "./blog-category.component.html",
  styleUrls: ["./blog-category.component.scss"],
})
export class BlogCategoryComponent implements OnInit {
  blogs = [];
  blogCategories: [];
  pageSize = 5;
  page = 1;

  blogPager;
  url;
  blogCategory;
  total;
  skip = 0;

  constructor(private route: ActivatedRoute, private appService: AppService) {}

  ngOnInit() {
    this.route.params.subscribe((params) => {
      this.url = params["url"];
      this.getBlog();
    });
    this.getBlogCategories();
  }

  getBlog() {
    this.appService
      .get(`${Urls.Blog}/GetBlogsByCategoryUrl/${this.url}`)
      .then((res: any) => {
        this.blogs = res.value;
      });
  }

  getBlogCategories() {
    this.appService.get(`${Urls.Lookup}/BlogCategories`).then((res: any) => {
      this.blogCategories = res;
    });
  }

  onPageChange(e: number) {
    this.page = e;
    this.skip = (e - 1) * this.pageSize;
    // this.loadBlog();
  }
}
