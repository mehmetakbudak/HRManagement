import { Injectable } from "@angular/core";
import { AppService } from "../app.service";
import { Urls } from "../models/consts";

@Injectable({
  providedIn: "root",
})
export class LookupService {
  constructor(private appService: AppService) {}

  get(code: Lookup, name = ""): Promise<any> {
    let filterName = "";
    if (name) {
      filterName = "contains(name,'" + name + "')&";
    }
    return this.appService.get(
      Urls.Lookup +
        "?$filter=type eq '" +
        Lookup[code] +
        "'&" +
        filterName +
        "$orderby=name"
    );
  }
}

export enum Lookup {
  Title = 1,
}
