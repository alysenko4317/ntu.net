using System;
using System.Collections.Generic;

namespace EFDemo_DBFirstApp.Models;

public partial class SubjectSet
{
    public int Id { get; set; }

    public string SubjectName { get; set; } = null!;

    public int Value { get; set; }

    public int SubjectTypeId { get; set; }

    public virtual SubjectTypeSet SubjectType { get; set; } = null!;

    public virtual ICollection<StudentGroupSet> StudentGroups { get; set; } = new List<StudentGroupSet>();
}
