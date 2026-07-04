import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-my-reviews',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './my-reviews.component.html',
  styleUrls: ['./my-reviews.component.scss']
})
export class MyReviewsComponent {}
