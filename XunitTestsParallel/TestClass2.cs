using System.Threading;
using Xunit;

namespace XunitTestsParallel
{
    public class TestClass2
    {
        public TestClass2()
        {
            
        }

        [Fact]
        private void TestClass2TestMethod1()
        {
            Thread.Sleep(100);
        }

        [Fact]
        private void TestClass2TestMethod2() {
            Thread.Sleep(100);
        }
    }
}