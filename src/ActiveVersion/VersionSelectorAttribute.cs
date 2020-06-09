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
		public string Suffix { get; }

		public VersionSelectorAttribute(string suffix = "")
		{
			Suffix = suffix;
		}

		public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
		{
			if (!routeContext.HttpContext.Items.TryGetValue(Constants.ContextKeys.Version, out var value))
				return true; // no version context, so we can't rule anything out

			if (!(value is VersionContext versionContext))
				return true; // developer error

			if (!(action is ControllerActionDescriptor controllerActionDescriptor))
				return true; // unexpected level of abstraction

			if (versionContext.Map == null)
				return true; // out of scope: we don't have any wire-ups to inject the data we need

			if (!(controllerActionDescriptor.EndpointMetadata.FirstOrDefault(x => x is VersionHashAttribute) is VersionHashAttribute valueHash))
				return true; // no identifier to make a determination (likely developer error, omitting the value hash on the controller or action)

			var controllerName = controllerActionDescriptor.ControllerName;
			if (!string.IsNullOrEmpty(Suffix) && controllerName.EndsWith(Suffix))
				controllerName = controllerName.Remove(controllerName.Length - Suffix.Length);

			if (!versionContext.Map.ContainsKey(controllerName))
				return true; // out of scope: the version tree doesn't make a determination about this action

			if (!versionContext.Map.TryGetValue(controllerName, out var version))
				return true; // developer error (missing controller version in the map)

			// finally we can determine if this is the intended version or not
			var valid = CompareMajor(version, valueHash) && CompareMinor(version, valueHash);

			return valid;
		}

		private static bool CompareMajor(Version version, VersionHashAttribute fingerprint) => MostlyEqual(version.Major, fingerprint.Major);

		private static bool CompareMinor(Version version, VersionHashAttribute fingerprint) => MostlyEqual(version.Minor.GetValueOrDefault(), fingerprint.Minor);

		private static bool MostlyEqual(ulong version, ulong fingerprint)
		{
			// In case storage is using IEEE 754 (like CosmosDB), we need to compare versus an epsilon
			var delta = Math.Abs((long) (version - fingerprint));
			return delta <= 1000;
		}
	}
}