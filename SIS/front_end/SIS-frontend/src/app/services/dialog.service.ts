import { ComponentType } from '@angular/cdk/portal';
import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';

interface DialogConfig { data?: any, closeDialogRef?: MatDialogRef<any>, doNext?: (value: any) => void }

@Injectable({
  providedIn: 'root'
})
export class DialogService {

  constructor(private dialog: MatDialog) { }

  openDialog(component: ComponentType<unknown>, config?: DialogConfig) {
    const newDialogRef = this.dialog.open(component, {
      height: '90%',
      width: '90%',
      panelClass: 'custom-dialog-container',
      data: config?.data
    });
    if (config?.closeDialogRef) config.closeDialogRef.close();
    if (config?.doNext) newDialogRef.afterClosed().subscribe(config.doNext);
    return newDialogRef;
  }
}
