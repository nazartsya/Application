import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { EventsService } from '../events.service';

@Component({
  selector: 'app-event-add',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './event-add.html',
  styleUrl: './event-add.css',
})
export class EventAdd {
  private fb = inject(FormBuilder);
  private events = inject(EventsService);
  private router = inject(Router);

  form = this.fb.nonNullable.group({
    title: ['', Validators.required],
    description: ['', [Validators.required, Validators.maxLength(1500)]],
    date: ['', Validators.required],
    time: ['', Validators.required],
    location: ['', Validators.required],
    capacity: [undefined as number | undefined, Validators.min(1)],
    visibility: ['public', Validators.required],
  });

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
    if (this.form.valid) {
      const { date, time, ...rest } = this.form.value;

      const dateTime = new Date(`${date}T${time}`);
      const now = new Date();
      if (dateTime < now) {
        alert('Cannot create event in the past.');
        return;
      }

      const formValue = { ...rest, date: dateTime.toISOString() };

      this.events.addEvent(formValue).subscribe((createdEvent) => {
        this.router.navigate(['/events', createdEvent.id]);
      });
    } else {
      this.form.markAllAsTouched();
    }
  }
}
