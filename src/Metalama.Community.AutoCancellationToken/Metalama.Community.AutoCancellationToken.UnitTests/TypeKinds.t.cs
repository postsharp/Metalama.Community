namespace Metalama.Community.AutoCancellationToken.UnitTests.TypeKinds;
// RewriterBase handles classes, structs, records and interfaces, but only classes were ever tested.
[AutoCancellationToken]
internal struct Struct
{
  public async Task RunAsync(System.Threading.CancellationToken cancellationToken = default) => await Helpers.Helper(cancellationToken);
}
[AutoCancellationToken]
internal record Record
{
  public async Task RunAsync(System.Threading.CancellationToken cancellationToken = default) => await Helpers.Helper(cancellationToken);
}
[AutoCancellationToken]
internal record struct RecordStruct
{
  public async Task RunAsync(System.Threading.CancellationToken cancellationToken = default) => await Helpers.Helper(cancellationToken);
}
[AutoCancellationToken]
internal interface IInterface
{
  async Task RunAsync(System.Threading.CancellationToken cancellationToken = default) => await Helpers.Helper(cancellationToken);
}
internal static class Helpers
{
  public static async Task Helper() => await Task.Yield();
  public static async Task Helper(CancellationToken cancellationToken) => await Task.Yield();
}