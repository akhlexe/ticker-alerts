import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatAutocompleteModule, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { FinancialAssetsService } from '../../services/financial-asset/financial-assets.service';
import { FinancialAssetDto } from '../../services/financial-asset/models/financial-asset.model';
import { BehaviorSubject, debounceTime, Observable, of, switchMap } from 'rxjs';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';

const MatModules = [ReactiveFormsModule,
  MatFormFieldModule,
  MatInputModule,
  MatIconModule,
  CommonModule,
  MatButtonModule,
  MatAutocompleteModule,]

@Component({
  selector: 'app-search-ticker',
  standalone: true,
  imports: [MatModules, CommonModule],
  templateUrl: './search-ticker.component.html',
  styleUrl: './search-ticker.component.css'
})
export class SearchTickerComponent implements OnInit {
  @Output() tickerSelected = new EventEmitter<FinancialAssetDto>();
  private searchControl = new BehaviorSubject<string | null>(null);
  public assetSearchList: Observable<FinancialAssetDto[]> | undefined;
  public tickerForm!: FormGroup;

  constructor(
    private assetsService: FinancialAssetsService,
    private formBuilder: FormBuilder) { }

  public ngOnInit(): void {
    this.tickerForm = this.formBuilder.group({
      ticker: [null, []]
    });

    this.tickerForm.get('ticker')?.valueChanges.subscribe(value => {
      this.searchControl.next(value);
    });

    this.assetSearchList = this.searchControl.pipe(
      debounceTime(300),
      switchMap((value) => {
        if (value === null || value.length <= 2) {
          return of([]);
        } else {
          return this.assetsService.getFinancialAssetsByCriteria(value);
        }
      })
    );
  }

  public onTickerSelected(event: MatAutocompleteSelectedEvent): void {
    this.tickerSelected.emit(event.option.value);
    console.log(event);
  }

  public displayAssetTicker(asset: FinancialAssetDto): string {
    return asset?.ticker && asset?.name
      ? asset?.ticker + ' - ' + asset?.name
      : '';
  }

  public clearTickerInput(): void {
    this.tickerForm.controls['ticker'].setValue(null);
    this.searchControl.next(null);
  }

}
