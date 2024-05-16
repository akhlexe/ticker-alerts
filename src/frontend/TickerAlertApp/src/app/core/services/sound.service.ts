import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SoundService {
  private audio: HTMLAudioElement;

  constructor() {
    this.audio = new Audio();
  }

  public playSound(soundFilePath: string): void {
    this.audio.src = soundFilePath;
    this.audio.load();
    this.audio.play().catch(error => console.error('Error playing sound:', error));
  }
}
