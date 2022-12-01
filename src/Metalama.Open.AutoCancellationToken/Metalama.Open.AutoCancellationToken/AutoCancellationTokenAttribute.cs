// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Metalama.Framework.Aspects;
using System;

namespace Metalama.Open.AutoCancellationToken;

[AttributeUsage( AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface )]
[RequireAspectWeaver( "Metalama.Open.AutoCancellationToken.Weaver.AutoCancellationTokenWeaver" )]
public class AutoCancellationTokenAttribute : TypeAspect { }