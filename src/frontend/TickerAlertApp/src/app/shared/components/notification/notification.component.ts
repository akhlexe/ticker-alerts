import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../../../core/services/notification/notification.service';
import { SignalRService } from '../../../core/services/signal-r.service';
import { SoundService } from '../../../core/services/sound.service';

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [],
  template: '',
  styleUrl: './notification.component.css'
})
export class NotificationComponent implements OnInit {
  private messageTitle: string = 'Notification'
  private notificationSoundPath: string = 'assets/sounds/notification.mp3'

  constructor(
    private signalRService: SignalRService,
    private notificationService: NotificationService,
    private soundService: SoundService) { }

  ngOnInit(): void {
    this.signalRService.notification$.subscribe(notification => {
      if (notification) {
        this.showNotification(notification);
      }
    })
  }

  private showNotification(notification: string) {
    this.notificationService.showImportantNotification(notification, this.messageTitle);
    this.soundService.playSound(this.notificationSoundPath);
  }
}
