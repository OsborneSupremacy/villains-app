import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ImageService {

  private baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getImage(imageName: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/images?imageName=${imageName}`);
  }

  // Other image-related methods here
}
