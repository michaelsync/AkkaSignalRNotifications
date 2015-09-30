using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Xunit;

namespace XunitTestsParallel
{
    public class TestClass1
    {
        //public TestClass1()
        //{
        //    var builder = new ContainerBuilder();
        //    builder.RegisterType<DataStore>().As<IDataStore>();

        //    var container = builder.Build();

        //}


        [Fact]
        private void TestClass1TestMethod1() {
            Thread.Sleep(50);
        }

        [Fact]
        private void TestClass1TestMethod2() {
            Thread.Sleep(50);
        }
    }
}
