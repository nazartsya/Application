import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { EventsService } from '../events.service';
import { switchMap, tap } from 'rxjs';
import { EventItem } from '../model/event-item';
import { Tag } from '../model/tag';
import { minMaxSelectedArray } from '../validations/min-max-selected-array';

@Component({
  selector: 'app-event-edit',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './event-edit.html',
  styleUrl: './event-edit.css',
})
export class EventEdit implements OnInit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private events = inject(EventsService);
  private router = inject(Router);

  eventId!: string;
  availableTags: Tag[] = [];

  form = this.fb.nonNullable.group({
    title: ['', Validators.required],
    description: ['', [Validators.required, Validators.maxLength(1500)]],
    date: ['', Validators.required],
    time: ['', Validators.required],
    location: ['', Validators.required],
    capacity: [undefined as number | undefined, Validators.min(1)],
    visibility: ['public', Validators.required],
    tags: this.fb.array<Tag>([], minMaxSelectedArray(1, 5)),
  });

  get tagsFormArray(): FormArray {
    return this.form.get('tags') as FormArray;
  }

  toggleTag(tag: Tag) {
    const index = this.tagsFormArray.value.findIndex((t: Tag) => t.id === tag.id);
    if (index > -1) {
      this.tagsFormArray.removeAt(index);
    } else {
      this.tagsFormArray.push(this.fb.control(tag));
    }
    this.tagsFormArray.markAsTouched();
  }

  isTagSelected(tag: Tag): boolean {
    return this.tagsFormArray.value.some((t: Tag) => t.id === tag.id);
  }

  ngOnInit() {
    this.events.getTags().subscribe((tags) => {
      this.availableTags = tags;
    });

    this.route.paramMap
      .pipe(
        switchMap((params) => {
          this.eventId = params.get('id')!;
          return this.events.getEvent(this.eventId);
        }),
        tap((event: EventItem) => this.patchForm(event))
      )
      .subscribe();
  }

  private patchForm(event: EventItem) {
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

    this.tagsFormArray.clear();
    event.tags?.forEach((t) => this.tagsFormArray.push(this.fb.control(t)));
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
      tags: this.tagsFormArray.value.map((t: Tag) => ({ name: t.name })),
    };

    const patchData = Object.entries(updatedData)
      .filter(([_, v]) => v !== undefined)
      .map(([k, v]) => ({ op: 'replace', path: `/${k}`, value: v }));

    this.events.updateEvent(this.eventId, patchData).subscribe(() => {
      this.router.navigate(['/events', this.eventId]);
    });
  }
}
