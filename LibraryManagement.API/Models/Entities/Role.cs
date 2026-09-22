using LibraryManagement.API.Models.Entities;

public class Role
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<MemberRole> MemberRoles { get; set; } = [];
}