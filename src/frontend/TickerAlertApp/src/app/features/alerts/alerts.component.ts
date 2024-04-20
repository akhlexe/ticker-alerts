import { Component, ViewChild } from '@angular/core';
import { AlertsTableComponent } from './components/alerts-table/alerts-table.component';
import { CreateAlertComponent } from './components/create-alert/create-alert.component';

const featureComponents = [AlertsTableComponent, CreateAlertComponent];

@Component({
  selector: 'app-alerts',
  standalone: true,
  imports: [featureComponents],
  templateUrl: './alerts.component.html',
  styleUrl: './alerts.component.css',
  providers: [],
})
export class AlertsComponent {
  @ViewChild(AlertsTableComponent) alertsTable!: AlertsTableComponent;

  onAlertCreated() {
    this.alertsTable.getData();
  }
}
