// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using ActiveVersion.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ActiveVersion
{
	public static class HttpContextExtensions
	{
		public static void SetVersionContext(this HttpContext context, VersionContext versionContext)
		{
			context.Items[Constants.ContextKeys.Version] = versionContext;
			TryAppendVersionHeader(context, versionContext);
		}

		public static VersionContext GetVersionContext(this HttpContext context)
		{
			return context.Items.TryGetValue(Constants.ContextKeys.Version, out var versionContext)
				? versionContext as VersionContext
				: default;
		}

		private static void TryAppendVersionHeader(HttpContext context, VersionContext versionContext)
		{
			var options = context.RequestServices.GetService(typeof(IOptionsSnapshot<VersioningOptions>)) as IOptionsSnapshot<VersioningOptions>;
			if (options != null && !string.IsNullOrWhiteSpace(options.Value.VersionHeader))
			{
				var versionString = versionContext.Group.GroupName;
				context.Response.Headers.TryAdd(options.Value.VersionHeader, versionString);
			}
		}
	}
}