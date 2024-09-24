import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IDataResponse, IFile } from './app.model';

@Injectable({
  providedIn: 'root'
})
export class FileService {
  private apiBaseUrl: string = 'https://localhost:7270/file/v1'
  constructor(private httpClient: HttpClient) { }

  public getFiles() {
    return this.httpClient.get<IDataResponse<IFile[]>>(`${this.apiBaseUrl}/files-info`);
  }

  public uploadFile(file: FormData) {
    return this.httpClient.post(`${this.apiBaseUrl}/upload`, file);
  }
}
