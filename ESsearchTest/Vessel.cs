using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESsearchTest
{
    [ElasticsearchType(Name = "allnames")]
    public class Vessel
    {

        [String(Name = "cfr")]  public string cfr { get; set; }

        [String(Name = "CountryCode")] public string CountryCode { get; set; }
        [String(Name = "VesselName")] public string VesselName { get; set; }

        [String(Name = "ExactName")] public string ExactName { get; set; }
        [String(Name = "PortCode")] public string PortCode { get; set; }
        [String(Name = "PortName")] public string PortName { get; set; }
        [String(Name = "Loa")] public string Loa { get; set; }
        [String(Name = "Lbp")] public string Lbp { get; set; }
        [String(Name = "TonRef")] public string TonRef { get; set; }
        [String(Name = "PowerMain")] public string PowerMain { get; set; }
    }

}
