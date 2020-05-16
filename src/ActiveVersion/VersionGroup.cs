// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace ActiveVersion
{
	[DebuggerDisplay("{" + nameof(GroupName) + "}")]
	public struct VersionGroup
	{
		public string GroupName { get; }
		public VersionGroup(short year, byte month, byte day) : this($"{year:D4}-{month:D2}-{day:D2}") { }
		public VersionGroup(string groupName) => GroupName = groupName;
	}
}