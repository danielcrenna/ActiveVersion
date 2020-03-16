// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;

namespace ActiveVersion
{
	public static class HttpContextExtensions
	{
		public static void SetVersionContext(this HttpContext context, VersionContext versionContext)
		{
			context.Items[Constants.ContextKeys.Version] = versionContext;
		}

		public static VersionContext GetVersionContext(this HttpContext context)
		{
			return context.Items.TryGetValue(Constants.ContextKeys.Version, out var versionContext)
				? versionContext as VersionContext
				: default;
		}
	}
}