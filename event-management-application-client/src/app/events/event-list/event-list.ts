import { AsyncPipe, CommonModule, DatePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { EventsService } from '../events.service';
import { BehaviorSubject, switchMap } from 'rxjs';
import { EventItem } from '../model/event-item';

@Component({
  selector: 'app-event-list',
  imports: [CommonModule, AsyncPipe, RouterLink, DatePipe],
  templateUrl: './event-list.html',
  styleUrl: './event-list.css',
})
export class EventList {
  private eventsService = inject(EventsService);
  private refresh$ = new BehaviorSubject<void>(undefined);

  events$ = this.refresh$.pipe(switchMap(() => this.eventsService.getEvents()));

  join(event: EventItem) {
    this.eventsService.joinEvent(event.id).subscribe(() => {
      event.isJoined = true;
      event.participantsCount = (event.participantsCount ?? 0) + 1;
    });
  }

  leave(event: EventItem) {
    this.eventsService.leaveEvent(event.id).subscribe(() => {
      event.isJoined = false;
      event.participantsCount = Math.max((event.participantsCount ?? 1) - 1, 0);
    });
  }
}
