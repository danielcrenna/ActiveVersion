// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net;
using ActiveRoutes;

namespace ActiveVersion.Configuration
{
	public class VersioningOptions : IFeatureToggle
	{
		public int ExplicitVersionRequiredStatusCode = (int) HttpStatusCode.NotFound;
		public bool RequireExplicitVersion { get; set; } = false;

		public bool EnableVersionHeader { get; set; } = true;
		public string VersionHeader { get; set; } = Constants.Versioning.VersionHeader;

		public bool EnableVersionParameter { get; set; } = true;
		public string VersionParameter { get; set; } = Constants.Versioning.VersionParameter;

		public bool EnableVersionPath { get; set; } = true;
		public string VersionPathPrefix { get; set; } = Constants.Versioning.VersionPathPrefix;

		public bool EnableUserVersions { get; set; } = true;
		public string UserVersionClaim { get; set; } = Constants.Versioning.UserVersionClaim;

		public int? VersionLifetimeSeconds { get; set; } = 180;

		public string[] VersionAgnosticPaths { get; set; } = {"/"};
		public bool Enabled { get; set; } = true;
	}
}