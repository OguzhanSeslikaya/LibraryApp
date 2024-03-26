import { Injectable } from '@angular/core';
import { HttpClientService } from '../http-client.service';
import { lastValueFrom } from 'rxjs';
import { book } from '../../models/book';

@Injectable({
  providedIn: 'root'
})
export class StockService {
  constructor(private httpClient : HttpClientService) { }

  async getStocks(){
    const data = lastValueFrom(this.httpClient.get<{stocks:book[]}>("/stock/getstocks"));
    await data.then();
    return data;
  }

  async createStock(createStockVM:any){
    const data = lastValueFrom(this.httpClient.post("/stock/admin/createStock",createStockVM));
    await data.then();
    return data;
  }

  async getStocksForAdmin(){
    const data = lastValueFrom(this.httpClient.get<any>("/stock/admin/getstocks"));
    await data.then();
    return data;
  }

  async totalStockChange(quantity:number,stockId:string){
    const data = lastValueFrom(this.httpClient.post("/stock/admin/totalStockChange",{stockId:stockId,count:quantity}))
    await data.then();
    return data;
  }

  async currentStockChange(quantity:number,stockId:string){
    const data = lastValueFrom(this.httpClient.post("/stock/admin/currentStockChange",{stockId:stockId,count:quantity}))
    await data.then();
    return data;
  }
}
