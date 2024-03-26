import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-admin-ui',
  standalone: true,
  imports: [RouterLink,RouterOutlet],
  templateUrl: './admin-ui.component.html',
  styleUrl: './admin-ui.component.scss'
})
export class AdminUiComponent {

}
