using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace CacheTestNetFramework
{
    internal class DelegatExpressionTest
    {
        private readonly List<object> _callTargets = new List<object>();
        private readonly List<object> _callMethods = new List<object>();
        private readonly List<int> _expsHasCodes = new List<int>();
        private readonly List<MethodInfo> _expsMethod = new List<MethodInfo>();

        private async ValueTask<string> DoSthX()
        {
            return "2";
        }

        private async ValueTask<string> DoSth(string input)
        {
            return "2";
        }

        private async ValueTask<string> DoSth2(string input)
        {
            return "2";
        }


        [TearDown]
        public void TearDown()
        {
            _callTargets.Clear();
            _callMethods.Clear();
            _expsHasCodes.Clear();
            _expsMethod.Clear();
        }

        #region expression
        [Test]
        public async ValueTask ExpressionsCallingSameMethod()
        {
            Expression<Func<ValueTask<string>>> exp1 = () => this.DoSth("a");

            var x = await WrappedExp(exp1);
            var y = await WrappedExp(() => this.DoSth("b"));

            Assert.AreEqual(_expsHasCodes[0], _expsHasCodes[1]); //Calling same method
            Assert.AreEqual(_expsMethod[0], _expsMethod[1]); //Calling same method
        }

        public async ValueTask<T> WrappedExp<T>(Expression<Func<ValueTask<T>>> exp)
        {
            MethodInfo m = ((MethodCallExpression)exp.Body).Method;
            _expsMethod.Add(m);
            _expsHasCodes.Add(m.GetHashCode());

            ConstantExpression arg1 = ((MethodCallExpression)exp.Body).Arguments[0] as ConstantExpression;
            //arg1.Type; //typeof(string)
            //arg1.Value; // "a"

            var r = await exp.Compile().Invoke();
            return r;
        }
        #endregion exp

        #region Func
        [Test]
        public async ValueTask TestMethodInvokingOn()
        {
            var x = await WrappedFunc( this.DoSthX);
            var y = await WrappedFunc( this.DoSthX);

            Assert.AreEqual(2, _callTargets.Count);
            Assert.AreEqual(_callTargets[0], _callTargets[1]); //invoking on the same target ("this")

            Assert.AreEqual(_callMethods[0], _callMethods[1]); //same DoSthX
        }

        [Test]
        public async ValueTask TestMethodInvokingOnSameTargetButDifferentMethod()
        {
            var x = await WrappedFunc(() => this.DoSth("a"));
            var y = await WrappedFunc(() => this.DoSth("a"));

            Assert.AreEqual(2, _callTargets.Count);
            Assert.AreEqual(_callTargets[0], _callTargets[1]); //invoking on the same target ("this")

            Assert.AreNotEqual(_callMethods[0], _callMethods[1]); //not equal even calling same method work since delegate itself is the method. 
        }

        [Test]
        public async ValueTask TestMethodNotEqual()
        {
            var x = await WrappedFunc(() => this.DoSth("a"));
            var y = await WrappedFunc(() => this.DoSth2("a")); //sth 2

            Assert.AreNotEqual(_callMethods[0], _callMethods[1]); //not equal DoSth and DoSth2 are not same
        }

        public async ValueTask<T> WrappedFunc<T>(Func<ValueTask<T>> func)
        {
            _callTargets.Add(func.Target);
            _callMethods.Add(func.Method);

            var r = await func.Invoke();
            return r;
        }
        #endregion


    }
}
