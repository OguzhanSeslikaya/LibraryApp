import { Component } from '@angular/core';
import { MatDialogModule } from '@angular/material/dialog';
import { StockService } from '../../../../services/stock/stock.service';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-create-stock-dialog',
  standalone: true,
  imports: [MatDialogModule,ReactiveFormsModule],
  templateUrl: './create-stock-dialog.component.html',
  styleUrl: './create-stock-dialog.component.scss'
})
export class CreateStockDialogComponent {

  constructor(private formBuilder : FormBuilder){}

  public stockForm = this.formBuilder.group({
    bookname:[''],
    stockQuantity:[5]
  });

}
