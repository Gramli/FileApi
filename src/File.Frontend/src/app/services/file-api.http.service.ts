import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IBase64File, IDataResponse, IFile } from '../app.model';

@Injectable({
  providedIn: 'root'
})
export class FileApiHttpService {
  private apiBaseUrl: string = 'https://localhost:7270/file/v1'
  constructor(private httpClient: HttpClient) { }

  public getFiles() {
    return this.httpClient.get<IDataResponse<IFile[]>>(`${this.apiBaseUrl}/files-info`);
  }

  public uploadFile(file: File) {
    const formData = new FormData();
    formData.append("file", file);
    return this.httpClient.post(`${this.apiBaseUrl}/upload`, formData);
  }

  public downloadFile(id: number) {
    return this.httpClient.get(`${this.apiBaseUrl}/download?id=${id}`, { responseType: 'blob' });
  }

  public downloadFileAsString(id: number) {
    return this.httpClient.get<IDataResponse<IBase64File>>(`${this.apiBaseUrl}/downloadAsJson?id=${id}`);
  }

  public exportFile(id: number, extension: string) {
    return this.httpClient.post(`${this.apiBaseUrl}/export`, {
      id,
      extension
    }, { responseType: 'blob' });
  }

  public convertFile(file: File, extension: string) {
    const formData = new FormData();
    formData.append("file", file);
    formData.append("formatToConvert", extension);
    return this.httpClient.post(`${this.apiBaseUrl}/convert`, formData, { responseType: 'blob' });
  }
}
