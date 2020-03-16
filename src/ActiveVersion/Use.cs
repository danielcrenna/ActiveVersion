// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using ActiveRoutes;
using ActiveVersion.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ActiveVersion
{
	public static class Use
	{
		// See: https://github.com/Microsoft/api-guidelines/blob/master/Guidelines.md#12-versioning

		public static IApplicationBuilder UseVersioning(this IApplicationBuilder app)
		{
			return app.Use(async (context, next) =>
			{
				if (context.FeatureEnabled<VersioningOptions>(out var options))
				{
					await ExecuteFeature(context, options, next);
				}
				else
				{
					await next();
				}
			});

			static async Task ExecuteFeature(HttpContext c, VersioningOptions o, Func<Task> next)
			{
				var versionResolver = c.RequestServices.GetRequiredService<IVersionContextResolver>();
				var versionContext = await versionResolver.ResolveAsync(c);
				if (versionContext != null && versionContext != VersionContext.None)
				{
					c.SetVersionContext(versionContext);
				}
				else
				{
					if (!o.RequireExplicitVersion ||
					    c.Request.Path.StartsWithAny(o.VersionAgnosticPaths, StringComparison.OrdinalIgnoreCase))
					{
						c.SetVersionContext(VersionContext.None);
					}
					else
					{
						c.Response.StatusCode = o.ExplicitVersionRequiredStatusCode;
						return;
					}
				}

				await next();
			}
		}
	}
}