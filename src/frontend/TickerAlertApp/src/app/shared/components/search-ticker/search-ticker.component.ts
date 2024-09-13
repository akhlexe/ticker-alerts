import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { debounceTime, Observable, of, switchMap } from 'rxjs';
import { FinancialAssetsService } from '../../services/financial-asset/financial-assets.service';
import { FinancialAssetDto } from './../../services/financial-asset/models/financial-asset.model';

const MatModules = [
  MatFormFieldModule,
  MatInputModule,
  MatIconModule,
  MatButtonModule,
  MatAutocompleteModule]

@Component({
  selector: 'app-search-ticker',
  standalone: true,
  imports: [MatModules, CommonModule, ReactiveFormsModule],
  templateUrl: './search-ticker.component.html',
  styleUrl: './search-ticker.component.css'
})
export class SearchTickerComponent implements OnInit {
  @Output() tickerSelected = new EventEmitter<FinancialAssetDto>();
  @Input() initialValue: string | null = null;
  public assetSearchList$: Observable<FinancialAssetDto[]> | undefined;
  public tickerForm!: FormGroup;

  constructor(
    private assetsService: FinancialAssetsService,
    private formBuilder: FormBuilder) {
  }

  public ngOnInit(): void {
    this.tickerForm = this.formBuilder.group({
      ticker: [this.initialValue]
    });

    this.assetSearchList$ = this.tickerForm.get('ticker')?.valueChanges.pipe(
      debounceTime(300),
      switchMap(value => value?.length > 2 ? this.assetsService.getFinancialAssetsByCriteria(value) : of([]))
    );
  }

  public onTickerSelected(event: MatAutocompleteSelectedEvent): void {
    this.tickerSelected.emit(event.option.value);
  }

  // If the windows just opens when the user writes, the asset will be just pure string.
  // The option selected is typeof FinancialAssetDto: { ticker: 'MSFT' }
  public displayAssetTicker(asset: any): string {
    if (!!asset && !!asset.ticker) {
      // Do something
      return asset.ticker;
    }
    return asset;
  }

  public clearTickerInput(): void {
    this.tickerForm.controls['ticker'].setValue('');
  }
}
