import { Component, OnInit } from '@angular/core';
import { FileService } from './file.service';
import { IFile } from './app.model';
import { faUpload, faFileImport } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  faUpload = faUpload;
  faFileImport = faFileImport;

  loading: boolean = false;
  data: IFile[] | undefined;
  constructor(private fileService: FileService) { }

  ngOnInit(): void {
    this.loadFileData();
  }

  private loadFileData() {
    this.loading = true;
    this.fileService.getFiles().subscribe({
      next: (response) => {
        this.data = response.data;
      },
      error: (error) => {
      }
    }).add(() => {
      this.loading = false
    });
  }

  onDownloadFile(id: number) {

  }

  onDownloadFileAsJson(id: number) {

  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];

    if (file) {
      const formData = new FormData();
      formData.append("thumbnail", file);

      const upload$ = this.fileService.uploadFile(formData);

      this.loading = true;
      upload$.subscribe({
        next: () => {
          this.loadFileData();
        },
        error: (error: any) => {
        },
      }).add(() => {
        this.loading = false;
      });

    }
  }
}
