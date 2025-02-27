﻿using System;
namespace backend.Models.Request
{
	public class CreateTableRequest
	{
		public string name { get; set; }
		public List<Inputs> inputs { get; set; }
	}

	public class Inputs {
		public string name { get; set; }
		public string type { get; set; }
	}
}

