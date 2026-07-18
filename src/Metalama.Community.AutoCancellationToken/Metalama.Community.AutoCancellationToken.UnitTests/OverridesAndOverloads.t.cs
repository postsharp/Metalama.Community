namespace Metalama.Community.AutoCancellationToken.UnitTests.OverridesAndOverloads;
internal interface IRunner
{
  Task RunAsync();
}
internal abstract class RunnerBase
{
  public abstract Task ExecuteAsync();
  public virtual Task VirtualAsync() => Task.CompletedTask;
}
// Regression test for #105: adding a parameter to a member that overrides or implements another member severs that
// relationship, and adding one that duplicates an existing overload is a duplicate member.
[AutoCancellationToken]
internal class Runner : RunnerBase, IRunner
{
  // Not transformed: overrides an abstract member.
  public override async Task ExecuteAsync() => await Task.Yield();
  // Not transformed: overrides a virtual member.
  public override async Task VirtualAsync() => await Task.Yield();
  // Not transformed: implicitly implements IRunner.RunAsync.
  public async Task RunAsync() => await Task.Yield();
  // Not transformed: the type already declares ExistingOverloadAsync(CancellationToken).
  public async Task ExistingOverloadAsync() => await Task.Yield();
  public async Task ExistingOverloadAsync(CancellationToken cancellationToken) => await Task.Yield();
  // Transformed: an ordinary async method is unaffected by the new guards.
  public async Task PlainAsync(System.Threading.CancellationToken cancellationToken = default) => await Task.Yield();
}
// Not transformed: explicit interface implementation.
[AutoCancellationToken]
internal class ExplicitRunner : IRunner
{
  async Task IRunner.RunAsync() => await Task.Yield();
}