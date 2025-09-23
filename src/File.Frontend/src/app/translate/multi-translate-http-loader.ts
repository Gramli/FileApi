import { Observable, forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { TranslateLoader } from '@ngx-translate/core';

export class MultiTranslateHttpLoader implements TranslateLoader {
  constructor(
    private http: HttpClient,
    public resources: { prefix: string; suffix: string }[]
  ) {}

  public getTranslation(lang: string): Observable<any> {
    return forkJoin(
      this.resources.map(config =>
        this.http.get(`${config.prefix}${lang}${config.suffix}`)
      )
    ).pipe(
      map(response => response.reduce((acc, obj) => ({ ...acc, ...obj }), {}))
    );
  }
}