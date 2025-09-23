import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IBase64File, IDataResponse, IFile } from '../app.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class FileApiHttpService {
  private apiBaseUrl: string = 'https://localhost:7270/v1/files';
  constructor(private httpClient: HttpClient) {}

  public getFiles(): Observable<IDataResponse<IFile[]>> {
    return this.httpClient.get<IDataResponse<IFile[]>>(
      `${this.apiBaseUrl}/`
    );
  }

  public uploadFile(file: File): Observable<IDataResponse<boolean>> {
    const formData = new FormData();
    formData.append('file', file);
    return this.httpClient.post<IDataResponse<boolean>>(
      `${this.apiBaseUrl}/upload`,
      formData
    );
  }

  public downloadFile(id: number): Observable<Blob> {
    return this.httpClient.get(`${this.apiBaseUrl}/${id}/download/`, {
      responseType: 'blob',
    });
  }

  public downloadFileAsString(
    id: number
  ): Observable<IDataResponse<IBase64File>> {
    return this.httpClient.get<IDataResponse<IBase64File>>(
      `${this.apiBaseUrl}/${id}/download/json`
    );
  }

  public exportFile(id: number, extension: string): Observable<Blob> {
    return this.httpClient.get(
      `${this.apiBaseUrl}/${id}/export?extension=${extension}`,
      { responseType: 'blob' }
    );
  }

  public convertFile(file: File, extension: string): Observable<Blob> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('formatToConvert', extension);
    return this.httpClient.post(`${this.apiBaseUrl}/convert`, formData, {
      responseType: 'blob',
    });
  }
}
