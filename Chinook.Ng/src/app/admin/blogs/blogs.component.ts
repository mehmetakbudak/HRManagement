import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { AppService } from "src/app/app.service";
import { Blog } from "src/app/models/blog";
import { Urls } from "src/app/models/consts";

class Filter {
  page: number = 1;
  pageSize: number = 5;
  blogCategoryId: number;
  title: string;
}

@Component({
  selector: "app-blogs",
  templateUrl: "./blogs.component.html",
  styleUrls: ["./blogs.component.scss"],
})
export class BlogsComponent implements OnInit {
  blogs = [];
  settings = [];
  blogCategories: [];
  total: number;
  filter: Filter = new Filter();
  modalTitle: string = "";
  blog: Blog = new Blog();

  constructor(
    private appService: AppService,
    private modalService: NgbModal,
    private router: Router
  ) {}

  ngOnInit() {
    this.settings = this.appService.settingEditDelete();
    this.getBlogs();
    this.getBlogCategories();
  }

  getBlogs() {
    this.appService
      .post(`${Urls.Blog}/GetByFilter`, this.filter)
      .then((res: any) => {
        this.total = res.count;
        this.blogs = res.list;
      });
  }

  filterForm(){
    this.getBlogs();
  }

  getBlogCategories() {
    this.appService.get(Urls.BlogCategory).then((res: any) => {
      this.blogCategories = res;
    });
  }

  onSelectBlog(e, c) {
    if (e.item.value === "edit") {
      this.router.navigateByUrl(`/admin/blogs/edit/${c.data.id}`);
    }
  }

  pageChange() {
    this.getBlogs();
  }

  add(content) {
    this.blog = new Blog();
    this.modalTitle = "Blog Ekle";
    this.modalService.open(content, { size: "lg" });
  }

  edit(e, content) {
    this.modalTitle = "Blog Düzenle";
    this.modalService.open(content, { size: "lg" });
  }

  save() {}
}
