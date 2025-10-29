import { AsyncPipe, CommonModule, DatePipe } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { EventsService } from '../events.service';
import { BehaviorSubject, switchMap } from 'rxjs';
import { EventItem } from '../model/event-item';
import { Tag } from '../model/tag';

@Component({
  selector: 'app-event-list',
  imports: [CommonModule, AsyncPipe, RouterLink, DatePipe],
  templateUrl: './event-list.html',
  styleUrl: './event-list.css',
})
export class EventList implements OnInit {
  private eventsService = inject(EventsService);
  private refresh$ = new BehaviorSubject<void>(undefined);

  availableTags: Tag[] = [];
  selectedTags: Tag[] = [];

  events$ = this.refresh$.pipe(switchMap(() => this.loadEvents()));

  ngOnInit(): void {
    this.eventsService.getTags().subscribe((tags) => (this.availableTags = tags));
  }

  private loadEvents() {
    const tagNames = this.selectedTags.map((t) => t.name);
    return this.eventsService.getEvents(tagNames);
  }

  toggleFilterTag(tag: Tag) {
    const index = this.selectedTags.findIndex((t) => t.id === tag.id);
    if (index > -1) {
      this.selectedTags.splice(index, 1);
    } else {
      this.selectedTags.push(tag);
    }
    this.refresh$.next();
  }

  isTagSelected(tag: Tag): boolean {
    return this.selectedTags.some((t) => t.id === tag.id);
  }

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
