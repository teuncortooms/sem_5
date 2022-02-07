import { HasId } from "../interfaces/has-id";
import { Claim } from "./claim";

export interface Role extends HasId {
  id: string;
  name: string;
  roleClaims?: Claim[];
}
