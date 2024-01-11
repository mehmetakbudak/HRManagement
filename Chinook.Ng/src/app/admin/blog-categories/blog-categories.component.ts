import { Component, OnInit } from "@angular/core";
import { AppService } from "../../app.service";
import { Urls } from "src/app/models/consts";
import { AbstractControl, FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ConfirmationService, MessageService } from "primeng/api";

class BlogCategoryFilter {
  name: string;
  url: string;
  isActive: boolean = null;
}

@Component({
  selector: "app-blog-categories",
  templateUrl: "./blog-categories.component.html",
})
export class BlogCategoriesComponent implements OnInit {
  form: FormGroup;
  totalRecords: number;
  rows = 5;
  first = 0;
  menuItems = [];
  gridMenuItems = [];
  loading = false;
  selectedData: any;
  filterForm: BlogCategoryFilter;
  submitted = false;
  blogCategories = [];
  isVisible = false;
  title = "";

  constructor(
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private appService: AppService,
    private formBuilder: FormBuilder
  ) { }

  ngOnInit() {
    this.filterForm = new BlogCategoryFilter();
    this.getBlogCategories();
    this.form = this.formBuilder.group({
      id: [0, []],
      name: ['', Validators.required],
      url: ['', [Validators.required]],
      isActive: [true, Validators.required]
    });
    this.menuItems = [
      { label: "Add", command: () => { this.add() } }
    ];
    this.gridMenuItems = [
      { label: "Edit", command: () => { this.edit() } },
      { label: "Delete", command: (e) => { this.delete(e) } }
    ];
  }

  getBlogCategories() {
    this.appService.post('BlogCategory/GetByFilter', {
      first: this.first,
      rows: this.rows,
      ...this.filterForm
    }).then((res: any) => {
      this.blogCategories = res.data;
      this.totalRecords = res.count;
    })
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  pageChange(e) {
    this.first = e.first;
    this.rows = e.rows;
    this.getBlogCategories();
  }

  menuToggle(menu, e, data) {
    this.gridMenuItems.forEach((menuItem) => {
      menuItem.data = data;
    });
    this.selectedData = data;
    menu.toggle(e);
  }

  reset() {
    this.filterForm = new BlogCategoryFilter();
    this.getBlogCategories();
  }

  search() {
    this.getBlogCategories();
  }

  add() {
    this.form.reset();
    this.isVisible = true;
    this.form.get('id').setValue(0);
    this.form.get('isActive').setValue(true);
    this.title = "Add Blog Category";
  }

  edit() {
    this.appService.get(`${Urls.BlogCategory}/${this.selectedData.id}`)
      .then((res: any) => {
        this.form.setValue(res);
        this.isVisible = true;
        this.title = "Edit Blog Category";
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
            this.getBlogCategories();
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
          this.getBlogCategories();
          this.messageService.add({ severity: 'info', summary: 'Success', detail: 'Addition was successful' });
          this.isVisible = false;
        });
    } else {
      this.appService.put(Urls.BlogCategory, this.form.value)
        .then((res: any) => {
          this.getBlogCategories();
          this.messageService.add({ severity: 'info', summary: 'Success', detail: 'The update was successful' });
          this.isVisible = false;
        });
    }
  }
}
