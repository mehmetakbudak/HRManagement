import { Component, OnInit } from "@angular/core";
import { AppService } from "../../app.service";
import { Urls } from "src/app/models/consts";
import { ConfirmationService, MessageService } from "primeng/api";
import { AbstractControl, FormBuilder, FormGroup, Validators } from "@angular/forms";
class NoteFilter {
  noteCategoryIds: [];
  title: string;
}
@Component({
  selector: "app-notes",
  templateUrl: "./notes.component.html",
  styleUrls: ["./notes.component.css"],
})
export class NotesComponent implements OnInit {
  form: FormGroup;
  formNoteCategory: FormGroup;
  notes = [];
  filterForm: NoteFilter;
  totalRecords: number;
  rows = 5;
  first = 0;
  loading = false;
  menuItems = [];
  gridMenuItems = [];
  gridNoteCategoryMenuItems = [];
  selectedData: any;
  noteCategories = [];
  isVisibleNote = false;
  isVisibleNoteCategory = false;
  title = "";
  submitted = false;
  noteCategoryHeader = "";

  constructor(
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private appService: AppService,
    private formBuilder: FormBuilder
  ) { }

  ngOnInit() {
    this.getNotes();
    this.getNoteCategories();
    this.filterForm = new NoteFilter();
    this.form = this.formBuilder.group({
      id: [0, []],
      noteCategoryId: [0, Validators.required],
      title: ['', [Validators.required]],
      description: ['', []]
    });
    this.formNoteCategory = this.formBuilder.group({
      id: [0, []],
      name: ['', [Validators.required]]
    });
    this.menuItems = [
      { label: "Add", command: () => { this.add() } }
    ];
    this.gridMenuItems = [
      { label: "Edit", command: () => { this.edit() } },
      { label: "Delete", command: (e) => { this.delete(e) } }
    ];
    this.gridNoteCategoryMenuItems= [
      { label: "Edit", command: () => { this.editNoteCategory() } },
      { label: "Delete", command: (e) => { this.deleteNoteCategory(e) } }
    ];
  }

  getNoteCategories() {
    this.appService.get(Urls.NoteCategory).then((res: any) => {
      this.noteCategories = res;
    });
  }

  getNotes() {
    this.loading = true;
    this.appService.post(`${Urls.Note}/GetByFilter`, {
      first: this.first,
      rows: this.rows,
      ...this.filterForm
    }).then((res: any) => {
      this.notes = res.data;
      this.totalRecords = res.count;
      this.loading = false;
    });
  }

  pageChange(e) {
    this.first = e.first;
    this.rows = e.rows;
    this.getNotes();
  }

  menuToggle(menu, e, data) {
    this.gridMenuItems.forEach((menuItem) => {
      menuItem.data = data;
    });
    this.selectedData = data;
    menu.toggle(e);
  }

  reset() {
    this.filterForm = new NoteFilter();
    this.getNotes();
  }

  search() {
    this.getNotes();
  }

  showNoteCategory() {
    this.isVisibleNoteCategory = true;
    this.noteCategoryHeader = "Add Note Category";
  }

  get fNoteCategory(): { [key: string]: AbstractControl } {
    return this.formNoteCategory.controls;
  }

  add() {
    this.title = "Add Note";
    this.isVisibleNote = true;
    this.form.reset();
    this.form.get('id').setValue(0);
  }

  edit() {
    console.log(this.selectedData);
    this.appService.get(`${Urls.Note}/${this.selectedData.id}`)
      .then((res: any) => {
        this.form.setValue(res);
        this.isVisibleNote = true;
        this.title = "Edit Note";
      });
  }

  delete(event) {
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: 'Are you sure that you want to delete?',
      header: 'Delete',
      icon: 'pi pi-exclamation-triangle',
      acceptIcon: "none",
      rejectIcon: "none",
      rejectButtonStyleClass: "p-button-text",
      accept: () => {
        this.appService.delete(`${Urls.Note}`, this.selectedData.id)
          .then((res: any) => {
            this.getNotes();
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
      this.appService.post(Urls.Note, this.form.value)
        .then((res: any) => {
          this.getNotes();
          this.messageService.add({ severity: 'info', summary: 'Success', detail: 'Addition was successful' });
          this.isVisibleNote = false;
        });
    } else {
      this.appService.put(Urls.Note, this.form.value)
        .then((res: any) => {
          this.getNotes();
          this.messageService.add({ severity: 'info', summary: 'Success', detail: 'The update was successful' });
          this.isVisibleNote = false;
        });
    }
  }

  editNoteCategory(){

  }

  deleteNoteCategory(e) {

  }

  saveNoteCategory() {
    this.submitted = true;
    if (this.formNoteCategory?.invalid) {
      return;
    }
  }
}
