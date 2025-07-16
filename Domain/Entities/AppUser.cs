using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
public class AppUser : IdentityUser<Guid>
{
    public AppUser() { }

    public AppUser(string email, string firstName, string lastName)
    {
        UserName = email;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int EcoPoints { get; private set; }

    [NotMapped]                         
    public string FullName => $"{FirstName} {LastName}".Trim();
    public void AddEcoPoints(int points)
    {
        if (points < 0) throw new ArgumentOutOfRangeException(nameof(points));
        EcoPoints += points;
    }
}
