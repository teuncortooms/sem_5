import { Injectable } from '@angular/core';
import { Message } from '../../models/Message';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  messages: Message[] = [];

  constructor() { }

  public logMessage(text: string): void {
    let date: Date = new Date();
    this.messages.push({ date: date, text: text });
  }

  public getMessages(): Message[] {
    return this.messages;
  }

  public clear(): void {
    this.messages = [];
  }
}
