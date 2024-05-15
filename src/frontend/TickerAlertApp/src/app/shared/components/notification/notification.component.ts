import { Component, OnInit } from '@angular/core';
import { SignalRService } from '../../../core/services/signal-r.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [],
  template: '',
  styleUrl: './notification.component.css'
})
export class NotificationComponent implements OnInit {
  private messageTitle: string = 'Notification'
  constructor(private signalRService: SignalRService, private toastrService: ToastrService) { }

  ngOnInit(): void {
    this.signalRService.notification$.subscribe(message => {
      if (message) {
        this.toastrService.show(message, this.messageTitle);
      }
    })
  }
}
