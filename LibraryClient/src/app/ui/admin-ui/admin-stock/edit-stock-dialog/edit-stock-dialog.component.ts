import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-edit-stock-dialog',
  standalone: true,
  imports: [MatDialogModule,ReactiveFormsModule],
  templateUrl: './edit-stock-dialog.component.html',
  styleUrl: './edit-stock-dialog.component.scss'
})
export class EditStockDialogComponent {
  constructor(private formBuilder : FormBuilder){}

  public stockForm = this.formBuilder.group({
    totalQuantity:[0],
    currentQuantity:[0]
  });
}
