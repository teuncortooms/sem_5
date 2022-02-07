import { AfterViewInit, Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { tap } from 'rxjs/operators';
import { Grade } from 'src/app/models/grade';
import { LoggerService } from 'src/app/services/logger.service';
import { PagedDataSource } from 'src/app/services/data-source/paged-data-source';
import { OverviewBase } from 'src/app/base-components/overview-base';
import { DialogService } from 'src/app/services/dialog.service';
import { dataSourceFactory, GENERIC_API_TOKEN, GENERIC_DATA_TOKEN, GRADES_API_TOKEN, GRADES_DATA_TOKEN, GROUPS_DATA_TOKEN, STUDENTS_DATA_TOKEN } from 'src/app/services/dependency-injection/di-config';
import { GenericTableConfig, GenericTableServiceComponent, ModelTableColumn } from '../../shared/generic-table/generic-table-service.component';
import { PagedApiService } from 'src/app/services/api/paged-api.service';
import { HttpClient } from '@angular/common/http';
import { ComponentType } from '@angular/cdk/portal';
import { TableBase } from 'src/app/base-components/table-base';
import { GenericDetailsDialogComponent } from '../../shared/generic-details-dialog/generic-details-dialog.component';
import { Student } from 'src/app/models/student';
import { Group } from 'src/app/models/group';
import { GroupedObservable } from 'rxjs';
import { ModelListColumn, ModelListParameters } from '../../shared/select-model-list/select-model-list.component';
import { HasId } from 'src/app/interfaces/has-id';
import { GenericEditDialogComponent } from '../../shared/generic-edit-dialog/generic-edit-dialog.component';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';

@Component({
  selector: 'app-grades-overview',
  templateUrl: './grades-overview.component.html',
  styleUrls: ['./grades-overview.component.scss']
})
export class GradesOverviewComponent  {

  private config: GenericTableConfig;
  private http: HttpClient;
  private logger: LoggerService;
  public ds: PagedDataSource<Grade,Grade>;

  private configuration: {detailsComponent: ComponentType<unknown>, editComponent: ComponentType<unknown>}
  @ViewChild('myTable') table?: TableBase<Grade, Grade>

  public canDelete: boolean = true;
  public canCreate: boolean = true;

  constructor(
    private dialogService: DialogService,
    @Inject(STUDENTS_DATA_TOKEN) dsUser: PagedDataSource<Student, Student>,
    @Inject(GROUPS_DATA_TOKEN) dsGroup: PagedDataSource<Group, Group>,
    config: GenericTableConfig,
    http: HttpClient,
    logger: LoggerService,
    authSvc: AuthenticationService)
    {
      const configuration = {
        detailsComponent: GenericDetailsDialogComponent,
        editComponent: GenericEditDialogComponent
      };
      //super(new PagedDataSource(new PagedApiService(http,logger),logger), dialogService, configuration);
      this.ds = new PagedDataSource(new PagedApiService(http,logger),logger);
      this.configuration = configuration;

      this.http = http;
      this.logger = logger;
      this.config = config;

      var groupSelectConfig: ModelListParameters<Group,Group> =  {
              editboxLabel: "Klas",
              dataSource: dsGroup,
              editboxValueDisplayCallback:()=> undefined,
              endpoint: 'groups',
              sortHeader: 'startDate',
              sortDirection: 'desc',
              model: undefined,
              onSelect: (group)=> undefined,
              displayedColumns: new Map<string, ModelListColumn<Group>>([
                ["name", { displayName: "Klas naam", callbackStringValue: (s)=> s.name}],
                ["period", { displayName: "Periode", callbackStringValue: (s)=> s.period}],
                ["startDate", { displayName: "Start", callbackStringValue: (s)=> new Date(s.startDate).toDateString()}],
                ["endDate", { displayName: "Eind", callbackStringValue: (s)=> new Date(s.endDate).toDateString()}],
               ]
               )
            }
        var groupSelectConfigP = (groupSelectConfig as unknown) as ModelListParameters<HasId,HasId>;
        var studentSelectConfig: ModelListParameters<Student,Student> = {
          editboxLabel: "Student",
          dataSource: dsUser,
          editboxValueDisplayCallback:()=> undefined,
          endpoint: 'students',
          sortHeader: 'startDate',
          sortDirection: 'desc',
          model: undefined,
          onSelect: ()=>undefined,
          displayedColumns: new Map<string, ModelListColumn<Student>>([
            ["firstName", { displayName: "Voornaam", callbackStringValue: (s)=> s.firstName}],
            ["lastName", { displayName: "Achternaam", callbackStringValue: (s)=> s.lastName}],
            ["currentGroup", { displayName: "Klas", callbackStringValue: (s)=> s.currentGroup?.name}]
           ]
           )
        }

      this.config.register({
//        editboxLabel: "Cijfer",
//        editboxValueDisplayCallback: () => "undefined",
        entityDisplayNameSingular: "Cijfer",
        entityDisplayNamePlural: "Cijfers",
        endpoint: 'grades',
        sortHeader: 'firstName',
        sortDirection: 'asc',
        editClaim: 'p_grade_write',
        deleteClaim: 'p_grade_delete',
        createClaim: 'p_grade_create',

        displayedColumns: new Map<string, ModelTableColumn<Grade,any>>([
          ["firstName", { displayName: "Voornaam", callbackStringValue: (s)=> s.student.firstName}],
          ["lastName", { displayName: "Achternaam", callbackStringValue: (s)=> s.student.lastName}],
          ["group", { displayName: "Klas", callbackStringValue: (s)=> s.group.name }],
          ["Score", { displayName: "Score", callbackStringValue: (s)=> s.score.toString()}]
         ]
         ),
         detailDialogColumns: new Map<string, ModelTableColumn<Grade,any>>([
          ["name", { displayName: "Naam", callbackStringValue: (s)=> s?.student?.fullName,
           callbackEditorValue:(m)=>m.student, lookupTableConfig: studentSelectConfig as ModelListParameters<any,any>
           ,callbackSaveValue:(m,v)=>m.student=v, editType:"hasid"
        }],
          ["lastName", { displayName: "Achternaam", callbackStringValue: (s)=> s?.student?.lastName}],
          ["group", { displayName: "Klas", callbackStringValue: (s)=> s?.group?.name, callbackEditorValue: (m)=>m.group,
             lookupTableConfig: groupSelectConfigP, callbackSaveValue: (m,v)=>m.group=v,editType:"hasid"

           }],
          ["Score", { displayName: "Score", callbackStringValue: (s)=> s?.score?.toString(),
          callbackEditorValue: (m)=>m.score, callbackSaveValue: (m,v)=>m.score=v, editType:"number"}]
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
      var grade : Partial<Grade> = {
        score: 0,
        group: undefined,
        student: undefined
      };

      this.dialogService.openDialog(this.configuration.editComponent, {
        data: {
          input: grade,
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
