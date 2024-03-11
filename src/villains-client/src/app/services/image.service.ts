import {Injectable} from '@angular/core';
import {DataService} from "./data.service";
import {ImageGetResponse} from "../models/image-get-response";
import {ImageUploadRequest} from "../models/image-upload-request";
import {ImageUploadResponse} from "../models/image-upload-response";

@Injectable({
  providedIn: 'root'
})
export class ImageService {

  constructor(private dataService: DataService) {
  }

  public async GetImageAsync(imageName: string) {
    return await this.dataService
      .GetAsync<ImageGetResponse>(`image?imageName=${imageName}`);
  }

  public async AddAsync(fileName: string, base64Image: string) {

    const request: ImageUploadRequest = {
      fileName: fileName,
      base64EncodedImage: base64Image
    };

    return await this.dataService
      .PostAsync<ImageUploadRequest, ImageUploadResponse>(`image`, request);
  }
}
