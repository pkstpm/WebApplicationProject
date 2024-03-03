using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApplicationProject.Models;

namespace WebApplicationProject.Areas.Identity.Data;

// Add profile data for application users by adding properties to the  class
public class ApplicationUser : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string FirstName { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; }
    [PersonalData]
    public Gender Gender { get; set; }

    public ICollection<UserEvent> UserEvents { get; set; }
    public ICollection<Event> Events { get; set; }
}

public enum Gender
{
    Male,
    Female,
    Other
}

