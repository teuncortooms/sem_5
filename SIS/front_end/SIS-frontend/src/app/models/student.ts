import { Group } from "./group";
import { HasId } from "../interfaces/has-id";

export interface Student extends HasId {
  id: string;
  firstName: string;
  lastName: string;
  fullName: string;
  currentGroup?: Group;
}

export interface StudentDetails extends HasId {
  id: string;
  firstName: string;
  lastName: string;
  fullName: string;
  email: string;
  currentGroup?: Group;
  groups?: Group[];
}
