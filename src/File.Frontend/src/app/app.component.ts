import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { IFile } from './app.model';
import { faUpload, faFileImport } from '@fortawesome/free-solid-svg-icons';
import { saveAs } from "file-saver";
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FileLoadingService } from './services/file-loading.service';

// TRY https://www.npmjs.com/package/ngx-toastr

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
  constructor(protected fileService: FileLoadingService, private ngbModal: NgbModal) { }

  ngOnInit(): void {
    this.fileService.filesInfo.subscribe({
      next: (response) => {
        this.data = response;
      },
      error: (error) => {
        console.log(error);
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
      })
    });
  }

  protected export(id: number) {
    this.ngbModal.open(this.dialogRef, {
      windowClass: 'modal-job-scrollable'
    }).closed.subscribe((selectedExtension: string) => {
      this.fileService.exportFile(id, selectedExtension, (fileContent) => {
        this.saveFile(id, fileContent, selectedExtension);
      })
    });
  }

  protected onDownloadFile(id: number) {
    this.fileService.downloadFile(id, (fileContent) => {
      this.saveFile(id, fileContent);
    });
  }

  protected onDownloadFileAsJson(id: number) {
    this.fileService.downloadFileAsJson(id, (fileContent) => {
      this.saveFile(id, fileContent);
    });
  }

  private saveFile(id: number, fileContent: Blob | string, extension?: string) {
    const file = this.data?.filter((file) => file.id === id)[0];
    saveAs(fileContent, extension === undefined ? file?.fileName : this.replaceExtension(file!.fileName, extension));
  }

  protected onUploadFileSelected(event: any) {
    const file = this.getTargetFile(event);
    this.fileService.uploadFile(file);
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
