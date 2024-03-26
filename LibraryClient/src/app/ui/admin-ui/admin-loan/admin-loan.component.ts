import { Component } from '@angular/core';
import { LoanService } from '../../../services/loan/loan.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerService } from '../../../services/spinner.service';

@Component({
  selector: 'app-admin-loan',
  standalone: true,
  imports: [],
  templateUrl: './admin-loan.component.html',
  styleUrl: './admin-loan.component.scss',
  host: {ngSkipHydration: 'true'}
})
export class AdminLoanComponent {
  public loans : any = [];

  constructor(private loanService : LoanService,private spinnerService : SpinnerService){}

  async ngOnInit(){
    await this.spinnerService.requestWithSpinner(async () => {
      this.loans = await this.loanService.getLoans();
    });
  }

  async getLoansByUsername(username:string){
    
    this.spinnerService.requestWithSpinner(async () => {
      this.loans = await this.loanService.getLoansByUsername(username);
    });
  }

  async giveBack(index:number){
    this.spinnerService.requestWithSpinner(async () => {
      var loan = this.loans[index];
      await this.loanService.giveBack(loan.id);
      this.loans = await this.loanService.getLoans();
    });
    
  }
}
