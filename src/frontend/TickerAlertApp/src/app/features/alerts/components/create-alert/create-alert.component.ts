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
})
export class CreateAlertComponent implements OnInit {
  createAlertForm!: FormGroup;
  constructor(private formBuilder: FormBuilder) {}

  ngOnInit(): void {
    this.createAlertForm = this.formBuilder.group({
      ticker: [null, [Validators.required]],
      targetPrice: [null, [Validators.required]],
    });
  }

  public onSubmit() {
    console.log('Se submiteo el form');
    this.clearTickerInput();
    this.clearPriceInput();
  }

  public clearTickerInput() {
    this.createAlertForm.controls['ticker'].setValue('');
  }

  public clearPriceInput() {
    this.createAlertForm.controls['targetPrice'].setValue('');
  }
}
