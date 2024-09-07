import { DOCUMENT } from '@angular/common';
import { Inject, Injectable } from '@angular/core';
import { Observable, Subject, Subscription } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class GlobalKeydownService {

  private keydownSubject = new Subject<KeyboardEvent>();
  private isListening = false;
  private authSubscription: Subscription;

  constructor(@Inject(DOCUMENT) private document: Document, private authService: AuthService) {
    this.authSubscription = this.authService.getLoggedInUsername().subscribe(isLoggedIn => {
      if (isLoggedIn) {
        this.startListening();
      } else {
        this.stopListening();
      }
    });
  }

  private startListening() {
    if (!this.isListening) {
      this.isListening = true;
      this.document.addEventListener('keydown', this.handleKeydown.bind(this));
    }
  }

  private stopListening() {
    if (this.isListening) {
      this.isListening = false;
      this.document.removeEventListener('keydown', this.handleKeydown.bind(this));
    }
  }

  private handleKeydown(event: KeyboardEvent) {
    this.keydownSubject.next(event);
    console.log(event);
  }

  public getKeydownObservable(): Observable<KeyboardEvent> {
    return this.keydownSubject.asObservable();
  }

  ngOnDestroy() {
    this.stopListening();
    this.authSubscription.unsubscribe();
  }
}
