import { Injectable } from '@angular/core';

@Injectable()
  export class FileApiService {
    public loading: boolean = false;

    public get filesInfo(){
        return this.filesInfo$.asObservable();
    }

    private filesInfo$ = new Subject<IFile[]>();

    

  }