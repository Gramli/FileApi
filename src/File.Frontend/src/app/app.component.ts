import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { IFile } from './app.model';
import { faUpload, faFileImport } from '@fortawesome/free-solid-svg-icons';
import { saveAs } from "file-saver";
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FileLoadingService } from './services/file-loading.service';
import { NotificationAdapterService } from './services/notification-adapter.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  providers: [FileLoadingService]
})
export class AppComponent implements OnInit {
  faUpload = faUpload;
  faFileImport = faFileImport;

  @ViewChild('dialog') dialogRef!: TemplateRef<any>;
  data: IFile[] | undefined;
  constructor(protected fileService: FileLoadingService, private ngbModal: NgbModal, private notifierService: NotificationAdapterService) { }

  ngOnInit(): void {
    this.fileService.filesInfo.subscribe({
      next: (response) => {
        this.data = response;
      },
      error: (error) => {
        this.notifierService.showError(`Error: ${error}`);
      }
    });

    this.fileService.loadFileData();
  }

  protected convert(file: File, fileName: string) {
    this.ngbModal.open(this.dialogRef, {
      windowClass: 'modal-job-scrollable'
    }).closed.subscribe((selectedExtension: string) => {
      this.fileService.convertFile(file, selectedExtension, (fileContent) => {
        saveAs(fileContent, this.replaceExtension(fileName, selectedExtension));
        this.notifierService.showSuccess(`Successfuly converted file: ${fileName}`);
      }, () => {
        this.notifierService.showError(`Error to convert file: ${fileName}`);
      })
    });
  }

  protected export(id: number) {
    this.ngbModal.open(this.dialogRef, {
      windowClass: 'modal-job-scrollable'
    }).closed.subscribe((selectedExtension: string) => {
      this.fileService.exportFile(id, selectedExtension, (fileContent) => {
        this.saveFile(id, fileContent, selectedExtension);
        this.notifierService.showSuccess(`Successfuly exported file id: ${id}`);
      }, () => {
        this.notifierService.showError(`Error to export file id: ${id}`);
      })
    });
  }

  protected onDownloadFile(id: number) {
    this.fileService.downloadFile(id, (fileContent) => {
      this.saveFile(id, fileContent);
      this.notifierService.showSuccess(`Successfuly downloaded file id: ${id}`);
    }, () => {
      this.notifierService.showError(`Error to download file id: ${id}`);
    });
  }

  protected onDownloadFileAsJson(id: number) {
    this.fileService.downloadFileAsJson(id, (fileContent) => {
      this.saveFile(id, fileContent);
      this.notifierService.showSuccess(`Successfuly downloaded as json file id: ${id}`);
    }, () => {
      this.notifierService.showError(`Error to download file id: ${id}`);
    });;
  }

  private saveFile(id: number, fileContent: Blob | string, extension?: string) {
    const file = this.data?.filter((file) => file.id === id)[0];
    saveAs(fileContent, extension === undefined ? file?.fileName : this.replaceExtension(file!.fileName, extension));
  }

  protected onUploadFileSelected(event: any) {
    const file = this.getTargetFile(event);
    this.fileService.uploadFile(file, ()=> {
      this.notifierService.showSuccess(`Successfuly uploaded file id: ${file.name}`);
    }, (error) => {
      this.notifierService.showError(`Error: ${error}`);
    });
  }

  protected onConvertFileSelected(event: any) {
    const file = this.getTargetFile(event);
    this.convert(file, file.name);
  }

  private getTargetFile(event: any): File {
    return event.target.files[0];
  }

  private replaceExtension(fileName: string, extension: string): string {
    return fileName.split('.')[0] + `.${extension}`;
  }
}
