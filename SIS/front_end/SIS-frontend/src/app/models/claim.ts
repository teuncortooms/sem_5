import { HasId } from "../interfaces/has-id";

export class Claim implements HasId {
  public id!: string;
  public name!: string;

  public static ClaimToDisplayName(claim: Claim)
  {
    switch(claim.name)
    {
      case "p_all": return "Alle rechten in het systeem";

      case "p_grade_read_own": return "Cijfers inzien (eigen)";
      case "p_grade_read_all": return "Cijfers inzien (iedereen)";
      case "p_grade_write": return "Cijfers wijzigen";
      case "p_grade_create": return "Cijfers toevoegen";
      case "p_grade_delete": return "Cijfers verwijderen";

      case "p_teacher_read": return "Docenten inzien";
      case "p_teacher_write": return "Docenten wijzigen";
      case "p_teacher_create": return "Docenten toevoegen";
      case "p_teacher_delete": return "Docenten verwijderen";

      case "p_admin_user_read": return "Gebruikers inzien";
      case "p_admin_user_create": return "Gebruikers aanmaken";
      case "p_admin_user_delete": return "Gebruikers verwijderen";
      case "p_admin_user_update": return "Gebruikers wijzigen";

      case "p_admin_role_read": return "Gebruikersrollen inzien";
      case "p_admin_role_create": return "Gebruikersrollen aanmaken";
      case "p_admin_role_update": return "Gebruikersrollen wijzigen";
      case "p_admin_role_delete": return "Gebruikersrollen verwijderen";

      case "p_group_read_own": return "Klassen inzien (eigen)";
      case "p_group_read_all": return "Klassen inzien (iedereen)";
      case "p_group_write": return "Klassen wijzigen";
      case "p_group_create": return "Klassen aanmaken";
      case "p_group_delete": return "Klassen verwijderen";

      case "p_student_read_own": return "Studentdata inzien (eigen)";
      case "p_student_read_all": return "Studentdata inzien (iedereen)";
      case "p_student_write": return "Studentdata wijzigen";
      case "p_student_create": return "Studentdata aanmaken";
      case "p_student_delete": return "Studentdata verwijderen";

      default: return "Ongedefinieerde rol ("+claim.name+")";
    }

  }

}
