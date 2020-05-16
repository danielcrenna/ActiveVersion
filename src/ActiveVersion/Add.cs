// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using ActiveCaching;
using ActiveOptions;
using ActiveVersion.Configuration;
using ActiveVersion.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ActiveVersion
{
	public static class Add
	{
		public static IServiceCollection AddVersioning(this IServiceCollection services, IConfiguration config)
		{
			return services.AddVersioning(config.FastBind);
		}

		public static IServiceCollection AddVersioning(this IServiceCollection services, Action<VersioningOptions> configureAction = null)
		{
			return services.AddVersioning<DefaultVersionContextResolver>(configureAction);
		}

		public static IServiceCollection AddVersioning<TVersionResolver>(this IServiceCollection services, IConfiguration config) where TVersionResolver : class, IVersionContextResolver
		{
			return services.AddVersioning<TVersionResolver>(config.FastBind);
		}

		public static IServiceCollection AddVersioning<TVersionResolver>(this IServiceCollection services, Action<VersioningOptions> configureAction = null) where TVersionResolver : class, IVersionContextResolver
		{
			services.AddHttpContextAccessor();

			if (configureAction != null)
				services.Configure(configureAction);

			services.AddHttpContextAccessor();
			services.AddInProcessCache();
			services.TryAddSingleton<IVersionContextStore, NoVersionContextStore>();
			services.AddScoped<IVersionContextResolver, TVersionResolver>();
			services.AddScoped(r => r.GetService<IHttpContextAccessor>()?.HttpContext?.GetVersionContext());
			return services;
		}
	}
}