import { ComponentType } from '@angular/cdk/portal';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable } from '@angular/material/table';
import { TableBase } from 'src/app/base-components/table-base';
import { HasId } from 'src/app/interfaces/has-id';
import { Group } from 'src/app/models/group';
import { Teacher } from 'src/app/models/teacher';
import { PagedApiService } from 'src/app/services/api/paged-api.service';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { PagedDataSource } from 'src/app/services/data-source/paged-data-source';
import { TEACHERS_DATA_TOKEN } from 'src/app/services/dependency-injection/di-config';
import { DialogService } from 'src/app/services/dialog.service';
import { LoggerService } from 'src/app/services/logger.service';
import { __await } from 'tslib';
import { GenericDetailsDialogComponent } from '../../shared/generic-details-dialog/generic-details-dialog.component';
import { GenericEditDialogComponent } from '../../shared/generic-edit-dialog/generic-edit-dialog.component';
import { GenericTableConfig, ModelTableColumn } from '../../shared/generic-table/generic-table-service.component';
import { ModelListColumn, ModelListParameters } from '../../shared/select-model-list/select-model-list.component';
import { EditTeacherDialogComponent } from '../edit-teacher-dialog/edit-teacher-dialog.component';

@Component({
  selector: 'app-teachers-overview',
  templateUrl: './teachers-overview.component.html',
  styleUrls: ['./teachers-overview.component.scss']
})
export class TeachersOverviewComponent {
  private config: GenericTableConfig;
  private http: HttpClient;
  private logger: LoggerService;
  public ds: PagedDataSource<Teacher,Teacher>;
  public dsGroup: PagedDataSource<Group,Group>;

  private configuration: {detailsComponent: ComponentType<unknown>, editComponent: ComponentType<unknown>}
  @ViewChild('myTable') table?: TableBase<Teacher, Teacher>
  public canDelete: boolean = true;
  public canCreate: boolean = true;

  constructor(private dialogService: DialogService, logger: LoggerService, config: GenericTableConfig,http: HttpClient,
    authSvc: AuthenticationService) {

      const configuration = {
        detailsComponent: GenericDetailsDialogComponent,
        editComponent: GenericEditDialogComponent
      };
      //super(dataSource, dialogService, configuration);
      this.ds = new PagedDataSource(new PagedApiService(http,logger),logger);
      this.ds.configure('teachers');
      this.dsGroup = new PagedDataSource(new PagedApiService(http,logger),logger);
      this.dsGroup.configure('groups');

      this.configuration = configuration;
      this.logger = logger;
      this.http = http;
      this.config = config;

      var groupSelectConfig: ModelListParameters<Group,Group> =  {
        editboxLabel: "Rol",
        dataSource: this.dsGroup,
        editboxValueDisplayCallback:()=> undefined,
        endpoint: 'groups',
        sortHeader: 'startDate',
        sortDirection: 'desc',
        model: undefined,
        displayedColumns: new Map<string, ModelListColumn<Group>>([
          ["name", { displayName: "Groep naam", callbackStringValue: (s)=> s.name}]
        ])
      }
  var groupSelectConfigP = (groupSelectConfig as unknown) as ModelListParameters<HasId,HasId>;


      config.register( {
        entityDisplayNameSingular: "Docent",
        entityDisplayNamePlural: "Docenten",
        endpoint: 'teachers',
        sortHeader: 'loginName',
        sortDirection: 'desc',
        editClaim: 'p_teacher_write',
        deleteClaim: 'p_teacher_delete',
        createClaim: 'p_teacher_create',

        displayedColumns: new Map<string, ModelTableColumn<Teacher,any>>([
          ["firstName", { displayName: "Voornaam", callbackStringValue: (s)=> s.firstName}],
          ["lastName", { displayName: "Achternaam", callbackStringValue: (s)=> s.lastName}],
          ["groups", { displayName: "Toegekende groepen", callbackStringValue: (s)=> s.groups?.map(e => e.name).join(", ")}]
         ]
         ),
         detailDialogColumns: new Map<string, ModelTableColumn<Teacher,any>>([
          ["firstName", { displayName: "Voornaam", callbackStringValue: (s)=> s?.firstName, callbackEditorValue:(s)=>s.firstName,
            callbackSaveValue:(m,v)=>m.firstName=v, editType:"string"}],
          ["lastName", { displayName: "Achternaam", callbackStringValue: (s)=> s?.lastName, callbackEditorValue:(s)=>s.lastName,callbackSaveValue:(m,v)=>m.lastName=v,
            editType:"string" }],
          ["groups", { displayName: "Toegekende groepen", callbackStringValue: (s)=> s.groups?.map(e => e.name).join(", "), callbackEditorValue:(s)=>s.groups
            ,lookupTableConfig: groupSelectConfigP, callbackSaveValue:(m,v)=>m.groups=v  ,editType:"list" }]
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
      var teacher : Teacher = {
        id: '',
        firstName: '',
        lastName: '',
        groups: []
      };

      this.dialogService.openDialog(this.configuration.editComponent, {
        data: {
          input: teacher,
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
