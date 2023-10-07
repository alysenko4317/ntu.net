using System;
using System.Collections.Generic;

namespace EFDemo_DBFirstApp.Models;

public partial class StudentGroupSet
{
    public int Id { get; set; }

    public string GroupName { get; set; } = null!;

    public virtual ICollection<SubjectSet> Subjects { get; set; } = new List<SubjectSet>();
}
