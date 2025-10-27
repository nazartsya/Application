import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { EventsService } from '../events.service';
import { AuthService } from '../../auth/auth.service';
import { BehaviorSubject, switchMap } from 'rxjs';
import { EventItemDetail } from '../model/event-item-detail';

@Component({
  selector: 'app-event-details',
  imports: [CommonModule, RouterLink],
  templateUrl: './event-details.html',
  styleUrl: './event-details.css',
})
export class EventDetails {
  private route = inject(ActivatedRoute);
  private service = inject(EventsService);
  private router = inject(Router);
  private auth = inject(AuthService);
  userId = this.auth.getUserId();

  private refresh$ = new BehaviorSubject<void>(undefined);

  event$ = this.refresh$.pipe(
    switchMap(() =>
      this.route.paramMap.pipe(switchMap((params) => this.service.getEvent(params.get('id')!)))
    )
  );

  join(event: EventItemDetail) {
    this.service.joinEvent(event.id).subscribe(() => this.refresh$.next());
  }

  leave(event: EventItemDetail) {
    this.service.leaveEvent(event.id).subscribe(() => this.refresh$.next());
  }

  onDelete(id: string) {
    if (confirm('Are you sure you want to delete this event?')) {
      this.service.deleteEvent(id).subscribe({
        next: () => this.router.navigate(['/events']),
        error: (err) => console.error('Delete failed:', err),
      });
    }
  }
}
