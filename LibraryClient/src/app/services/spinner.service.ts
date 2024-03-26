import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class SpinnerService {

  constructor(private spinner : NgxSpinnerService) { }

  async requestWithSpinner(method : () => Promise<void>){
    this.spinner.show("spinner");
    await method();
    setTimeout(() => {
    this.spinner.hide("spinner");
    }, 500)
  }
}
