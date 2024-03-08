import {Injectable} from '@angular/core';
import { lastValueFrom } from "rxjs";
import {environment} from "../../environments/environment";
import {HttpClient, HttpContext, HttpHeaderResponse, HttpHeaders, HttpParams} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class DataService {

  private baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) {
  }

  public GetAsync = async <T>(subUrl: string) => {
    return await lastValueFrom(this.http.get<T>(`${this.baseUrl}${subUrl}`));
  }

  public PostAsync = async <TIn, TOut>(subUrl: string, body: TIn) => {
    return await lastValueFrom(this.http.post<TOut>(`${this.baseUrl}${subUrl}`, body));
  }

  public PutAsync = async <T>(subUrl: string, body: T) => {
    return await lastValueFrom(this.http.put(`${this.baseUrl}${subUrl}`, body));
  }
}
