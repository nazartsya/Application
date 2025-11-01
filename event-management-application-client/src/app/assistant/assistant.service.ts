import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { AskResponse } from './ask-response';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AssistantService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/assistant/ask`;

  ask(question: string): Observable<AskResponse> {
    return this.http.post<AskResponse>(this.apiUrl, { question });
  }
}
