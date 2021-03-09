import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppService } from 'src/app/app.service';

@Component({
  selector: 'app-blog-single',
  templateUrl: './blog-single.component.html',
  styleUrls: ['./blog-single.component.scss']
})
export class BlogSingleComponent implements OnInit {
  id: number;
  blog;

  constructor(private route: ActivatedRoute,
    private appService: AppService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.id = params['id'];
      this.appService.getOdata(this.appService.feBlog + "?$filter=id eq " + this.id).then((res: any) => {
        this.blog = res.value[0];

      });
    });
  }

}
