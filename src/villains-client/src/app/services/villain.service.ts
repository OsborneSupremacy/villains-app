import {Injectable} from '@angular/core';
import {Villain} from '../models/villain';
import {DataService} from "./data.service";
import {VillainCreateResponse} from "../models/villain-create-response";

@Injectable({
  providedIn: 'root'
})
export class VillainService {
  constructor(private dataService: DataService) {
  }

  public async GetAsync(id: string) {
    return this.dataService.GetAsync<Villain>(`villain?id=${id}`);
  }

  public async GetAllAsync() {
    return this.dataService.GetAsync<Villain[]>('villains');
  }

  public async AddAsync(villain: Villain) {
    return await this.dataService.PostAsync<Villain, VillainCreateResponse>('villain', villain);
  }

  public async UpdateAsync(villain: Villain) {
    return await this.dataService.PutAsync<Villain>(`villain`, villain);
  }
}
