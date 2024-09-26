using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryGuardian
{
    public class ResetManager
    {
        /// we will store all recorded changes
        /// we will then be able to undo them after sorting has happened
        /// we will also provide a UI to show these changes
        /// and ability to jsut restore one file, a category of files, or all files
        /// 

        private readonly DirGuard _dirguard;
        private readonly ILogger _logger;

        public ResetManager(DirGuard dirGuard, ILogger logger) 
        {
            _dirguard = dirGuard;
            _logger = logger;
        }

        List<Record> _changes = new List<Record>();

        public void RecordChange(Record record)
        {
            _changes.Add(record);
        }
    }

    public class Record()
    {
        public string OGpath;
        public string NewPath;
        public DateTime TimeOfChange;
    }
}
