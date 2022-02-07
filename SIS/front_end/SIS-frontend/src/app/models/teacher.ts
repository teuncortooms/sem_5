import { HasId } from "../interfaces/has-id";
import { Group } from "./group";

export interface Teacher extends HasId {
    id: string;
    firstName: string;
    lastName: string;
    emailAddress?: string;
    groups?: Group[];
   /* address?: string;
    housenr?: number;
    postalcode?: string;
    city?: string;
    mobilephone?: string;
    birthdate?: Date; */
  }
