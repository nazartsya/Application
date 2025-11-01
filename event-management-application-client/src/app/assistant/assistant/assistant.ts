import { Component, inject } from '@angular/core';
import { AssistantService } from '../assistant.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-assistant',
  imports: [CommonModule, FormsModule],
  templateUrl: './assistant.html',
  styleUrl: './assistant.css',
})
export class Assistant {
  private assistant = inject(AssistantService);
  question = '';
  answer = '';
  loading = false;

  ask() {
    if (!this.question.trim()) return;
    this.loading = true;
    this.answer = '';
    this.assistant.ask(this.question).subscribe({
      next: (res) => {
        this.answer = res.answer;
        this.loading = false;
      },
      error: () => {
        this.answer = 'Error contacting AI service.';
        this.loading = false;
      },
    });
  }
}
