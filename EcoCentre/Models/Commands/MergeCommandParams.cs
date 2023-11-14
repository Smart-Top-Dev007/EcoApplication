using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoCentre.Models.Commands
{
    public class MergeCommandParams
    {
        public string MergeSourcesStr { get; set; }
        public string MergeDest { get; set; }
    }
}