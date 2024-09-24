
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
