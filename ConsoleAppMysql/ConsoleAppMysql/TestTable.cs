using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace MyTestDB {

	[JsonObject(MemberSerialization.OptIn)]
	public partial class TestTable {

		[JsonProperty, Column(IsPrimary = true, IsIdentity = true)]
		public int id { get; set; }

		[JsonProperty]
		public string remark { get; set; }

	}

}
