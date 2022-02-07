import { SelectionModel } from '@angular/cdk/collections';
import { Component, Directive, Input, OnChanges } from '@angular/core';
import { Group } from 'src/app/models/group';
import { LoggerService } from '../services/logger.service';

export interface ListConfiguration {
  displayedColumns: string[],
  sorts?: ((a: any, b: any) => number)[]
}

@Directive()
export class ChildrensListBase<TModel> implements OnChanges {
  @Input() isEditMode?: boolean;
  @Input() models?: TModel[];
  @Input() current?: TModel;

  modelsSorted?: TModel[];
  displayedColumns: string[];
  selection = new SelectionModel<TModel>(true, []);

  constructor(private logger: LoggerService, private configuration: ListConfiguration) {
    this.displayedColumns = configuration.displayedColumns;
  }

  ngOnChanges(): void {
    this.modelsSorted = this.sort();
    if (this.isEditMode) this.displayedColumns = ['select', ...this.displayedColumns];
  }

  private sort(): TModel[] | undefined {
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
}


