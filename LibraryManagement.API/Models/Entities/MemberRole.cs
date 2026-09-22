namespace LibraryManagement.API.Models.Entities;

public class MemberRole
{
    public int RoleId { get; set; }
    public int MemberId { get; set; }

    public Role? Role { get; set; }
    public Member? Member { get; set; }
}