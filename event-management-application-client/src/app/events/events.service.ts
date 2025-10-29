import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EventItem } from './model/event-item';
import { EventItemDetail } from './model/event-item-detail';
import { environment } from '../../environments/environment';
import { Tag } from './model/tag';

@Injectable({
  providedIn: 'root',
})
export class EventsService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/events`;

  getEvents(tagNames: string[] = []): Observable<EventItem[]> {
    let params = new HttpParams();
    if (tagNames.length) {
      tagNames.forEach((t) => (params = params.append('tags', t)));
    }
    return this.http.get<EventItem[]>(this.apiUrl, { params });
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

  getTags(): Observable<Tag[]> {
    return this.http.get<Tag[]>(`${environment.apiUrl}/tags`);
  }
}
