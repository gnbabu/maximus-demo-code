namespace PDMSRestServices.Models
{
    public class AssignedToResponse
    {
        public string CurrentlyAssignedTo { get; set; }
        public List<AssignableUser> Users { get; set; }
    }

    public class AssignedToUserRequest
    {
        public string RegId { get; set; }
        public string AssignedUser { get; set; }
        public string AssignedUserId { get; set; }
        public string UserId { get; set; }
    }



    public class UpdateCredentialStatusRequest
    {
        public string registrationId { get; set; }
        public int statusId { get; set; }
        public string userId { get; set; }
    }

    public class AssignedToUserResponse
    {
        public string message { get; set; }

    }

    public class UpdateCredentialStatusResponse
    {
        public string message { get; set; }

    }

    public class UserRoleDto
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public bool IsWorkQueueEligible { get; set; }
    }
    public class AssignableUser
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
    }


    public class MembershipUserDto
    {
        public string UserName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public bool IsApproved { get; init; }
        public bool IsLockedOut { get; init; }
        public DateTime CreateDate { get; init; }
        public DateTime LastLoginDate { get; init; }
        public DateTime LastActivityDate { get; init; }
    }

    public class AdminActionModel
    {
        public string Action { get; set; }
    }

    public class AdminActionRequest
    {
        public string Action { get; set; }
        public string Npi { get; set; }
        public string LoggedUser { get; set; }
    }
    public class AdminActionResponse
    {
        public string Message { get; set; }
        public bool isError { get; set; }
    }

}
