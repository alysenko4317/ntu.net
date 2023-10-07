using System;
using System.Collections.Generic;

namespace EFDemo_DBFirstApp.Models;

public partial class SubjectTypeSet
{
    public int Id { get; set; }

    public string TypeName { get; set; } = null!;

    public int? ParentSubjectTypeId { get; set; }

    public virtual ICollection<SubjectTypeSet> InverseParentSubjectType { get; set; } = new List<SubjectTypeSet>();

    public virtual SubjectTypeSet? ParentSubjectType { get; set; }

    public virtual ICollection<SubjectSet> SubjectSets { get; set; } = new List<SubjectSet>();
}
