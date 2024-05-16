import { Component, OnInit } from '@angular/core';
import { SignalRService } from '../../../core/services/signal-r.service';
import { ToastrService } from 'ngx-toastr';
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
    private toastrService: ToastrService,
    private soundService: SoundService) { }

  ngOnInit(): void {
    this.signalRService.notification$.subscribe(notification => {
      if (notification) {
        this.showNotification(notification);
      }
    })
  }

  private showNotification(notification: string) {
    this.toastrService.success(notification, this.messageTitle, {
      disableTimeOut: true
    });

    this.soundService.playSound(this.notificationSoundPath);
  }
}
