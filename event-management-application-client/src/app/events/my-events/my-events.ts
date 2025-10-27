import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { EventsService } from '../events.service';
import { BehaviorSubject, switchMap } from 'rxjs';
import { FullCalendarModule } from '@fullcalendar/angular';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
import { CalendarOptions } from '@fullcalendar/core';

@Component({
  selector: 'app-my-events',
  imports: [CommonModule, FullCalendarModule],
  templateUrl: './my-events.html',
  styleUrl: './my-events.css',
})
export class MyEvents {
  private eventsService = inject(EventsService);
  private refresh$ = new BehaviorSubject<void>(undefined);

  events$ = this.refresh$.pipe(switchMap(() => this.eventsService.getMyEvents()));

  calendarOptions: CalendarOptions = {
    plugins: [dayGridPlugin, timeGridPlugin, interactionPlugin],
    initialView: 'dayGridMonth',
    headerToolbar: {
      left: 'prev,next',
      center: 'title',
      right: 'dayGridMonth,timeGridWeek',
    },
    height: 650,
    events: [],
    eventColor: '#6d5dfc',
    eventTextColor: '#fff',
    eventClick: (info) => {
      const eventId = info.event.id;
      window.location.href = `/events/${eventId}`;
    },
  };

  constructor() {
    this.events$.subscribe((events) => {
      this.calendarOptions.events = events.map((e) => ({
        id: e.id,
        title: e.title,
        start: e.date,
      }));
    });
  }
}
