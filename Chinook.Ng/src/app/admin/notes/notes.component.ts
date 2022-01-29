import { Component, OnInit } from "@angular/core";
import { AppService } from "../../app.service";
import {
  defaultMessageType,
  AlertService,
  alertType,
} from "src/app/services/alert.service";
import { ConfirmService } from "src/app/services/confirm.service";

export class NoteCategory {
  id: number;
  name: string;
}
export class Note {
  id: number;
  noteCategoryId: number;
  title: string;
  description: string;
}

@Component({
  selector: "app-notes",
  templateUrl: "./notes.component.html",
  styleUrls: ["./notes.component.css"],
})
export class NotesComponent implements OnInit {
  noteCategorySettings = [
    { text: "Düzenle", value: "edit" },
    { text: "Sil", value: "delete" },
  ];
  noteSettings = [
    { text: "Taşı", value: "move" },
    { text: "Sil", value: "delete" },
  ];

  noteCategory: NoteCategory = new NoteCategory();
  note: Note = new Note();
  moveNote: Note = new Note();

  screenWidth = "40vw";
  noteCategoryId;
  selectNoteCategoryId;
  noteId;

  categories;
  moveCategories;
  selectMoveNoteId;
  countCategory;

  notes;
  countNote;
  noteHeader;

  isCategoryVisible = false;
  visibleNotes = false;
  visibleNoteDetail = false;
  titleCategory;

  isMoveNoteVisible = false;

  constructor(
    private alertService: AlertService,
    private confirmService: ConfirmService,
    private appService: AppService
  ) {}

  ngOnInit() {
    this.categoryBind();
    if (screen.width < 768) {
      this.screenWidth = "90vw";
    }
  }

  categoryBind() {
    this.appService.getApi(this.appService.noteCategory).then((res: any) => {
      this.categories = res;
      this.countCategory = res.length;
    });
  }

  onSelectRowNoteCategory(e) {
    this.selectNoteCategoryId = e.row.data.id;
    this.noteBind();
    this.visibleNoteDetail = false;
  }

  onSelectRowNote(e) {
    this.noteHeader = "Not Düzenle";
    this.visibleNoteDetail = true;
    this.note = e.row.data;
  }

  addCategory() {
    this.titleCategory = "Yeni Kategori";
    this.isCategoryVisible = true;
    this.visibleNotes = false;
    this.visibleNoteDetail = false;
    this.noteCategory = new NoteCategory();
  }

  noteBind() {
    this.appService
      .getApi(
        `${this.appService.note}/GetByCategoryId/${this.selectNoteCategoryId}`
      )
      .then((res: any) => {
        this.notes = res;
        this.countNote = res.length;
      });
  }

  addNote() {
    this.noteHeader = "Yeni Not";
    this.visibleNoteDetail = true;
    this.note = new Note();
  }

  saveNote(e) {
    const result = e.validationGroup.validate();
    if (result.isValid) {
      if (this.note.id) {
        this.appService.put(this.appService.note, this.note).then((res) => {
          this.noteBind();
          this.noteClear();
          this.alertService.showDefaultMessage(
            defaultMessageType.update,
            alertType.success
          );
        });
      } else {
        this.note.id = 0;
        this.note.noteCategoryId = this.noteCategoryId;
        this.appService.post(this.appService.note, this.note).then((res) => {
          this.noteBind();
          this.noteClear();
          this.alertService.showDefaultMessage(
            defaultMessageType.save,
            alertType.success
          );
        });
      }
    }
  }
  saveCategory(e) {
    const result = e.validationGroup.validate();
    if (result.isValid) {
      if (this.noteCategory.id) {
        this.appService
          .put(this.appService.noteCategory, this.noteCategory)
          .then((res) => {
            this.categoryBind();
            this.isCategoryVisible = false;
            this.alertService.showDefaultMessage(
              defaultMessageType.update,
              alertType.success
            );
          });
      } else {
        this.noteCategory.id = 0;
        this.appService
          .post(this.appService.noteCategory, this.noteCategory)
          .then((res) => {
            this.isCategoryVisible = false;
            this.categoryBind();
            this.alertService.showDefaultMessage(
              defaultMessageType.save,
              alertType.success
            );
          });
      }
    }
  }

  onSelectNoteCategory(e, c) {
    if (e.item.value === "edit") {
      this.isCategoryVisible = true;
      this.titleCategory = "Kategori Düzenle";
      this.noteCategory = c.data;
    } else if (e.item.value === "delete") {
      this.confirmService.delete().then((isAccept: boolean) => {
        if (isAccept) {
          this.appService
            .delete(this.appService.noteCategory, c.data.id)
            .then((res) => {
              this.categoryBind();
              this.alertService.showDefaultMessage(
                defaultMessageType.delete,
                alertType.success
              );
            });
        }
      });
    }
  }

  onSelectNote(e, c) {
    if (e.item.value === "move") {
      this.isMoveNoteVisible = true;
      this.moveNote = c.data;
    } else if (e.item.value === "delete") {
      this.confirmService.delete().then((isDelete: boolean) => {
        if (isDelete) {
          this.appService
            .delete(this.appService.note, c.data.id)
            .then((res) => {
              this.noteBind();
              this.visibleNoteDetail = false;
              this.alertService.showDefaultMessage(
                defaultMessageType.delete,
                alertType.success
              );
            });
        }
      });
    }
  }

  onMoveNote(e) {
    const result = e.validationGroup.validate();
    if (result.isValid) {
      this.moveNote.noteCategoryId = this.selectMoveNoteId;
      this.appService
        .put(this.appService.note + "/move", this.moveNote)
        .then((res) => {
          this.isMoveNoteVisible = false;
          this.visibleNoteDetail = false;
          this.noteBind();
          this.alertService.showMessage(
            "Taşıma işlemi başarıyla gerçekleşti.",
            alertType.info
          );
        });
    }
  }

  noteClear() {
    this.noteHeader = "Yeni Not";
    this.note = new Note();
  }
}
