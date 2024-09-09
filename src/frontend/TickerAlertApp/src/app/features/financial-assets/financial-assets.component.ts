import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-financial-assets',
  standalone: true,
  imports: [],
  templateUrl: './financial-assets.component.html',
  styleUrl: './financial-assets.component.css'
})
export class FinancialAssetsComponent {
  private selectedAssetId = '';

  constructor(private params: ActivatedRoute) {
    this.getSymbolFromRoute();
  }

  private getSymbolFromRoute(): void {
    this.params.queryParams.subscribe(params => {
      this.selectedAssetId = params['id'];
      console.log('Asset ID:', this.selectedAssetId);
    })
  }

  public getTicker(): string {
    return this.selectedAssetId;
  }
}
