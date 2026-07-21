import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslatePipe } from '@ngx-translate/core';
import { UzayAccountService } from '../../core/services/uzay-account.service';
import { Account } from '../../core/models/account.model';
import { Transaction, TransactionType } from '../../core/models/transaction.model';

type OperationTab = 'deposit' | 'withdraw' | 'transfer';

@Component({
  selector: 'app-uzay-account-detail',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslatePipe],
  templateUrl: './uzay-account-detail.html',
  styleUrl: './uzay-account-detail.scss'
})
export class UzayAccountDetailComponent implements OnInit {
  private uzayAccountService = inject(UzayAccountService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  account: Account | null = null;
  transactions: Transaction[] = [];
  isLoading = true;

  TransactionType = TransactionType;

  // --- İşlem modalı ---
  showModal = false;
  activeTab: OperationTab = 'deposit';
  isSubmitting = false;

  amount: number | null = null;
  description = '';
  toIban = '';

  // Çeviri anahtarı tutar
  message = '';
  messageType: 'success' | 'error' = 'success';

  ngOnInit(): void {
    const accountId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadAccount(accountId);
    this.loadTransactions(accountId);
  }

  loadAccount(accountId: number): void {
    this.uzayAccountService.getMyAccounts().subscribe({
      next: (accounts) => {
        this.account = accounts.find(a => a.id === accountId) ?? null;
      }
    });
  }

  loadTransactions(accountId: number): void {
    this.isLoading = true;
    this.uzayAccountService.getTransactions(accountId).subscribe({
      next: (data) => {
        this.transactions = data;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  // --- Modal kontrolü ---

  openModal(tab: OperationTab): void {
    this.activeTab = tab;
    this.resetForm();
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
    this.resetForm();
  }

  /** Sekme değişince form temizlenir — yarım kalan veri diğer işleme taşınmasın */
  switchTab(tab: OperationTab): void {
    this.activeTab = tab;
    this.resetForm();
  }

  private resetForm(): void {
    this.amount = null;
    this.description = '';
    this.toIban = '';
    this.message = '';
  }

  submit(): void {
    if (!this.account) return;

    if (!this.amount || this.amount <= 0) {
      this.showMessage('ERRORS.INVALID_AMOUNT', 'error');
      return;
    }

    if (this.activeTab === 'transfer' && !this.toIban.trim()) {
      this.showMessage('ERRORS.RECIPIENT_NOT_FOUND', 'error');
      return;
    }

    this.isSubmitting = true;
    const accountId = this.account.id;

    const request$ =
      this.activeTab === 'deposit'
        ? this.uzayAccountService.deposit(accountId, this.amount, this.description)
        : this.activeTab === 'withdraw'
        ? this.uzayAccountService.withdraw(accountId, this.amount, this.description)
        : this.uzayAccountService.transfer(accountId, this.toIban, this.amount, this.description);

    request$.subscribe({
      next: () => {
        this.isSubmitting = false;
        this.showModal = false;
        this.resetForm();
        // Bakiye ve işlem listesi değişti — ikisini de tazele
        this.loadAccount(accountId);
        this.loadTransactions(accountId);
      },
      error: (err) => {
        this.isSubmitting = false;
        // Backend hata KODU dönüyor, ERRORS.<code> ile çeviriyoruz
        const code = err?.error?.code;
        this.showMessage(code ? `ERRORS.${code}` : 'COMMON.ERROR', 'error');
      }
    });
  }

  private showMessage(key: string, type: 'success' | 'error'): void {
    this.message = key;
    this.messageType = type;
  }

  getCurrencySymbol(currency: string): string {
    switch (currency) {
      case 'TL':
      case 'TRY': return '₺';
      case 'USD': return '$';
      case 'EUR': return '€';
      case 'GBP': return '£';
      default: return currency;
    }
  }

  goBack(): void {
    this.router.navigate(['/dashboard']);
  }
}