import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'currencyMask',
  standalone: true,
})
export class CurrencyMaskPipe implements PipeTransform {
  transform(
    value: number | string,
    locale: string = 'en-US',
    currency: string = 'USD'
  ): string {
    if (value === null || value === undefined) return '';

    let numericValue = typeof value === 'number' ? value : parseFloat(value);
    if (isNaN(numericValue)) return '';

    return new Intl.NumberFormat(locale, {
      style: 'currency',
      currency: currency,
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    }).format(numericValue);
  }
}
