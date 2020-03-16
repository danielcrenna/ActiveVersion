// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace ActiveVersion
{
	public class VersionContext
	{
		public static VersionContext None = new VersionContext();
		public VersionGroup Group { get; set; }
		public Dictionary<string, Version> Map { get; set; }
		public string[] Identifiers { get; set; }
	}
}