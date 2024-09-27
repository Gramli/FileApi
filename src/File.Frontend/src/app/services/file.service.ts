import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IBase64File, IDataResponse, IFile } from '../app.model';

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

  public downloadFile(id: number) {
    return this.httpClient.get(`${this.apiBaseUrl}/download?id=${id}`, { responseType: 'blob' });
  }

  public downloadFileAsString(id: number) {
    return this.httpClient.get<IDataResponse<IBase64File>>(`${this.apiBaseUrl}/downloadAsJson?id=${id}`);
  }
}
