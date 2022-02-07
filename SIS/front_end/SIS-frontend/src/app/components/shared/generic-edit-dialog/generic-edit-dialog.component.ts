import { ComponentType } from '@angular/cdk/portal';
import { KeyValue } from '@angular/common';
import { Component, Inject, OnInit, Type } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { LoggerService } from 'angular-auth-oidc-client';
import { HasId } from 'src/app/interfaces/has-id';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { PagedDataSource } from 'src/app/services/data-source/paged-data-source';
import { DialogService } from 'src/app/services/dialog.service';
import { GenericTableConfig, ModelTableColumn } from '../generic-table/generic-table-service.component';

@Component({
  selector: 'app-generic-edit-dialog',
  templateUrl: './generic-edit-dialog.component.html',
  styleUrls: ['./generic-edit-dialog.component.scss']
})
export class GenericEditDialogComponent<M extends HasId,D extends HasId> implements OnInit {
  title: string;
  isNew: boolean = false;
  public canDelete: boolean = true;
  public details: Partial<D>;
  public config: GenericTableConfig;
  private configuration: {
    input:Partial<D>,
      dataSource:PagedDataSource<M, D>,
      titleEdit: string,
      titleAdd: string,
      detailsDialog: ComponentType<unknown>
  }

  constructor(
    @Inject(MAT_DIALOG_DATA) private conf: {
      input:Partial<D>,
      dataSource:PagedDataSource<M, D>,
      titleEdit: string,
      titleAdd: string,
      detailsDialog: ComponentType<unknown>
    },
    dialogService: DialogService,
//    logger: LoggerService,
    private currentDialog: MatDialogRef<GenericEditDialogComponent<M,D>>,
    config: GenericTableConfig,
    authSvc: AuthenticationService
  ) {
    this.config = config;
    this.details = conf.input;
    if (!conf.input || !conf.input.id) {
      this.isNew = true;
      //this.details = {};
    }
    this.configuration = conf;

    this.title = this.isNew ? conf.titleAdd : conf.titleEdit;
    currentDialog.disableClose = true;

    if(config.retrieve().deleteClaim != undefined)
    {
      if(!authSvc.hasClaimPart(config.retrieve().deleteClaim!) && !authSvc.hasClaimPart('p_all'))
        this.canDelete = false;
    }
   }

  ngOnInit(): void {
  }

  parseDefinitiveModel(): D
  {
    return this.details as D;
  }

  onSaveButtonClick(): void {

    var definitive = this.parseDefinitiveModel();

    if (this.isNew) this.configuration.dataSource.add(definitive);
    else this.configuration.dataSource.update(this.details.id!, definitive);
    this.currentDialog.close();

  }

  onDeleteButtonClick(): void {
    if (!this.details.id) throw new Error("Delete button should not be visible");
    this.configuration.dataSource.remove([this.details.id]);
    this.currentDialog.close();
  }

  unsorted(a: any, b: any): number { return 0; }

  triggerchangefield(data:KeyValue<string,ModelTableColumn<HasId,any>>,event:any): void
  {
    data!.value!.callbackSaveValue!(this.parseDefinitiveModel(),event.target.value);
  }

  retrievevalue(data:KeyValue<string,ModelTableColumn<HasId,any>>): any
  {
    var model : D = this.details as D;
    if(data.value.callbackSaveValue != undefined && data.value.lookupTableConfig != undefined)
      data.value.lookupTableConfig.onSelect=(m)=>data.value.callbackSaveValue!(model,m as any);
    return data!.value!.callbackEditorValue!(model);
  }

  textOrPassword(data:KeyValue<string,ModelTableColumn<HasId,any>>): string
  {
    if(data.value.secureField == true)
    {
      return "password";
    }

    return "text";
  }

  matchtype(data:KeyValue<string,ModelTableColumn<HasId,any>>,type:string): boolean
  {
    var model : D = this.details as D;
    if(data?.value?.callbackEditorValue == null)
      return false;
    if(data.value.onlyNewItems == true && this.isNew == false)
    {
      return false;
    }
    var val = data!.value!.callbackEditorValue!(model);

//    if(val == undefined)
//      reSturn false;

    switch(type)
    {
      case "string": if( data.value.editType == type ) return true; else return false;
      case "number": if( data.value.editType == type ) return true; else return false;
      case "hasid": if( data.value.editType != type ) return false; else {
          data.value.lookupTableConfig!.model=val;
          data.value.lookupTableConfig!.editboxValueDisplayCallback=()=>data.value.callbackStringValue(model);
        return true;
      }
      case "list": if( data.value.editType == type )
        {
          data.value.lookupTableConfig!.models = val;
          return true;
        } else return false;
    }
    return false;
  }

}
