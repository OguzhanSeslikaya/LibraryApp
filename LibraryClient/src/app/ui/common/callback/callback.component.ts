import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import * as oidc from 'oidc-client'

@Component({
  selector: 'app-callback',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './callback.component.html',
  styleUrl: './callback.component.scss'
})
export class CallbackComponent {
    constructor(private router: Router){}

    ngOnInit(){
      this.router.navigateByUrl("");
    }
  }
