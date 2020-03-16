// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace ActiveVersion
{
	internal static class PathStringExtensions
	{
		public static bool StartsWithAny(this PathString input, IEnumerable<string> stringValues,
			StringComparison comparison = StringComparison.CurrentCulture)
		{
			foreach (var value in stringValues)
			{
				if (input.StartsWithSegments(value, comparison))
					return true;
			}

			return false;
		}
	}
}