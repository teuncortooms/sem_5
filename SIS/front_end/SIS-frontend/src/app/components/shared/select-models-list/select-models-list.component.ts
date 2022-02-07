import { SelectionModel } from '@angular/cdk/collections';
import { Component, EventEmitter, Inject, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ChildrensListBase, ListConfiguration } from 'src/app/base-components/childrens-list-base';
import { HasId } from 'src/app/interfaces/has-id';
import { LoggerService } from 'src/app/services/logger.service';
import { ModelListParameters } from '../select-model-list/select-model-list.component';

@Component({
  selector: 'app-select-models-list',
  templateUrl: './select-models-list.component.html',
  styleUrls: ['./select-models-list.component.scss']
})
export class SelectModelsListComponent<M extends HasId> implements OnInit,OnChanges {
  @Input() public data!: ModelListParameters<M,M>;
  @Input() models?: M[];
  @Input() current?: M;
  @Input() selectedItemsEvent?: EventEmitter<SelectionModel<M>>;
  public displayedColumns :string[] = ['Select'];
  private configuration: ListConfiguration = {
    displayedColumns: this.displayedColumns,
    sorts: [
      (a, b) => a.id.localeCompare(b.id)
    ]
  }

  modelsSorted?: M[];
  //displayedColumns: string[];

  selection = new SelectionModel<M>(true, []);
  constructor(
    //@Inject(MAT_DIALOG_DATA) data: ModelListParameters<M,M>,
    private logger: LoggerService)
  {
    this.selection.changed.subscribe((m)=>  this.selectedItemsEvent?.emit(this.selection))

  }
  ngOnChanges(changes: SimpleChanges): void {
    this.modelsSorted = this.sort();

  }

  ngOnInit(): void {
    this.data.displayedColumns.forEach( (v,k)=> this.displayedColumns.push(k) );

    this.logger.log("selecting columns: "+ this.displayedColumns.toString());



  }

   /** Whether the number of selected elements matches the total number of rows. */
   public isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.models?.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  public masterToggle() {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }

    if (this.models) this.selection.select(...this.models);
  }

  private sort(): M[] | undefined {
    if (!this.models) {
      this.logger.log("No models to sort");
      return undefined;
    }

    var result = [...this.models];
    this.configuration.sorts?.forEach(compareFn => {
      result.sort(compareFn);
    });
    return result;
  }




}
