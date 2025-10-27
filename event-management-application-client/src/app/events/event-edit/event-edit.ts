import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { EventsService } from '../events.service';
import { switchMap, tap } from 'rxjs';
import { EventItem } from '../model/event-item';

@Component({
  selector: 'app-event-edit',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './event-edit.html',
  styleUrl: './event-edit.css',
})
export class EventEdit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private events = inject(EventsService);
  private router = inject(Router);

  eventId!: string;

  form = this.fb.nonNullable.group({
    title: ['', Validators.required],
    description: ['', [Validators.required, Validators.maxLength(1500)]],
    date: ['', Validators.required],
    time: ['', Validators.required],
    location: ['', Validators.required],
    capacity: [undefined as number | undefined, Validators.min(1)],
    visibility: ['public', Validators.required],
  });

  ngOnInit() {
    this.route.paramMap
      .pipe(
        switchMap((params) => {
          this.eventId = params.get('id')!;
          return this.events.getEvent(this.eventId);
        }),
        tap((event: EventItem) => {
          const utcDate = new Date(event.date);
          const local = new Date(utcDate.getTime() - utcDate.getTimezoneOffset() * 60000);
          const date = local.toISOString().slice(0, 10);
          const time = local.toISOString().slice(11, 16);

          this.form.patchValue({
            title: event.title,
            description: event.description,
            date,
            time,
            location: event.location,
            capacity: event.capacity,
            visibility: event.isVisible ? 'public' : 'private',
          });
        })
      )
      .subscribe();
  }

  isInvalid(controlName: string, errorType?: string): boolean {
    const control = this.form.get(controlName);
    if (!control) return false;

    const invalid = control.invalid && (control.touched || control.dirty);
    if (errorType) {
      return !!(control.hasError(errorType) && (control.touched || control.dirty));
    }
    return invalid;
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const { date, time, visibility, ...rest } = this.form.value;
    const dateTime = new Date(`${date}T${time}`);
    const now = new Date();
    if (dateTime < now) {
      alert('Cannot set event date in the past.');
      return;
    }

    const updatedData = {
      ...rest,
      date: dateTime.toISOString(),
      isVisible: visibility === 'public',
    };

    const patchData = Object.entries(updatedData)
      .filter(([_, v]) => v !== undefined)
      .map(([k, v]) => ({ op: 'replace', path: `/${k}`, value: v }));

    this.events.updateEvent(this.eventId, patchData).subscribe(() => {
      this.router.navigate(['/events', this.eventId]);
    });
  }
}
