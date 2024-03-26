import { Component } from '@angular/core';
import { StockService } from '../../../services/stock/stock.service';
import { MatDialog } from '@angular/material/dialog';
import { CreateStockDialogComponent } from './create-stock-dialog/create-stock-dialog.component';
import { EditStockDialogComponent } from './edit-stock-dialog/edit-stock-dialog.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerService } from '../../../services/spinner.service';

@Component({
  selector: 'app-admin-stock',
  standalone: true,
  imports: [],
  templateUrl: './admin-stock.component.html',
  styleUrl: './admin-stock.component.scss'
})
export class AdminStockComponent{
  public books: any[] = [];
  constructor(private stockService: StockService,private spinnerService:SpinnerService, private dialog: MatDialog) {}

  async ngOnInit() {
    this.spinnerService.requestWithSpinner(async () => {
      this.books = (await this.stockService.getStocksForAdmin()).stocks;
    });
  }

  async createStockDialog() {
    this.dialog.open(CreateStockDialogComponent).afterClosed().subscribe(async result => {
      if (result) {
        if (result.bookName && result.quantity && result.quantity > 0) {
          this.spinnerService.requestWithSpinner(async () => {
          await this.stockService.createStock(result);
          this.books = (await this.stockService.getStocksForAdmin()).stocks;
          });
        }
      }
    });
  }

  async editStockDialog(id: string) {
    this.dialog.open(EditStockDialogComponent).afterClosed().subscribe(async result => {
      if (result) {
        this.spinnerService.requestWithSpinner(async () => {
          if(result.totalQuantity){
            await this.stockService.totalStockChange(result.totalQuantity,id);
          }else if(result.currentQuantity){
            await this.stockService.currentStockChange(result.currentQuantity,id);
          }
          this.books = (await this.stockService.getStocksForAdmin()).stocks;
        });
      }
    });
  }
}
