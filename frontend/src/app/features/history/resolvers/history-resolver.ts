import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { Observable } from 'rxjs';
import { HistoryRecord } from '../models/history.model';
import { HistoryService } from '../services/history.service';

export const historyResolver: ResolveFn<HistoryRecord[]> = (): Observable<HistoryRecord[]> => {
  const historyService = inject(HistoryService);
  return historyService.getAllHistory();
};
