import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EventItem } from './model/event-item';
import { EventItemDetail } from './model/event-item-detail';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class EventsService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/events`;

  getEvents(): Observable<EventItem[]> {
    return this.http.get<EventItem[]>(this.apiUrl);
  }

  getEvent(id: string): Observable<EventItemDetail> {
    return this.http.get<EventItemDetail>(`${this.apiUrl}/${id}`);
  }

  addEvent(data: Partial<EventItem>): Observable<any> {
    return this.http.post(this.apiUrl, data);
  }

  updateEvent(id: string, patchData: any[]): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${id}`, patchData);
  }

  deleteEvent(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  joinEvent(eventId: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/${eventId}/join`, {});
  }

  leaveEvent(eventId: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/${eventId}/leave`, {});
  }

  getMyEvents(): Observable<EventItem[]> {
    return this.http.get<EventItem[]>(`${environment.apiUrl}/users/me/events`);
  }
}
