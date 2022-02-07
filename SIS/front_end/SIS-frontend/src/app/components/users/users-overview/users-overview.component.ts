import { ComponentType } from '@angular/cdk/portal';
import { HttpClient } from '@angular/common/http';
import { AfterViewInit, ChangeDetectorRef, Component, ComponentFactoryResolver, Inject, Injectable, OnChanges, OnInit, SimpleChanges, ViewChild, ViewContainerRef } from '@angular/core';
import { OverviewBase } from 'src/app/base-components/overview-base';
import { TableBase } from 'src/app/base-components/table-base';
import { HasId } from 'src/app/interfaces/has-id';
import { Claim } from 'src/app/models/claim';
import { Role } from 'src/app/models/role';
import { User } from 'src/app/models/user';
import { PagedApiService } from 'src/app/services/api/paged-api.service';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { PagedDataSource } from 'src/app/services/data-source/paged-data-source';
import { GENERIC_DATA_TOKEN, USER_API_TOKEN, USER_DATA_TOKEN } from 'src/app/services/dependency-injection/di-config';
import { DialogService } from 'src/app/services/dialog.service';
import { LoggerService } from 'src/app/services/logger.service';
import { GenericDetailsDialogComponent } from '../../shared/generic-details-dialog/generic-details-dialog.component';
import { GenericEditDialogComponent } from '../../shared/generic-edit-dialog/generic-edit-dialog.component';
import { GenericTableConfig, GenericTableServiceComponent, ModelTableColumn, ModelTableParameters } from '../../shared/generic-table/generic-table-service.component';
import { ModelListColumn, ModelListParameters } from '../../shared/select-model-list/select-model-list.component';


@Component({
  selector: 'app-users-overview',
  templateUrl: './users-overview.component.html',
  styleUrls: ['./users-overview.component.scss']
})
export class UsersOverviewComponent {
  private config: GenericTableConfig;
  private http: HttpClient;
  private logger: LoggerService;
  public ds: PagedDataSource<User,User>;
  public dsRoles: PagedDataSource<Role,Role>;
  public dsClaims: PagedDataSource<Claim,Claim>;
  private configuration: {detailsComponent: ComponentType<unknown>, editComponent: ComponentType<unknown>}
  @ViewChild('myTable') table?: TableBase<User, User>
  public canDelete: boolean = true;
  public canCreate: boolean = true;

  constructor(
  private dialogService: DialogService, logger: LoggerService, config: GenericTableConfig,http: HttpClient,
  authSvc: AuthenticationService)
  {

    const configuration = {
      detailsComponent: GenericDetailsDialogComponent,
      editComponent: GenericEditDialogComponent
    };
    //super(dataSource, dialogService, configuration);
    this.ds = new PagedDataSource(new PagedApiService(http,logger),logger);
    this.ds.configure('users');
    this.dsRoles = new PagedDataSource(new PagedApiService(http,logger),logger);
    this.dsRoles.configure('roles');
    this.dsClaims = new PagedDataSource(new PagedApiService(http,logger),logger);
    this.dsClaims.configure('permissions');
    this.configuration = configuration;
    this.logger = logger;
    this.http = http;
    this.config = config;

    var roleSelectConfig: ModelListParameters<Role,Role> =  {
      editboxLabel: "Rol",
      dataSource: this.dsRoles,
      editboxValueDisplayCallback:()=> undefined,
      endpoint: 'roles',
      sortHeader: 'startDate',
      sortDirection: 'desc',
      model: undefined,
      displayedColumns: new Map<string, ModelListColumn<Role>>([
        ["name", { displayName: "Rol naam", callbackStringValue: (s)=> s.name}],
        ["roleClaims", { displayName: "Rol permissies", callbackStringValue: (s)=> s.roleClaims?.map(c=>Claim.ClaimToDisplayName(c)).join(', ')}]
      ])
    }
var roleSelectConfigP = (roleSelectConfig as unknown) as ModelListParameters<HasId,HasId>;

var claimSelectConfig: ModelListParameters<Claim,Claim> =  {
  editboxLabel: "Direct toegewezen permissies",
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


    config.register( {
      entityDisplayNameSingular: "Gebruiker",
      entityDisplayNamePlural: "Gebruikers",
      endpoint: 'users',
      sortHeader: 'loginName',
      sortDirection: 'desc',
      editClaim: 'p_admin_user_update',
      deleteClaim: 'p_admin_user_delete',
      createClaim: 'p_admin_user_create',

      displayedColumns: new Map<string, ModelTableColumn<User,any>>([
        ["loginName", { displayName: "Login naam", callbackStringValue: (s)=> s.loginName}],
        ["Email", { displayName: "Email adres", callbackStringValue: (s)=> s.eMail}],
        ["roles", { displayName: "Toegekende rollen", callbackStringValue: (s)=> s?.roles.map(e => e.name).join(", ")}]
       ]
       ),
       detailDialogColumns: new Map<string, ModelTableColumn<User,any>>([
        ["loginName", { displayName: "Login naam", callbackStringValue: (s)=> s?.loginName, callbackEditorValue:(s)=>s.loginName,
          callbackSaveValue:(m,v)=>m.loginName=v, editType:"string"}],
        ["Email", { displayName: "Email adres", callbackStringValue: (s)=> s?.eMail, callbackEditorValue:(s)=>s.eMail,callbackSaveValue:(m,v)=>m.eMail=v,
          editType:"string" }],
        ["password", { displayName: "Wachtwoord", callbackStringValue: (s)=> s?.password, callbackEditorValue:(s)=>s.password,
        callbackSaveValue:(m,v)=>m.password=v, secureField: true, editType:"string", onlyNewItems:true}],
        ["roles", { displayName: "Toegekende rollen", callbackStringValue: (s)=> s?.roles.map(e => e.name).join(", "), callbackEditorValue:(s)=>s.roles
          ,lookupTableConfig: roleSelectConfigP, callbackSaveValue:(m,v)=>m.roles=v,editType:"list" }],
        ["claims", { displayName: "Directe toegekende permissies", callbackStringValue: (s)=> s?.claims.map(e => Claim.ClaimToDisplayName(e)).join(", ")
          , callbackEditorValue:(m)=>m.claims, lookupTableConfig: claimSelectConfigP, callbackSaveValue:(m,v)=>m.claims=v,editType:"list"}]
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

   public openAddDialog(): void {
    var user : User = {
      id: '',
      eMail: '',
      password: '',
      loginName: '',
      claims: [],
      roles: []
    };

    this.dialogService.openDialog(this.configuration.editComponent, {
      data: {
        input: user,
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
