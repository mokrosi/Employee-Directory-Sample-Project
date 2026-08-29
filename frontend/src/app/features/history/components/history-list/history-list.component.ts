import { Component, Input } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { HistoryRecord } from '../../models/history.model';

@Component({
  selector: 'app-history-list',
  standalone: true,
  imports: [CommonModule, DatePipe],
  templateUrl: './history-list.component.html',
  styleUrls: ['./history-list.component.css']
})
export class HistoryListComponent {
  @Input() histories: HistoryRecord[] = [];
  @Input() isLoading: boolean = false;
}
