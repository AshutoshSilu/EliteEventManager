import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { HeaderComponent } from '@shared/components/header/header.component';
import { FooterComponent } from '@shared/components/footer/footer.component';

@Component({
  selector: 'app-about',
  standalone: true,
  imports: [RouterLink, HeaderComponent, FooterComponent],
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.scss']
})
export class AboutComponent {}
