import { Routes } from '@angular/router';
import { CallbackComponent } from './ui/common/callback/callback.component';
import { BookListComponent } from './ui/user-ui/book-list/book-list.component';
import { AdminUiComponent } from './ui/admin-ui/admin-ui.component';
import { AdminStockComponent } from './ui/admin-ui/admin-stock/admin-stock.component';
import { AdminLoanComponent } from './ui/admin-ui/admin-loan/admin-loan.component';
import { isAdminGuard } from './guards/is-admin.guard';
import { NotFoundComponent } from './ui/common/not-found/not-found.component';
import { CreateLoanComponent } from './ui/user-ui/create-loan/create-loan.component';
import { LoanListComponent } from './ui/user-ui/loan-list/loan-list.component';

export const routes: Routes = [
    {path:"",component:CreateLoanComponent},
    {path:"callback",component:CallbackComponent},
    {path:"book-list",component:BookListComponent},
    {path:"create-loan",component:CreateLoanComponent},
    {path:"loan-list",component:LoanListComponent},
    {path:"admin",component:AdminUiComponent,canActivate:[isAdminGuard],children:[
        {path:"",component:AdminStockComponent},
        {path:"stock",component:AdminStockComponent},
        {path:"loan",component:AdminLoanComponent}
    ]},
    {path:"**",component:NotFoundComponent}
];
