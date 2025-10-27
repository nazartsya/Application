import { EventItem } from './event-item';
import { ParticipantItem } from './participant-item';

export interface EventItemDetail extends EventItem {
  participants?: ParticipantItem[];
}
