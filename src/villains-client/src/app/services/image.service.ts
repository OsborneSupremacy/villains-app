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
    // Assuming you have an endpoint like /images/{imageName}
    const endpoint = `${this.baseUrl}/images?imageName=${imageName}`;
    return this.http.get(endpoint);
  }

  // Other image-related methods here
}
