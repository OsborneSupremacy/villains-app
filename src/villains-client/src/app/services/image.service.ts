import {Injectable} from '@angular/core';
import {DataService} from "./data.service";
import {ImageGetResponse} from "../models/image-get-response";
import {ImageUploadRequest} from "../models/image-upload-request";
import {ImageUploadResponse} from "../models/image-upload-response";

@Injectable({
  providedIn: 'root'
})
export class ImageService {

  private imageCache: { [key: string]: ImageGetResponse } = {};

  constructor(private dataService: DataService) {
  }

  public GetImageSource(imageName: string): string {
    if (!this.imageCache[imageName])
      return `./assets/images/villain-placeholder.jpeg`;
    return this.imageCache[imageName].imageSrc;
  }

  public async CacheImageAsync(imageName: string) {
    if(this.imageCache[imageName])
      return;
    this.imageCache[imageName] = await this.dataService
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
