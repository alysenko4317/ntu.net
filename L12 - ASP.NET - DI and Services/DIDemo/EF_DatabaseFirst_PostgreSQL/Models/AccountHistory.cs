using System;
using System.Collections.Generic;

namespace EFDemo_DBFirstPG.Models;

public partial class AccountHistory
{
    public long AccountHistoryId { get; set; }

    public int AccountId { get; set; }

    public string SearchKey { get; set; } = null!;

    public DateOnly SearchDate { get; set; }
}
