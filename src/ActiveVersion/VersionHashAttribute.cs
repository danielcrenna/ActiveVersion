// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ActiveVersion
{
	[AttributeUsage(AttributeTargets.All, Inherited = false)]
	public class VersionHashAttribute : Attribute
	{
		public VersionHashAttribute(ulong major, ulong minor = 0)
		{
			Major = major;
			Minor = minor;
		}

		public ulong Major { get; set; }
		public ulong Minor { get; set; }
	}
}