import { Component } from '@angular/core';
import { StockService } from '../../../services/stock/stock.service';
import { book } from '../../../models/book';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerService } from '../../../services/spinner.service';

@Component({
  selector: 'app-book-list',
  standalone: true,
  imports: [],
  templateUrl: './book-list.component.html',
  styleUrl: './book-list.component.scss'
})
export class BookListComponent {
  public books : book[] = [];
  constructor(private stockService:StockService,private spinnerSevice : SpinnerService){}

  async ngOnInit(){
    this.spinnerSevice.requestWithSpinner(async () => {
      this.books = (await this.stockService.getStocks()).stocks;
    });
  }
}
