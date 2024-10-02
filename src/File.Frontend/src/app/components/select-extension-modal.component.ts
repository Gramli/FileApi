import { Component, Input } from '@angular/core';
import { NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-select-extension-modal',
  templateUrl: './select-extension-modal.component.html',
})
export class SelectExtensionModalComponent {
  @Input({ required: true }) modalRef!: NgbModalRef;

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
    this.modalRef.close(this.selectedExtension);
  }
}
