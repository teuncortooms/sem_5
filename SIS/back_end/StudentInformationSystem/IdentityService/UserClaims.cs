using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService
{
    public class UserClaims
    {
        /// <summary>
        /// All policies also include the All token; the first user that logs in will receive the p_all token if not already given out.
        /// </summary>
        public const string All = "p_all";

        public const string GradeReadOwn = "p_grade_read_own";
        public const string GradeReadAll = "p_grade_read_all";
        public const string GradeWrite = "p_grade_write";
        public const string GradeCreate = "p_grade_create";
        public const string GradeDelete = "p_grade_delete";

        public const string GroupReadOwn = "p_group_read_own";
        public const string GroupReadAll = "p_group_read_all";
        public const string GroupWrite = "p_group_write";
        public const string GroupCreate = "p_group_create";
        public const string GroupDelete = "p_group_delete";

        public const string StudentReadOwn = "p_student_read_own";
        public const string StudentReadAll = "p_student_read_all";
        public const string StudentWrite = "p_student_write";
        public const string StudentCreate = "p_student_create";
        public const string StudentDelete = "p_student_delete";

        public const string AdminUsersRead = "p_admin_user_read";
        public const string AdminUsersCreate = "p_admin_user_create";
        public const string AdminUserDelete = "p_admin_user_delete";
        public const string AdminUserUpdate = "p_admin_user_update";

        public const string AdminRolesRead = "p_admin_role_read";
        public const string AdminRolesCreate = "p_admin_role_create";
        public const string AdminRolesUpdate = "p_admin_role_update";
        public const string AdminRolesDelete = "p_admin_role_delete";

        public const string TeacherRead = "p_teacher_read";
        public const string TeacherWrite = "p_teacher_write";
        public const string TeacherCreate = "p_teacher_create";
        public const string TeacherDelete = "p_teacher_delete";

        public const string StudentId = "student_id";

        /// <summary>
        /// All claims permissions that are registered as Policies can be used as in mvc controller authorized policy filters.
        /// </summary>
        public readonly static string[] Permissions =
        {
            AdminUsersRead,
            AdminUsersCreate,
            AdminUserDelete,
            AdminUserUpdate,

            AdminRolesRead,
            AdminRolesCreate,
            AdminRolesUpdate,
            AdminRolesDelete,

            TeacherRead,
            TeacherWrite,
            TeacherCreate,
            TeacherDelete,

            GradeReadOwn,
            GradeReadAll,
            GradeWrite,
            GradeCreate,
            GradeDelete,

            GroupReadOwn,
            GroupReadAll,
            GroupWrite,
            GroupCreate,
            GroupDelete,

            StudentReadOwn,
            StudentReadAll,
            StudentWrite,
            StudentCreate,
            StudentDelete
        };

        /// <summary>
        /// Convert text claim to SIS-Claims application claim for use with Identity Framework.
        /// </summary>
        /// <param name="userClaim"></param>
        /// <returns>Claim</returns>
        public static Claim Claim(string userClaim)
        {
            return new Claim("SIS-Claims", userClaim);
        }

    }

    
}
