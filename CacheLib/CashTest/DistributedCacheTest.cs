using CashTest.Entity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq.Expressions;

namespace CashTest
{
    internal class DistributedCacheTest
    {
        private readonly List<object> _callTargets = new List<object>();
        private readonly List<object> _callMethods = new List<object>();
        private readonly List<object> _exps = new List<object>();

        [TearDown]
        public void TearDown()
        {
            _callTargets.Clear();
            _callMethods.Clear();
            _exps.Clear();
        }

        [Test]
        public void Test1()
        {
            //var cache = Setup.Container.GetRequiredService<IDistributedCache>();
            var cache = Setup.Container.GetRequiredService<IMemoryCache>();

            string key = "key1";
            Person person1 = new Person("P1");
            cache.Set(key, person1);
            var person1B = cache.Get<Person>(key);
            Assert.NotNull(person1B);
            Assert.AreEqual(person1.Name, person1B.Name);

        }

        [Test]
        public async ValueTask TestMethodNotEqual1()
        {
            var x = await WrappedMethod(() => this.DoSth("a"));
            var y = await WrappedMethod(() => this.DoSth("a"));

            //Assert.AreEqual(2, _callTargets.Count);
            //Assert.AreEqual(_callTargets[0], _callTargets[1]);

            Assert.AreNotEqual(_callMethods[0], _callMethods[1]); //not equal

        }

        [Test]
        public async ValueTask TestMethodNotEqual2()
        {
            var x = await WrappedMethod(() => this.DoSth("a"));
            var y = await WrappedMethod(() => this.DoSth2("a")); //sth 2

            Assert.AreNotEqual(_callMethods[0], _callMethods[1]); //not equal
           
        }

        public async ValueTask<T> WrappedMethod<T>(Func<ValueTask<T>> func)
        {
            _callTargets.Add(func.Target);
            _callMethods.Add(func.Method);
            
            var r  = await func.Invoke();
            return r;
        }


        private async  ValueTask<string> DoSth(string input)
        {
            return "2";
        }

        private async ValueTask<string> DoSth2(string input)
        {
            return "2";
        }


        [Test]
        public async ValueTask TestExpEqual()
        {
            Expression<Func<ValueTask<string>>> exp1 = () => this.DoSth("a");
            Expression<Func<ValueTask<string>>> exp2 = () => this.DoSth("b");

            var x = await WrappedExp(exp1);
            var y = await WrappedExp(exp2);
            
            Assert.AreEqual(_exps[0], _exps[1]);
        }

        [Test]
        public async ValueTask TestExpNotEqual()
        {
            Expression<Func<ValueTask<string>>> exp1 = () => this.DoSth("a");
            Expression<Func<ValueTask<string>>> exp2 = () => this.DoSth2("b");

            var x = await WrappedExp(exp1);
            var y = await WrappedExp(exp2);

            Assert.AreNotEqual(_exps[0], _exps[1]);
        }

        public async ValueTask<T> WrappedExp<T>(Expression<Func<ValueTask<T>>> exp)
        {
            MethodInfo m = ((MethodCallExpression)exp.Body).Method; 
            _exps.Add(m.GetHashCode());
            
            var r = await exp.Compile().Invoke();
            return r;
        }
    }
}
