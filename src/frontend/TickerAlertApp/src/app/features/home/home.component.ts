import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { SearchTickerComponent } from '../../shared/components/search-ticker/search-ticker.component';
import { FinancialAssetDto } from '../../shared/services/financial-asset/models/financial-asset.model';


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, SearchTickerComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  public onTickerSelected(tickerSelected: FinancialAssetDto) {
    console.log(tickerSelected);
  }
}
