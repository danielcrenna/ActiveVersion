// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace ActiveVersion
{
	public static class Constants
	{
		public static class ContextKeys
		{
			public const string Version = nameof(Version);
		}

		public static class Versioning
		{
			public const string DefaultVersion = "1.0";
			public const string VersionHeader = "X-Api-Version";
			public const string VersionParameter = "api-version";
			public const string VersionPathPrefix = "v";
			public const string UserVersionClaim = "version";
		}
	}
}