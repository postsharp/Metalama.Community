// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Diagnostics;
using Metalama.Framework.Eligibility;
using System;

namespace Metalama.Community.Architecture
{
    [AttributeUsage( AttributeTargets.All & ~(AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter | AttributeTargets.Assembly | AttributeTargets.Module) )]
    public class ExperimentalAttribute : Attribute, IAspect<IDeclaration>
    {
        private static DiagnosticDefinition<(IDeclaration, DeclarationKind, string?)> _warning = new(
            "LAMA0900",
            Severity.Warning,
            "The '{0}' {1} is experimental.{2}" );

        public string? Description { get; set; }

        public ExperimentalAttribute( string? description = null )
        {
            //this.Description = description;
        }

        public void BuildAspect( IAspectBuilder<IDeclaration> builder )
        {
            builder.Diagnostics.Report( _warning.WithArguments( (builder.Target, builder.Target.DeclarationKind, this.Description) ) );
        }

        public void BuildEligibility( IEligibilityBuilder<IDeclaration> builder )
        {
        }
    }
}