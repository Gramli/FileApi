export interface IDataResponse<T> {
  data: T;
  errors: string[];
}

export interface IFile {
  id: number;
  name: string;
  fileName: string;
  contentType: string;
}

export interface IBase64File {
  fileName: string;
  contentType: string;
  length: string;
  data: string;
}
