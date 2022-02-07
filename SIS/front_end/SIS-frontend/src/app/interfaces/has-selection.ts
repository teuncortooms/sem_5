import { SelectionModel } from '@angular/cdk/collections';

export interface HasSelection<TModel> {
    selection: SelectionModel<TModel>,
}
