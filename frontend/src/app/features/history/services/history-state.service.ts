import { Injectable, inject } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HistoryService } from './history.service';
import { HistoryRecord } from '../models/history.model';

@Injectable({
  providedIn: 'root'
})
export class HistoryStateService {
  private historyService = inject(HistoryService);

  private historySubject = new BehaviorSubject<HistoryRecord[]>([]);
  private loadingSubject = new BehaviorSubject<boolean>(false);
  private errorSubject = new BehaviorSubject<string | null>(null);

  history$ = this.historySubject.asObservable();
  loading$ = this.loadingSubject.asObservable();
  error$ = this.errorSubject.asObservable();

  loadAllHistory(): void {
    this.loadingSubject.next(true);
    this.errorSubject.next(null);

    this.historyService.getAllHistory().subscribe({
      next: (records) => {
        this.historySubject.next(records || []);
        this.loadingSubject.next(false);
      },
      error: (err) => {
        this.loadingSubject.next(false);
        this.errorSubject.next(this.extractErrorMessage(err, 'Failed to load transfer history.'));
      }
    });
  }

  private extractErrorMessage(err: any, fallback: string): string {
    if (!err) return fallback;
    if (typeof err === 'string') return err;
    if (typeof err.error === 'string') return err.error;
    if (typeof err.error?.message === 'string') return err.error.message;
    if (typeof err.error?.detail === 'string') return err.error.detail;
    if (typeof err.message === 'string') return err.message;
    return fallback;
  }
}
