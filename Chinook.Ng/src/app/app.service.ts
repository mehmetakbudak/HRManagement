import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { AuthenticationService } from "./services/authentication.service";
import ODataStore from "devextreme/data/odata/store";

@Injectable({
  providedIn: "root",
})
export class AppService {
  public blogCategory = "BlogCategory";
  public noteCategory = "NoteCategory";
  public note = "Note";
  public user = "User";
  public lookup = "Lookup";
  public profile = "Profile";
  public menu = "Menu";
  public blog = "Blog";
  public feBlogCategory = "feBlogCategory";
  public feBlog = "feBlog";
  public fePage = "fePage";
  public feMenu = "feMenu";

  constructor(
    private http: HttpClient,
    private authenticationService: AuthenticationService
  ) {}

  getToken(): string {
    let currentUser = this.authenticationService.currentUserValue;
    return currentUser.token;
  }

  getApi(url) {
    return this.http.get(environment.apiUrl + url).toPromise();
  }

  getApiWithAut(url) {
    return this.http.get(environment.apiUrl + url).toPromise();
  }

  getOdata(url) {
    return this.http.get(environment.oDataUrl + url).toPromise();
  }

  getOdataStore(url) {
    return new ODataStore({
      url: environment.oDataUrl + url,
      version: 4,
      beforeSend: (e) => {
        e.headers = {
          Authorization: "Bearer " + this.getToken(),
        };
      },
    });
  }

  post(url, data) {
    return this.http.post(environment.apiUrl + url, data).toPromise();
  }

  put(url, data) {
    return this.http.put(environment.apiUrl + url, data).toPromise();
  }

  delete(url, id) {
    return this.http.delete(environment.apiUrl + url + "/" + id).toPromise();
  }

  settingEditDelete() {
    return [
      { text: "Düzenle", value: "edit" },
      { text: "Sil", value: "delete" },
    ];
  }
}
