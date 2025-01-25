using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApp1
{
  public class RyanCfg
  {
    public const string RyanCfgStr = "RyanCfg";
    public const string RyanCfgSubStr = "RyanCfg:Sub";


    public int CfgNumber { get; set; }
    public string CfgString { get; set; }
    //public RyanCfgSub Sub {get;set;}
  }


  public class RyanCfgSub
  {
    
    public int SubCfgNumber { get; set; }
    public string SubCfgString { get; set; }
    
  }
}
