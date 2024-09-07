import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { SearchTickerComponent } from "../../search-ticker.component";

@Component({
  selector: 'app-global-search',
  standalone: true,
  imports: [MatFormFieldModule, CommonModule, FormsModule, MatDialogModule, MatInputModule, SearchTickerComponent],
  templateUrl: './global-search.component.html',
  styleUrl: './global-search.component.css'
})
export class GlobalSearchComponent {
  inputValue: string = '';

  constructor(
    public dialogRef: MatDialogRef<GlobalSearchComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    if (data && data.initialInput) {
      this.inputValue = data.initialInput;
    }
  }

  close(): void {
    this.dialogRef.close();
  }
}
