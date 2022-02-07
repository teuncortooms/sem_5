import { ComponentType } from '@angular/cdk/portal';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { TableBase } from 'src/app/base-components/table-base';
import { HasId } from 'src/app/interfaces/has-id';
import { Claim } from 'src/app/models/claim';
import { Role } from 'src/app/models/role';
import { PagedApiService } from 'src/app/services/api/paged-api.service';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { PagedDataSource } from 'src/app/services/data-source/paged-data-source';
import { DialogService } from 'src/app/services/dialog.service';
import { LoggerService } from 'src/app/services/logger.service';
import { GenericDetailsDialogComponent } from '../../shared/generic-details-dialog/generic-details-dialog.component';
import { GenericEditDialogComponent } from '../../shared/generic-edit-dialog/generic-edit-dialog.component';
import { GenericTableConfig, ModelTableColumn } from '../../shared/generic-table/generic-table-service.component';
import { ModelListColumn, ModelListParameters } from '../../shared/select-model-list/select-model-list.component';

@Component({
  selector: 'app-roles-overview',
  templateUrl: './roles-overview.component.html',
  styleUrls: ['./roles-overview.component.scss']
})
export class RolesOverviewComponent implements OnInit {
  public dsClaims: PagedDataSource<Claim,Claim>;

  private config: GenericTableConfig;
  private http: HttpClient;
  private logger: LoggerService;
  public ds: PagedDataSource<Role,Role>;
  private configuration: {detailsComponent: ComponentType<unknown>, editComponent: ComponentType<unknown>}
  @ViewChild('myTable') table?: TableBase<Role, Role>
  public canDelete: boolean = true;
  public canCreate: boolean = true;


  constructor( private dialogService: DialogService, logger: LoggerService, config: GenericTableConfig,http: HttpClient,
    authSvc: AuthenticationService)
  {
    const configuration = {
      detailsComponent: GenericDetailsDialogComponent,
      editComponent: GenericEditDialogComponent
    };

    this.dsClaims = new PagedDataSource(new PagedApiService(http,logger),logger);
    this.dsClaims.configure('permissions');

    var claimSelectConfig: ModelListParameters<Claim,Claim> =  {
      editboxLabel: "Rol permissies",
      dataSource: this.dsClaims,
      editboxValueDisplayCallback:()=> undefined,
      endpoint: 'permissions',
      sortHeader: 'startDate',
      sortDirection: 'desc',
      model: undefined,
      displayedColumns: new Map<string, ModelListColumn<Claim>>([
        ["name", { displayName: "Omschrijving", callbackStringValue: (s)=> Claim.ClaimToDisplayName(s)}]
      ])
    }
    var claimSelectConfigP = (claimSelectConfig as unknown) as ModelListParameters<HasId,HasId>;


    this.ds = new PagedDataSource(new PagedApiService(http,logger),logger);
    this.ds.configure('roles');
    this.configuration = configuration;
    this.logger = logger;
    this.http = http;
    this.config = config;
    config.register( {
      entityDisplayNameSingular: "Rol",
      entityDisplayNamePlural: "Rollen",
      endpoint: 'roles',
      sortHeader: 'name',
      sortDirection: 'asc',
      editClaim: 'p_admin_role_write',
      deleteClaim: 'p_admin_role_delete',
      createClaim: 'p_admin_role_create',

      displayedColumns: new Map<string, ModelTableColumn<Role,any>>([
        ["name", { displayName: "Rol naam", callbackStringValue: (s)=> s.name }],
        ["roleClaims", { displayName: "Rol permissies", callbackStringValue: (s)=> s.roleClaims?.map(c=>Claim.ClaimToDisplayName(c)).join(', ')}]
       ]
       ),
       detailDialogColumns: new Map<string, ModelTableColumn<Role,any>>([
        ["name", { displayName: "Rol naam", callbackStringValue: (s)=> s?.name, callbackEditorValue: (s)=>s.name, callbackSaveValue: (m,v)=>m.name=v
          ,editType:"string"}],
        ["roleClaims", { displayName: "Rol permissies", callbackStringValue: (s)=> s?.roleClaims?.map(c=>Claim.ClaimToDisplayName(c)).join(', ')
         , callbackEditorValue:(m)=>m.roleClaims, callbackSaveValue:(m,v)=>m.roleClaims=v, lookupTableConfig: claimSelectConfigP, editType:"list" }]
       ]
       )
    });

    if(config.retrieve().deleteClaim != undefined)
    {
      if(!authSvc.hasClaimPart(config.retrieve().deleteClaim!) && !authSvc.hasClaimPart('p_all'))
        this.canDelete = false;
    }
    if(config.retrieve().createClaim != undefined)
    {
      if(!authSvc.hasClaimPart(config.retrieve().createClaim!) && !authSvc.hasClaimPart('p_all'))
        this.canCreate = false;
    }
  }

  ngOnInit(): void {
  }

   public openAddDialog(): void {
    var role : Role = {
      id: '',
      name: '',
      roleClaims: []
    };

    this.dialogService.openDialog(this.configuration.editComponent, {
      data: {
        input: role,
        dataSource: this.ds,
        titleEdit: this.config.retrieve().entityDisplayNameSingular + ' bewerken',
        titleAdd: this.config.retrieve().entityDisplayNameSingular + ' toevoegen',
        detailsDialog: this
        }
    });
  }

  public openDetailsDialog(id: string): void {
    this.dialogService.openDialog(this.configuration.detailsComponent, {
      data: { inputId: id, dataSource: this.ds }
    });
  }

  public removeSelection() {
    const models = this.table?.selection.selected;
    this.table?.selection.clear();
    const ids = models?.map(s => s.id);

    if (ids) this.ds.remove(ids);
  }


}
