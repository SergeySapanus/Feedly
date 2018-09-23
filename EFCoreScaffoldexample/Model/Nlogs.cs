using System;
using System.Collections.Generic;

namespace EFCoreScaffoldexample.Model
{
    public partial class Nlogs
    {
        public int Id { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
    }
}
