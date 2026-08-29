import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HistoryRecord } from '../models/history.model';

@Injectable({
  providedIn: 'root'
})
export class HistoryService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7214/api/history';

  getAllHistory(): Observable<HistoryRecord[]> {
    return this.http.get<HistoryRecord[]>(this.apiUrl);
  }
}
