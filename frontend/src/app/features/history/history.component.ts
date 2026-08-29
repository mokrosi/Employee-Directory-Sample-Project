import { Component, inject, OnInit } from '@angular/core';
import { CommonModule, AsyncPipe } from '@angular/common';
import { HistoryStateService } from './services/history-state.service';
import { HistoryListComponent } from './components/history-list/history-list.component';

@Component({
  selector: 'app-history',
  standalone: true,
  imports: [CommonModule, AsyncPipe, HistoryListComponent],
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent implements OnInit {
  public stateService = inject(HistoryStateService);

  ngOnInit(): void {
    this.stateService.loadAllHistory();
  }

  onRefresh(): void {
    this.stateService.loadAllHistory();
  }
}
