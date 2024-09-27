import { provideHttpClient } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { BrowserModule } from "@angular/platform-browser";
import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { NgbActiveModal, NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { SelectExtensionComponent } from "./components/select-extension.component";


@NgModule({
    declarations: [
        AppComponent,
        SelectExtensionComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        FontAwesomeModule,
        NgbModule,
        FormsModule,
    ],
    providers: [provideHttpClient(), NgbActiveModal],
    bootstrap: [AppComponent]
})
export class AppModule {
}
