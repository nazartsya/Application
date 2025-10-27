export interface EventItem {
  id: string;
  title: string;
  description: string;
  date: string;
  location: string;
  capacity?: number;
  isVisible: boolean;
  isJoined?: boolean;
  participantsCount?: number;
  createdBy?: string;
}
