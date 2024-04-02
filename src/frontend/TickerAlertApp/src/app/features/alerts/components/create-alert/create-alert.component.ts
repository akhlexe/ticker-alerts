import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { CreateAlertRequest } from './models/create-alert.models';
import { AlertsService } from '../../services/alerts.service';

@Component({
  selector: 'app-create-alert',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    CommonModule,
    MatButtonModule,
  ],
  templateUrl: './create-alert.component.html',
  styleUrl: './create-alert.component.css',
  providers: [AlertsService],
})
export class CreateAlertComponent implements OnInit {
  createAlertForm!: FormGroup;
  constructor(
    private formBuilder: FormBuilder,
    private alertsService: AlertsService
  ) {}

  ngOnInit(): void {
    this.createAlertForm = this.formBuilder.group({
      ticker: ['', [Validators.required]],
      targetPrice: ['', [Validators.required]],
    });
  }

  public onSubmit() {
    const formValues = this.createAlertForm.value;

    const request: CreateAlertRequest = {
      ticker: formValues.ticker,
      targetPrice: formValues.targetPrice,
    };

    this.alertsService.createAlert(request).subscribe((result) => {
      if (result.success) {
        this.createAlertForm.reset();

        // disparo evento para avisar que se creo nueva alerta para actualizar la tabla.
      } else {
        console.log(result.errors);
      }
    });
  }
  focusFirstInput() {
    throw new Error('Method not implemented.');
  }

  public clearTickerInput() {
    this.createAlertForm.controls['ticker'].setValue('');
  }

  public clearPriceInput() {
    this.createAlertForm.controls['targetPrice'].setValue('');
  }
}
