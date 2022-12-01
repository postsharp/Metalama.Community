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
        public virtual void Public() { }

        // Not transformed (already virtual).
        public virtual void PublicVirtual() { }

        // Transformed.
        protected async virtual void ProtectedAsync()
        {
            await Task.Yield();
        }

        // Transformed.
        private protected virtual void PrivateProtected() { }

        // Transformed (sealed removed).
        public override string ToString()
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
        public virtual int Property { get; }
    }
}