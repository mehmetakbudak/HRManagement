import { Injectable } from '@angular/core';
import { AppService } from '../app.service';

@Injectable({
  providedIn: 'root'
})

export class LookupService {
  constructor(private appService: AppService) { }

  get(code: Lookup, name = ''): Promise<any> {
    let filterName = '';
    if (name) {
      filterName = "contains(name,'" + name + "')&";
    }
    return this.appService.getOdata(this.appService.lookup + "?$filter=type eq '" + Lookup[code] + "'&" + filterName + '$orderby=name');
  }
}


export enum Lookup {
  Title = 1
}


