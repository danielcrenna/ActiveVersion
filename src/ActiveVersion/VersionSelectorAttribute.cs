// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;

namespace ActiveVersion
{
	public class VersionSelectorAttribute : ActionMethodSelectorAttribute
	{
		public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
		{
			if (!routeContext.HttpContext.Items.TryGetValue(Constants.ContextKeys.Version, out var value))
				return true; // no version context, so we can't rule anything out

			if (!(value is VersionContext versionContext))
				return true; // developer error?

			if (!(action is ControllerActionDescriptor controllerActionDescriptor))
				return true; // unexpected level of abstraction

			if (versionContext.Map == null)
				return true; // out of scope: we don't have any wire-ups to inject the data we need

			if (!versionContext.Map.ContainsKey(controllerActionDescriptor.ControllerName))
				return true; // out of scope: the version tree doesn't make a determination about this action

			if (!(controllerActionDescriptor.EndpointMetadata.FirstOrDefault(x => x is FingerprintAttribute) is
				FingerprintAttribute fingerprint))
				return true; // no identifier to make a determination

			if (!versionContext.Map.TryGetValue(controllerActionDescriptor.ControllerName, out var version))
				return true; // developer error?

			// finally we can determine if this is the intended version or not
			var isValidForRequest = CompareMajor(version, fingerprint) && CompareMinor(version, fingerprint);

			return isValidForRequest;
		}

		private static bool CompareMajor(Version version, FingerprintAttribute fingerprint)
		{
			return MostlyEqual(version.Major, fingerprint.Major);
		}

		private static bool CompareMinor(Version version, FingerprintAttribute fingerprint)
		{
			return MostlyEqual(version.Minor.GetValueOrDefault(), fingerprint.Minor);
		}

		private static bool MostlyEqual(ulong version, ulong fingerprint)
		{
			// In case storage is using IEEE 754 (like CosmosDB), we need to compare versus an epsilon
			var delta = Math.Abs((long) (version - fingerprint));
			return delta <= 1000;
		}
	}
}