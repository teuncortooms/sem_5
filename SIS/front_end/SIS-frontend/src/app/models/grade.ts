import { Group } from "./group";
import { HasId } from "../interfaces/has-id";
import { Student } from "./student";

export interface Grade extends HasId {
    id: string;
    group: Group;
    student: Student;
    score: number;
}
