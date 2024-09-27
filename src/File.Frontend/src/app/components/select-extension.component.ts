import { Component, EventEmitter, Input, Output } from "@angular/core";
import { NgbActiveModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: 'app-select-extension',
  templateUrl: './select-extension.component.html',
})
export class SelectExtensionComponent {
  @Input({ required: true }) modalRef!: NgbModalRef;
  @Output() onSelectedExtension: EventEmitter<string> = new EventEmitter();

  extensions: string[] = ['json', 'xml', 'yaml'];

  private _selectedExtension: string | undefined;
  set selectedExtension(value: string) {
    this._selectedExtension = value;
  }

  get selectedExtension() {
    if (!this._selectedExtension) {
      this._selectedExtension = this.extensions[0];
    }

    return this._selectedExtension;
  }

  protected submit() {
    this.modalRef.close();
    this.onSelectedExtension.emit(this.selectedExtension);
  }

}
