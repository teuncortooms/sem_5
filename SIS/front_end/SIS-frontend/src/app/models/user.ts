import { HasId } from "../interfaces/has-id";
import { Claim } from "./claim";
import { Role } from "./role";

export interface User extends HasId {
  id: string;
  loginName: string;
  password?: string;
  eMail: string;
  claims: Claim[];
  roles: Role[];
}

export interface RegisterCommand {
    email: string;
    password: string;
    confirmPassword: string;
}

export interface RegisterResponse {
    statusCode: number;
    status: string;
    message: string;
    errors: string[];
}

export interface LoginCommand {
    email: string;
    password: string;
}

export interface ExternalLoginCommand {
    idToken: string;
    provider: string;
}

export interface LoginResponse {
    statusCode: number;
    status: string;
    token: string;
    expiration: Date;
}

export interface UserDetailsResponse {
    username: string;
    roles: string[];
    claims: { type: string, value: string }[]
}
