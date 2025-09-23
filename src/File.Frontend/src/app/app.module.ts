import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClient, provideHttpClient } from '@angular/common/http';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import { SelectExtensionModalComponent } from './components/select-extension-modal.component';
import { NotifierModule } from 'gramli-angular-notifier';
import { MultiTranslateHttpLoader } from './translate/multi-translate-http-loader';
import {
  TranslateLoader,
  TranslateModule,
} from '@ngx-translate/core';

export function HttpLoaderFactory(http: HttpClient): TranslateLoader {
  return new MultiTranslateHttpLoader(http, [
    { prefix: './assets/i18n/', suffix: '.json' },
    { prefix: './assets/i18n/components/', suffix: '.json' },
  ]);
}

@NgModule({
  declarations: [AppComponent, SelectExtensionModalComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FontAwesomeModule,
    NgbModule,
    FormsModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient],
      },
      lang: 'en',
      fallbackLang: 'en'
    }),
    NotifierModule.withConfig({
      position: {
        horizontal: {
          position: 'right',
        },
      },
    }),
  ],
  providers: [
    provideHttpClient()
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
