// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace ActiveVersion
{
	public interface IVersionContextStore
	{
		bool SupportsFallbackVersion { get; }
		Task<VersionContext> FindByKeyAsync(string versionKey);
	}
}