import {Injectable} from '@angular/core';
import {DataService} from "./data.service";
import {ImageGetResponse} from "../models/image-get-response";
@Injectable({
  providedIn: 'root'
})
export class ImageService {

  constructor(private dataService: DataService) {
  }

  public async GetImageAsync(imageName: string){
    const imgResponse = await this.dataService.GetAsync<ImageGetResponse>(`image?imageName=${imageName}`);
    return imgResponse.imageSrc;
  }
}
