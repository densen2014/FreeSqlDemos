using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AME.Models.Dto; 

namespace AME.Models.Restaurant
{
 
 
    [JsonObject(MemberSerialization.OptIn), Table(Name = "Customers", DisableSyncStructure = true)]
    public partial class CustomerGroupDto
    {
        public string CusGroupname { get; set; }
        public int CusGroup { get; set; }
        public bool UsePrice2 { get; set; } 
    }
}

