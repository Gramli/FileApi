import { Injectable } from '@angular/core';
import { Subject, Subscription } from 'rxjs';
import { IFile } from '../app.model';
import { FileApiHttpService } from './file-api.http.service';
import { Buffer } from 'buffer';

@Injectable()
export class FileLoadingService {
  public loading: boolean = false;

  public get filesInfo() {
    return this.filesInfo$.asObservable();
  }

  private filesInfo$ = new Subject<IFile[]>();

  constructor(private fileService: FileApiHttpService) { }

  public downloadFile(id: number, onSuccess: (fileContent: Blob) => void, onError?: (error: any) => void) {
    this.runSubScriptionWithProgress(() =>
      this.fileService.downloadFile(id).subscribe({
        next: (response) => onSuccess(response),
        error: (error: any) => this.processError(error, onError)
      }));
  }

  public downloadFileAsJson(id: number, onSuccess: (fileContent: string) => void, onError?: (error: any) => void) {
    this.runSubScriptionWithProgress(() =>
      this.fileService.downloadFileAsString(id).subscribe({
        next: (response) => {
          const fileContent = Buffer.from(response.data.data, 'base64').toString('utf-8');
          onSuccess(fileContent);
        },
        error: (error: any) => this.processError(error, onError)
      }));
  }

  public uploadFile(file: File, onSuccess?: () => void, onError?: (error: any) => void) {
    if (file) {
      const upload$ = this.fileService.uploadFile(file);

      this.runSubScriptionWithProgress(() => upload$.subscribe({
        next: () => {
          this.loadFileData();
          if (onSuccess) {
            onSuccess();
          }
        },
        error: (error: any) => this.processError(error, onError)
      }));

    }
  }

  public loadFileData() {
    this.runSubScriptionWithProgress(() =>
      this.fileService.getFiles().subscribe({
        next: (response) => {
          this.filesInfo$.next(response.data);
        },
        error: (error) => this.processError(error)
      }));
  }

  public exportFile(id: number, extension: string, onSuccess: (fileContent: Blob) => void, onError?: (error: any) => void) {
    this.runSubScriptionWithProgress(() =>
      this.fileService.exportFile(id, extension).subscribe({
        next: (response) => onSuccess(response),
        error: (error: any) => this.processError(error, onError)
      }));
  }

  public convertFile(file: File, extension: string, onSuccess: (fileContent: Blob) => void, onError?: (error: any) => void) {
    if (file) {
      const upload$ = this.fileService.convertFile(file, extension);

      this.runSubScriptionWithProgress(() => upload$.subscribe({
        next: (response) => {
          onSuccess(response);
        },
        error: (error: any) => this.processError(error, onError)
      }));

    }
  }

  private processError(error: any, onError?: (error: any) => void) {
    if (onError) {
      onError(error);
    }
    else {
      console.error(error);
    }
  }

  private runSubScriptionWithProgress(action: () => Subscription) {
    this.loading = true;
    action().add(() => { this.loading = false; });
  }
}
