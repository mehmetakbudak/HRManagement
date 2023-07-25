import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { AuthenticationService } from "./services/authentication.service";
import ODataStore from "devextreme/data/odata/store";

@Injectable({
  providedIn: "root",
})
export class AppService {

  constructor(
    private http: HttpClient,
    private authenticationService: AuthenticationService
  ) {}

  getToken(): string {
    let currentUser = this.authenticationService.currentUserValue;
    return currentUser.token;
  }

  get(url) {
    return this.http.get(environment.apiUrl + url).toPromise();
  }

  getApiWithAut(url) {
    return this.http.get(environment.apiUrl + url).toPromise();
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
