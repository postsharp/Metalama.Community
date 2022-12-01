// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using System;
using System.Threading.Tasks;

#pragma warning disable CA1822  // Mark members as static
#pragma warning disable IDE0051 // Remove unused private members

namespace Metalama.Open.Virtuosity.TestApp
{
    [Virtualize]
    internal class C : IDisposable
    {
        // Not transformed.
        private void ImplicitPrivate() { }

        // Not transformed.
        private void ExplicitPrivate() { }

        // Transformed.
        public void Public() { }

        // Not transformed (already virtual).
        public virtual void PublicVirtual() { }

        // Transformed.
        protected async void ProtectedAsync()
        {
            await Task.Yield();
        }

        // Transformed.
        private protected void PrivateProtected() { }

        // Transformed (sealed removed).
        public sealed override string ToString()
        {
            return "";
        }

        // Not transformed.
        public override int GetHashCode()
        {
            return 0;
        }

        // Not transformed.
        public static void PublicStatic() { }

        // Not transformed.
        void IDisposable.Dispose() { }

        // Transformed.
        public int Property { get; }
    }
}