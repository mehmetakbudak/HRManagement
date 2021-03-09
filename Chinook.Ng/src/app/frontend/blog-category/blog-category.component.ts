import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppService } from 'src/app/app.service';

@Component({
  selector: 'app-blog-category',
  templateUrl: './blog-category.component.html',
  styleUrls: ['./blog-category.component.scss']
})
export class BlogCategoryComponent implements OnInit {
  blogs = [];
  pageSize = 5;
  page = 1;

  blogPager;
  url;
  blogCategory;
  total;
  skip = 0;

  constructor(private route: ActivatedRoute,
    private appService: AppService) {
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.url = params['url'];
      this.appService.getOdata(this.appService.feBlogCategory + "?$filter=url eq '" + this.url + "'")
        .then((resBlogCategory: any) => {
          this.blogCategory = resBlogCategory.value[0];

          if (this.blogCategory) {
            this.loadBlog();
          }
        });
    });
  }

  loadBlog() {
    this.appService.getOdata(this.appService.feBlog + "?$filter=blogCategoryId eq " +
      this.blogCategory.id + "&$skip=" + this.skip + "&$top=" + this.pageSize + "&$count=true")
      .then((resBlog: any) => {
        this.total = resBlog['@odata.count'];
        this.blogs = resBlog.value;
      });
  }

  onPageChange(e: number) {
    this.page = e;
    this.skip = (e - 1) * this.pageSize;
    this.loadBlog();
  }

}
