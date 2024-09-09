import { CommonModule } from '@angular/common';
import { Component, Inject, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { SearchTickerComponent } from "../search-ticker/search-ticker.component";
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { FinancialAssetDto } from '../../services/financial-asset/models/financial-asset.model';
import { Router } from '@angular/router';

const MatModules = [
  MatFormFieldModule,
  MatInputModule,
  MatIconModule,
  MatButtonModule,
  MatAutocompleteModule,
  MatDialogModule]

@Component({
  selector: 'app-global-search',
  standalone: true,
  imports: [CommonModule, FormsModule, SearchTickerComponent, MatModules],
  templateUrl: './global-search.component.html',
  styleUrl: './global-search.component.css'
})
export class GlobalSearchComponent {
  inputValue: string = '';

  constructor(
    public dialogRef: MatDialogRef<GlobalSearchComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private route: Router
  ) {
    if (data && data.initialInput) {
      this.inputValue = data.initialInput;
    }
  }

  close(): void {
    this.dialogRef.close();
  }

  public onTickerSelected(assetSelected: FinancialAssetDto) {
    this.close();
    this.route.navigate(['/financial-assets', assetSelected.id]);
  }
}
