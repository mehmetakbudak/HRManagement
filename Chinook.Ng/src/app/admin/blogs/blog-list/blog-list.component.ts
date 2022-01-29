import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { AppService } from "src/app/app.service";

@Component({
  selector: "app-blog",
  templateUrl: "./blog-list.component.html",
  styleUrls: ["./blog-list.component.scss"],
})
export class BlogListComponent implements OnInit {
  blogs = [];
  settings = [];

  constructor(private appService: AppService, private router: Router) {}

  ngOnInit() {
    this.settings = this.appService.settingEditDelete();
    this.getBlogs();
  }

  getBlogs() {
    this.appService.getApi(this.appService.blog).then((res: any) => {
      this.blogs = res;
    });
  }

  onSelectBlog(e, c) {
    if (e.item.value === "edit") {
      this.router.navigateByUrl(`/admin/blogs/edit/${c.data.id}`);
    }
  }
}
