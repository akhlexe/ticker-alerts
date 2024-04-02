import { Component, OnInit } from '@angular/core';
import { Alert } from './models/alert.model';
import { MatTableModule } from '@angular/material/table';
import { AlertsService } from './services/alerts.service';

@Component({
  selector: 'app-alerts',
  standalone: true,
  imports: [MatTableModule],
  templateUrl: './alerts.component.html',
  styleUrl: './alerts.component.css',
  providers: [AlertsService],
})
export class AlertsComponent implements OnInit {
  public displayedColumns: string[] = [
    'tickerName',
    'targetPrice',
    'actualPrice',
    'difference',
    'state',
  ];
  public alerts: Alert[] = [];

  constructor(private alertsService: AlertsService) {}

  ngOnInit(): void {
    this.alertsService.getAlerts().subscribe((result) => {
      this.alerts = result;
    });

    // this.alerts = [
    //   {
    //     tickerName: 'BTC',
    //     targetPrice: 70000,
    //     actualPrice: 60000,
    //     difference: 10000,
    //     state: 'PENDING',
    //   },
    //   {
    //     tickerName: 'ETH',
    //     targetPrice: 3800,
    //     actualPrice: 3650,
    //     difference: 150,
    //     state: 'PENDING',
    //   },
    // ];
  }
}
