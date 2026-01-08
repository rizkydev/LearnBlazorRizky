using System;
using System.Collections.Generic;

namespace RizkyApps.API.Models;

public partial class Exam
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public bool IsRandomQuestion { get; set; }

    public bool IsRandomAnswer { get; set; }

    public bool IsMultipleAttempt { get; set; }

    public string Note { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string ModifiedBy { get; set; } = null!;

    public DateTime ModifiedDate { get; set; }
}
