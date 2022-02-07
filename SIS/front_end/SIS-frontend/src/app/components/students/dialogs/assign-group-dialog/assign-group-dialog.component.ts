import { Component } from '@angular/core';
import { AssignChildrenDialogBase } from 'src/app/base-components/assign-children-dialog-base';
import { Group, GroupDetails } from 'src/app/models/group';

@Component({
  selector: 'app-assign-group-dialog',
  templateUrl: './assign-group-dialog.component.html',
  styleUrls: ['./assign-group-dialog.component.scss']
})
export class AssignGroupDialogComponent extends AssignChildrenDialogBase<Group, GroupDetails> {

}