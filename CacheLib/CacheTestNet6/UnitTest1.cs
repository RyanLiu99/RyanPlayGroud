using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace CacheTestNet6
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestDebugAssert()
        {
            Debug.Assert(true);
            Console.WriteLine("After Debug.Assert(x)");
        }

        [Test]
        public void Test1()
        {
            var exp = () => DoSth("Test");
            exp();

        }

        public async ValueTask<T> WrappedExp<T>(Expression<Func<ValueTask<T>>> exp)
        {
            MethodInfo m = ((MethodCallExpression)exp.Body).Method;

            var arg1 = ((MethodCallExpression)exp.Body).Arguments[0];



            var r = await exp.Compile().Invoke();
            return r;
        }

        private T DoSth<T>(T x)
        {
            return x;
        }
    }
}