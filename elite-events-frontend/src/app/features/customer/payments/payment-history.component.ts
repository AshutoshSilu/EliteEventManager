import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { API_ENDPOINTS } from '@core/constants/api-endpoints';
import { Payment } from '@core/models/payment.model';

@Component({
  selector: 'app-payment-history',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './payment-history.component.html',
  styleUrls: ['./payment-history.component.scss']
})
export class PaymentHistoryComponent implements OnInit {
  private http = inject(HttpClient);
  payments = signal<Payment[]>([]);

  ngOnInit(): void {
    this.http.get<any>(API_ENDPOINTS.payments.myPayments).subscribe(res => {
      if (res.success && res.data) this.payments.set(res.data);
    });
  }
}
