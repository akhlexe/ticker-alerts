import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  Output
} from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { FinancialAssetsService } from '../../../../shared/services/financial-asset/financial-assets.service';
import { AlertsService } from '../../services/alerts.service';
import { CreateAlertModalComponent } from '../create-alert-modal/create-alert-modal.component';

@Component({
  selector: 'app-create-alert',
  standalone: true,
  imports: [
    MatIconModule,
    CommonModule,
    MatButtonModule,
    CreateAlertModalComponent
  ],
  templateUrl: './create-alert.component.html',
  styleUrl: './create-alert.component.css',
  providers: [AlertsService, FinancialAssetsService],
})
export class CreateAlertComponent {

  @Output() alertCreated: EventEmitter<void> = new EventEmitter<void>();

  constructor(
    private dialog: MatDialog,
  ) { }

  public openCreateAlertModal() {
    this.dialog.open(CreateAlertModalComponent, {
      data: { defaultAsset: null }
    }).afterClosed().subscribe(isAlertCreated => {
      if (isAlertCreated) {
        this.alertCreated.emit();
      }
    });
  }
}
