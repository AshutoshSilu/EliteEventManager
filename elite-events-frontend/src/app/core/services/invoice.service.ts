import { Injectable } from '@angular/core';
import { jsPDF } from 'jspdf';
import autoTable from 'jspdf-autotable';
import { Booking } from '../models/booking.model';

@Injectable({ providedIn: 'root' })
export class InvoiceService {

  generateInvoice(booking: Booking): void {
    const doc = new jsPDF();
    const pageWidth = doc.internal.pageSize.getWidth();

    // --- Header ---
    doc.setFontSize(22);
    doc.setFont('helvetica', 'bold');
    doc.text('INVOICE', pageWidth / 2, 25, { align: 'center' });

    // Company name
    doc.setFontSize(12);
    doc.setFont('helvetica', 'normal');
    doc.text('Elite Event Manager', pageWidth / 2, 33, { align: 'center' });

    // Divider line
    doc.setDrawColor(200, 200, 200);
    doc.setLineWidth(0.5);
    doc.line(14, 38, pageWidth - 14, 38);

    // --- Invoice Info Section ---
    let y = 48;
    doc.setFontSize(10);
    doc.setFont('helvetica', 'bold');
    doc.text('Invoice Details', 14, y);
    doc.text('Customer Details', pageWidth / 2 + 10, y);

    y += 8;
    doc.setFont('helvetica', 'normal');
    doc.text(`Invoice No: ${booking.bookingNumber}`, 14, y);
    doc.text(`Name: ${booking.customerName}`, pageWidth / 2 + 10, y);

    y += 6;
    doc.text(`Date: ${this.formatDate(booking.createdAt)}`, 14, y);
    doc.text(`Email: ${booking.customerEmail}`, pageWidth / 2 + 10, y);

    y += 6;
    doc.text(`Event Date: ${this.formatDate(booking.eventDate)}`, 14, y);
    doc.text(`Guests: ${booking.guestCount}`, pageWidth / 2 + 10, y);

    y += 6;
    doc.text(`Status: ${booking.status}`, 14, y);

    // --- Event Info ---
    y += 12;
    doc.setFont('helvetica', 'bold');
    doc.text('Event Information', 14, y);
    y += 7;
    doc.setFont('helvetica', 'normal');
    if (booking.eventTitle) {
      doc.text(`Event: ${booking.eventTitle}`, 14, y);
      y += 6;
    }
    if (booking.venueName) {
      doc.text(`Venue: ${booking.venueName}`, 14, y);
      y += 6;
    }
    if (booking.packageName) {
      doc.text(`Package: ${booking.packageName}`, 14, y);
      y += 6;
    }
    if (booking.specialRequests) {
      doc.text(`Special Requests: ${booking.specialRequests}`, 14, y);
      y += 6;
    }

    // --- Services Table ---
    if (booking.details?.length) {
      y += 6;
      doc.setFont('helvetica', 'bold');
      doc.text('Services', 14, y);
      y += 4;

      const tableData = booking.details.map(d => [
        d.serviceName,
        d.vendorName || '-',
        d.quantity.toString(),
        this.formatCurrency(d.unitPrice),
        this.formatCurrency(d.totalPrice)
      ]);

      autoTable(doc, {
        startY: y,
        head: [['Service', 'Vendor', 'Qty', 'Unit Price', 'Total']],
        body: tableData,
        theme: 'grid',
        headStyles: {
          fillColor: [63, 81, 181],
          textColor: 255,
          fontStyle: 'bold',
          fontSize: 9
        },
        bodyStyles: { fontSize: 9 },
        columnStyles: {
          0: { cellWidth: 50 },
          1: { cellWidth: 40 },
          2: { cellWidth: 20, halign: 'center' },
          3: { cellWidth: 35, halign: 'right' },
          4: { cellWidth: 35, halign: 'right' }
        },
        margin: { left: 14, right: 14 }
      });

      y = (doc as any).lastAutoTable.finalY + 10;
    } else {
      y += 10;
    }

    // --- Pricing Summary ---
    doc.setFont('helvetica', 'bold');
    doc.text('Payment Summary', 14, y);
    y += 8;

    const summaryX = pageWidth - 80;
    doc.setFont('helvetica', 'normal');
    doc.text('Subtotal:', summaryX, y);
    doc.text(this.formatCurrency(booking.subTotal), pageWidth - 16, y, { align: 'right' });
    y += 7;

    doc.text('Tax:', summaryX, y);
    doc.text(this.formatCurrency(booking.taxAmount), pageWidth - 16, y, { align: 'right' });
    y += 7;

    doc.text('Discount:', summaryX, y);
    doc.text(`-${this.formatCurrency(booking.discountAmount)}`, pageWidth - 16, y, { align: 'right' });
    y += 2;

    doc.setDrawColor(100, 100, 100);
    doc.line(summaryX, y, pageWidth - 14, y);
    y += 7;

    doc.setFont('helvetica', 'bold');
    doc.setFontSize(12);
    doc.text('Total:', summaryX, y);
    doc.text(this.formatCurrency(booking.totalAmount), pageWidth - 16, y, { align: 'right' });

    // --- Footer ---
    const footerY = doc.internal.pageSize.getHeight() - 20;
    doc.setFontSize(8);
    doc.setFont('helvetica', 'normal');
    doc.setTextColor(130, 130, 130);
    doc.text('Thank you for choosing Elite Event Manager!', pageWidth / 2, footerY, { align: 'center' });
    doc.text(`Generated on ${new Date().toLocaleDateString()}`, pageWidth / 2, footerY + 5, { align: 'center' });

    // Save PDF
    doc.save(`Invoice_${booking.bookingNumber}.pdf`);
  }

  private formatDate(date: string | Date): string {
    return new Date(date).toLocaleDateString('en-IN', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }

  private formatCurrency(amount: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR',
      minimumFractionDigits: 2
    }).format(amount);
  }
}
