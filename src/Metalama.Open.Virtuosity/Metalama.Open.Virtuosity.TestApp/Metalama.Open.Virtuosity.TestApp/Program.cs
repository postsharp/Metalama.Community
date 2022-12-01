using Moq;

namespace Metalama.Open.Virtuosity.TestApp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var mock = new Mock<Test>();

            mock.Setup( foo => foo.Method() ).Returns( "Mock test" );

            // If the mock succeeded, this should print "Mock test!".
            Console.WriteLine( mock.Object.Method() );
        }
    }
}