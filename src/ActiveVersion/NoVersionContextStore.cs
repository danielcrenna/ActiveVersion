// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace ActiveVersion
{
	public class NoVersionContextStore : IVersionContextStore
	{
		private static readonly Task<VersionContext> None = Task.FromResult(VersionContext.None);

		public bool SupportsFallbackVersion => false;

		public Task<VersionContext> FindByKeyAsync(string versionKey)
		{
			return None;
		}
	}
}