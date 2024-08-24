import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ToastrConfigs } from './models/message-config.model';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(private toastr: ToastrService) { }

  public showInfo(message: string, title?: string) {
    this.toastr.info(message, title, ToastrConfigs.defaultConfig);
  }

  public showError(message: string, title?: string) {
    this.toastr.error(message, title, ToastrConfigs.defaultConfig);
  }

  public showWarning(message: string, title?: string) {
    this.toastr.warning(message, title, ToastrConfigs.defaultConfig);
  }

  public showSuccess(message: string, title?: string) {
    this.toastr.success(message, title, ToastrConfigs.defaultConfig);
  }

  public showImportantNotification(message: string, title?: string) {
    this.toastr.success(message, title, ToastrConfigs.importantConfig);
  }
}
