import { Directive, ViewChild } from '@angular/core';
import { PagedDataSource } from 'src/app/services/data-source/paged-data-source';
import { DialogService } from 'src/app/services/dialog.service';
import { TableBase } from './table-base';
import { HasId } from '../interfaces/has-id';
import { ComponentType } from '@angular/cdk/portal';

@Directive()
export abstract class OverviewBase<TModel extends HasId, TDetails extends HasId> {
  @ViewChild('myTable') table?: TableBase<TModel, TDetails>

  constructor(
    public dataSource: PagedDataSource<TModel, TDetails>,
    public dialogService: DialogService,
    private configuration: {detailsComponent: ComponentType<unknown>, editComponent: ComponentType<unknown>}
  ) { }

  public openAddDialog(): void {
    this.dialogService.openDialog(this.configuration.editComponent);
  }

  public openDetailsDialog(id: string): void {
    this.dialogService.openDialog(this.configuration.detailsComponent, {
      data: id
    });
  }

  public removeSelection() {
    const models = this.table?.selection.selected;
    this.table?.selection.clear();
    const ids = models?.map(s => s.id);

    if (ids) this.dataSource.remove(ids);
  }
}
