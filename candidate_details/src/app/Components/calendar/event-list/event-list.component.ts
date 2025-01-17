import { CommonModule, DatePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { Calendar } from '../../../Models/calendar.model';
import { CalendarService } from '../../../Services/calendar.service';

@Component({
  selector: 'app-event-list',
  imports: [CommonModule, DatePipe],
  templateUrl: './event-list.component.html',
  styleUrl: './event-list.component.css',
})
export class EventListComponent {
  birthdayList: Calendar[] = [];
  holidayList: Calendar[] = [];
  service = inject(CalendarService);

  constructor() {
    this.getEventList();
  }

  getEventList() {
    this.service.getEventList().subscribe((res: any) => {
      if (res.bd) {
        this.birthdayList = res.bd;
        this.holidayList = res.hd;
      }
    });
  }
}
