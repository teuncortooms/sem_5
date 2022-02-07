import { ComponentType } from '@angular/cdk/portal';
import { KeyValue } from '@angular/common';
import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DetailsDialogBase } from 'src/app/base-components/details-dialog-base';
import { HasId } from 'src/app/interfaces/has-id';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { PagedDataSource } from 'src/app/services/data-source/paged-data-source';
import { GENERIC_DATA_TOKEN } from 'src/app/services/dependency-injection/di-config';
import { DialogService } from 'src/app/services/dialog.service';
import { LoggerService } from 'src/app/services/logger.service';
import { GenericEditDialogComponent } from '../generic-edit-dialog/generic-edit-dialog.component';
import { GenericTableConfig } from '../generic-table/generic-table-service.component';

@Component({
  selector: 'app-generic-details-dialog',
  templateUrl: './generic-details-dialog.component.html',
  styleUrls: ['./generic-details-dialog.component.scss']
})
export class GenericDetailsDialogComponent<M extends HasId,D extends HasId> implements OnInit {
  title: string;
  public details?: D;
  public config: GenericTableConfig;
  private configuration: { title: string, endpoint: string, editComponent: ComponentType<unknown> }
  public canEdit: boolean = true;
  public canDelete: boolean = true;

  constructor(
    @Inject(MAT_DIALOG_DATA) private conf: { inputId:string, dataSource:PagedDataSource<M, D> },
    private dialogService: DialogService,
    private logger: LoggerService,
    private currentDialog: MatDialogRef<GenericDetailsDialogComponent<M,D>>,
    config: GenericTableConfig,
    authSvc: AuthenticationService
  )
  {
    const configuration = {
      title: 'Een '+config.retrieve().entityDisplayNameSingular.toLowerCase()+' bekijken',
      endpoint: config.retrieve().endpoint,
      editComponent: GenericEditDialogComponent
    };
    //super(inputId, dataSource, dialogService, logger, currentDialog, configuration);
    this.configuration = configuration;
    this.title = configuration.title;
    this.config = config;
    if(config.retrieve().editClaim != undefined)
    {
      if(!authSvc.hasClaimPart(config.retrieve().editClaim!) && !authSvc.hasClaimPart('p_all'))
        this.canEdit = false;
    }

    if(config.retrieve().deleteClaim != undefined)
    {
      if(!authSvc.hasClaimPart(config.retrieve().deleteClaim!) && !authSvc.hasClaimPart('p_all'))
        this.canDelete = false;
    }

  }

  ngOnInit(): void {
    this.conf.dataSource.configure(this.configuration.endpoint);
    this.loadDetails();
  }

  loadDetails() {
    this.conf.dataSource.getDetails(this.conf.inputId).then((details) => {
      if (!details) this.logger.log(`Could not find details for ${this.conf.inputId}`);
      this.details = details;
    });
  }

  onDeleteButtonClick(): void {
    if (!this.conf.inputId) throw new Error("Delete button should not be visible");
    this.conf.dataSource.remove([this.conf.inputId]);
    this.currentDialog.close();
  }

  openEditDialog(): void {
    if (!this.configuration) throw new Error("Details dialog is not configured.")
    this.dialogService.openDialog(this.configuration.editComponent, {
      data: {
        input: this.details,
        dataSource: this.conf.dataSource,
        titleEdit: this.config.retrieve().entityDisplayNameSingular + ' bewerken',
        titleAdd: this.config.retrieve().entityDisplayNameSingular + ' toevoegen',
        detailsDialog: this
        },
      closeDialogRef: this.currentDialog
    });
  }

  retrieveModel(): D {
    return this.details!;
  }

  unsorted(a: any, b: any): number { return 0; }

}
