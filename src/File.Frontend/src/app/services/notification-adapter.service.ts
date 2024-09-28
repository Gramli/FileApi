import { Injectable } from "@angular/core";
import { NotifierService } from "gramli-angular-notifier";

@Injectable({
    providedIn: 'root'
  })
export class NotificationAdapterService {
    constructor(private notifierService: NotifierService){}

    public showSuccess(message: string){
        this.notifierService.notify('success', message);
    }

    public showError(message: string){
        this.notifierService.notify('error', message);
    }
}