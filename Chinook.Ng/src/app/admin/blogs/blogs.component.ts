import { Component, OnInit } from "@angular/core";
import { AbstractControl, FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { ConfirmationService, MessageService } from "primeng/api";
import { AppService } from "src/app/app.service";
import { Urls } from "src/app/models/consts";

class BlogFilter {
  blogCategoryId: number;
  title: string;
  url: string;
  isActive: boolean = null;
}

@Component({
  selector: "app-blogs",
  templateUrl: "./blogs.component.html",
  styleUrls: ["./blogs.component.scss"],
})
export class BlogsComponent implements OnInit {
  form: FormGroup;
  blogs = [];
  // blogCategories: [];
  totalRecords: number;
  rows = 5;
  first = 0;
  menuItems = [];
  gridMenuItems = [];
  selectedData: any;
  loading = false;
  title: string = "";
  filterForm: BlogFilter;
  submitted = false;
  isVisible = false;

  constructor(
    private appService: AppService,
    private formBuilder: FormBuilder,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) { }

  ngOnInit() {
    this.getBlogs();
    this.filterForm = new BlogFilter();
    this.form = this.formBuilder.group({
      id: [0, []],
      blogCategoryId: [0, []],
      shortDefinition: ['', []],
      description: ['', []],
      imageUrl: ['', []],
      sequence: [0, []],
      title: ['', Validators.required],
      url: ['', [Validators.required]],
      isActive: [true, Validators.required],
      published: [true, Validators.required]
    });
    this.menuItems = [
      { label: "Add", command: () => { this.add() } }
    ];
    this.gridMenuItems = [
      { label: "Edit", command: () => { this.edit() } },
      { label: "Delete", command: (e) => { this.delete(e) } }
    ];
    // this.getBlogCategories();
  }

  getBlogs() {
    this.appService.post(`${Urls.Blog}/GetByFilter`, {
      first: this.first,
      rows: this.rows,
      ...this.filterForm
    }).then((res: any) => {
      this.totalRecords = res.count;
      this.blogs = res.data;
    });
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  pageChange(e) {
    this.first = e.first;
    this.rows = e.rows;
    this.getBlogs();
  }

  menuToggle(menu, e, data) {
    this.gridMenuItems.forEach((menuItem) => {
      menuItem.data = data;
    });
    this.selectedData = data;
    menu.toggle(e);
  }

  reset() {
    this.filterForm = new BlogFilter();
    this.getBlogs();
  }

  search() {
    this.getBlogs();
  }

  add() {
    this.form.reset();
    this.isVisible = true;
    this.form.get('id').setValue(0);
    this.form.get('isActive').setValue(true);
    this.title = "Add Blog";
  }

  edit() {
    this.appService.get(`${Urls.Blog}/${this.selectedData.id}`)
      .then((res: any) => {
        this.form.setValue(res);
        this.isVisible = true;
        this.title = "Edit Blog";
      });
  }

  delete(e) {
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: 'Are you sure that you want to delete?',
      header: 'Delete',
      icon: 'pi pi-exclamation-triangle',
      acceptIcon: "none",
      rejectIcon: "none",
      rejectButtonStyleClass: "p-button-text",
      accept: () => {
        this.appService.delete(`${Urls.BlogCategory}`, this.selectedData.id)
          .then((res: any) => {
            this.getBlogs();
            this.messageService.add({ severity: 'info', summary: 'Success', detail: 'Deletion was successful' });
          })
      }
    });
  }

  save() {
    this.submitted = true;
    if (this.form?.invalid) {
      return;
    }
    if (this.form.value.id == 0) {
      this.appService.post(Urls.BlogCategory, this.form.value)
        .then((res: any) => {
          this.getBlogs();
          this.messageService.add({ severity: 'info', summary: 'Success', detail: 'Addition was successful' });
          this.isVisible = false;
        });
    } else {
      this.appService.put(Urls.BlogCategory, this.form.value)
        .then((res: any) => {
          this.getBlogs();
          this.messageService.add({ severity: 'info', summary: 'Success', detail: 'The update was successful' });
          this.isVisible = false;
        });
    }
  }
}
