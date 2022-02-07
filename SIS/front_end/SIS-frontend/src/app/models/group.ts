import { HasId } from "../interfaces/has-id";
import { Student } from "./student";

export interface Group extends HasId {
  id: string;
  name: string;
  period: string;
  startDate: Date;
  endDate: Date;
}

export interface GroupDetails extends HasId {
  id: string;
  name: string;
  period: string;
  startDate: Date;
  endDate: Date;
  students?: Student[];
}
