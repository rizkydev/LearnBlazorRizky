using System;
using System.Collections.Generic;

namespace RizkyApps.API.Models;

public partial class MsUser
{
    public long Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Name { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public DateTime? Birthday { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
