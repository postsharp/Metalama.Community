// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Metalama.Framework.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Metalama.Community.Architecture
{
    public class InternalImplementAttribute : TypeAspect
    {
        public override void BuildAspect( IAspectBuilder<INamedType> builder )
        {
            builder.With( t => t ).ValidateReferences( this.ValidateReference, ReferenceKinds.All );
        }

        private void ValidateReference( in ReferenceValidationContext context )
        {
            // TODO: #32419
        }
    }
}
