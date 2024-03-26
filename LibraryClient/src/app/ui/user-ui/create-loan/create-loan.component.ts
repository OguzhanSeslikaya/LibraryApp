import { Component } from '@angular/core';
import { LoanService } from '../../../services/loan/loan.service';
import { NgxSpinner, NgxSpinnerService } from 'ngx-spinner';
import { SpinnerService } from '../../../services/spinner.service';

@Component({
  selector: 'app-create-loan',
  standalone: true,
  imports: [],
  templateUrl: './create-loan.component.html',
  styleUrl: './create-loan.component.scss'
})
export class CreateLoanComponent {
  constructor(private loanService : LoanService,private spinnerService : SpinnerService){}
  ngOnInit(){
  }
  async yolla(bookId:any){
    this.spinnerService.requestWithSpinner(async () => {
          await this.loanService.createLoan({bookId:bookId});
    });
  }
}
