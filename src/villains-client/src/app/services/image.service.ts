import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {DataService} from "./data.service";
import {ImageGetResponse} from "../models/image-get-response";
@Injectable({
  providedIn: 'root'
})
export class ImageService {

  constructor(private dataService: DataService) {
  }

  public async GetImageAsync(imageName: string): Promise<Observable<ImageGetResponse>> {
    return this.dataService.GetAsync<ImageGetResponse>(`images?imageName=${imageName}`);
  }

}
