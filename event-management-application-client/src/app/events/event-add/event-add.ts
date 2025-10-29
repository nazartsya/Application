import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { EventsService } from '../events.service';
import { Tag } from '../model/tag';
import { minMaxSelectedArray } from '../validations/min-max-selected-array';

@Component({
  selector: 'app-event-add',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './event-add.html',
  styleUrl: './event-add.css',
})
export class EventAdd implements OnInit {
  private fb = inject(FormBuilder);
  private events = inject(EventsService);
  private router = inject(Router);

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

  ngOnInit() {
    this.events.getTags().subscribe((tags) => {
      this.availableTags = tags;
    });
  }

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
      const { date, time, visibility, ...rest } = this.form.value;

      const dateTime = new Date(`${date}T${time}`);
      const now = new Date();
      if (dateTime < now) {
        alert('Cannot create event in the past.');
        return;
      }

      const formValue = {
        ...rest,
        isVisible: visibility === 'public',
        date: dateTime.toISOString(),
        tags: this.tagsFormArray.value.map((t: Tag) => ({ name: t.name })),
      };

      this.events.addEvent(formValue).subscribe((createdEvent) => {
        this.router.navigate(['/events', createdEvent.id]);
      });
    } else {
      this.form.markAllAsTouched();
    }
  }
}
