import { Component, Directive, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { LoggerService } from 'src/app/services/logger.service';
import { PagedDataSource } from 'src/app/services/data-source/paged-data-source';
import { DialogService } from 'src/app/services/dialog.service';
import { HasId } from '../interfaces/has-id';
import { ComponentType } from '@angular/cdk/portal';


@Directive()
export abstract class DetailsDialogBase<TModel extends HasId, TDetails extends HasId> implements OnInit {
  title: string;
  details?: TDetails;

  constructor(
    @Inject(MAT_DIALOG_DATA) public inputId: string,
    private dataSource: PagedDataSource<TModel, TDetails>,
    private dialogService: DialogService,
    private logger: LoggerService,
    private currentDialog: MatDialogRef<any>,
    private configuration: { title: string, endpoint: string, editComponent: ComponentType<unknown> }
  ) {
    this.title = configuration.title;
  }

  ngOnInit(): void {
    this.dataSource.configure(this.configuration.endpoint);
    this.loadDetails();
  }

  loadDetails() {
    this.dataSource.getDetails(this.inputId).then((details) => {
      if (!details)
       this.logger.log(`Could not find details for ${this.inputId}`);
      this.details = details;
    });
  }

  onDeleteButtonClick(): void {
    if (!this.inputId) throw new Error("Delete button should not be visible");
    this.dataSource.remove([this.inputId]);
    this.currentDialog.close();
  }

  openEditDialog(): void {
    if (!this.configuration) throw new Error("Details dialog is not configured.")
    this.dialogService.openDialog(this.configuration.editComponent, {
      data: this.details,
      closeDialogRef: this.currentDialog
    });
  }
}
