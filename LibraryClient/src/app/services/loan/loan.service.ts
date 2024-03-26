import { Injectable } from '@angular/core';
import { HttpClientService } from '../http-client.service';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoanService {

  constructor(private httpClient : HttpClientService) { }

  async createLoan(createLoanModel : any){
    const data = lastValueFrom(this.httpClient.post("/loan/createLoan",createLoanModel));
    await data.then();
    return data;
  }

  async getMyLoans(){
    const data = lastValueFrom(this.httpClient.get("/loan/getMyLoans"));
    await data.then();
    return data;
  }

  async getLoans(){
    const data = lastValueFrom(this.httpClient.get("/loan/admin/getLoans"));
    await data.then();
    return data;
  }

  async getLoansByUsername(userName:string){
    const data = lastValueFrom(this.httpClient.get(`/loan/admin/getLoansByUsername?username=${userName}`));
    await data.then();
    return data;
  }

  async giveBack(loanId : string){
    const data = lastValueFrom(this.httpClient.post(`/loan/admin/giveBack?loanId=${loanId}`,{}));
    await data.then();
    return data;
  }

}
