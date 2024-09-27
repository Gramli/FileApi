import { Component, ElementRef, Input, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { IFile } from './app.model';
import { faUpload, faFileImport } from '@fortawesome/free-solid-svg-icons';
import { saveAs } from "file-saver";
import { Subscription } from 'rxjs';
import { Buffer } from 'buffer';
import { FileService } from './services/file.service';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  faUpload = faUpload;
  faFileImport = faFileImport;

  @ViewChild('dialog') dialogRef!: TemplateRef<any>;

  loading: boolean = false;
  data: IFile[] | undefined;
  constructor(private fileService: FileService, private ngbModal: NgbModal) { }

  ngOnInit(): void {
    this.loadFileData();
  }

  private loadFileData() {
    this.runSubScriptionWithProgress(() =>
      this.fileService.getFiles().subscribe({
        next: (response) => {
          this.data = response.data;
        },
        error: (error) => {
          console.error(error);
        }
      }));
  }

  convert() {
    this.ngbModal.open(this.dialogRef, {
      windowClass: 'modal-job-scrollable'
    });
  }

  onDownloadFile(id: number) {
    this.runSubScriptionWithProgress(() =>
      this.fileService.downloadFile(id).subscribe({
        next: (response) => {
          const file = this.data?.filter((file) => file.id === id )[0];
          saveAs(response, file?.fileName);
        },
        error: (error: any) => {
          console.error(error);
        }
      }));
  }

  onDownloadFileAsJson(id: number) {
    this.runSubScriptionWithProgress(() =>
      this.fileService.downloadFileAsString(id).subscribe({
        next: (response) => {
          const file = this.data?.filter((file) => file.id === id)[0];
          const fileContent = Buffer.from(response.data.data, 'base64').toString('utf-8');
          saveAs(fileContent, file?.fileName);
        },
        error: (error: any) => {
          console.error(error);
        }
      }));
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];

    if (file) {
      const formData = new FormData();
      formData.append("thumbnail", file);

      const upload$ = this.fileService.uploadFile(formData);

      this.runSubScriptionWithProgress(() => upload$.subscribe({
        next: () => {
          this.loadFileData();
        },
        error: (error: any) => {
          console.error(error);
        },
      }));

    }
  }

  selectedExtension(extension: string) {
  }

  private runSubScriptionWithProgress(action: () => Subscription) {
    this.loading = true;
    action().add(() => { this.loading = false; });
  }
}
