using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backendclienttesting.Backend.Models
{
    public class MaintenanceCheckResult
    {
        public bool AutoSent { get; set; }
        public bool Warning { get; set; }
        public string Message { get; set; }
    }

}
