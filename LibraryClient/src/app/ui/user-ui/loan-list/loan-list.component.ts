import { Component } from '@angular/core';
import { LoanService } from '../../../services/loan/loan.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerService } from '../../../services/spinner.service';

@Component({
  selector: 'app-loan-list',
  standalone: true,
  imports: [],
  templateUrl: './loan-list.component.html',
  styleUrl: './loan-list.component.scss'
})
export class LoanListComponent {
  public loans : any = [];

  constructor(private loanService : LoanService,private spinnerService : SpinnerService){}
  async ngOnInit(){
    this.spinnerService.requestWithSpinner(async () => {
      this.loans = await this.loanService.getMyLoans();
    });
  }

}
