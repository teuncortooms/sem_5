import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Message } from 'src/app/models/message';

@Injectable({
  providedIn: 'root'
})
export class LoggerService {
  messages: Message[] = [];
  snackbarDuration = 5;

  constructor(private snackBar: MatSnackBar) { }

  public log(message: any, notifyUser = false): void {
    let date: Date = new Date();
    this.messages.push({ date: date, text: message });
    console.log(message);
    if (notifyUser) this.openSnackbar(message);
  }

  public notifyUser(text: string) {
    this.log(text);
  }

  public remove(message: Message) {
    this.messages = this.messages.filter(m => m !== message);
  }

  public clear(): void {
    this.messages = [];
  }

  private openSnackbar(text: string) {
    this.snackBar.open(text, undefined, {
      duration: this.snackbarDuration * 1000,
      panelClass: 'snackbar'
    });
  }
}
