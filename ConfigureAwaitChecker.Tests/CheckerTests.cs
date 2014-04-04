﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ConfigureAwaitChecker.Tests.TestClasses;
using NUnit.Framework;

namespace ConfigureAwaitChecker.Tests
{
    [TestFixture]
    public class CheckerTests
    {
        static string File(string className)
        {
            var location = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase), "TestClasses", string.Format("{0}.cs", className));
            return location.Replace(@"file:\", string.Empty);
        }

        static Checker CreateChecker<T>() where T : TestClassBase
        {
            return new Checker(File(typeof(T).Name));
        }

        static string Dump(IEnumerable<CheckerResult> results)
        {
            var sb = new StringBuilder();
            foreach (var item in results)
            {
                sb.AppendFormat("Result:{0}\tL:{1,-6}|C:{2}", 
                    item.HasConfigureAwaitFalse,
                    item.Line,
                    item.Column);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        [Test]
        public void SimpleAwait_Missing()
        {
            var result = CreateChecker<SimpleAwait_Missing>().Check().ToArray();
            Console.WriteLine(Dump(result));
            Assert.IsFalse(result[0].HasConfigureAwaitFalse);
        }
        [Test]
        public void SimpleAwait_Fine()
        {
            var result = CreateChecker<SimpleAwait_Fine>().Check().ToArray();
            Console.WriteLine(Dump(result));
            Assert.IsTrue(result[0].HasConfigureAwaitFalse);
        }

        [Test]
        public void SimpleAwaitWithBraces_Missing()
        {
            var result = CreateChecker<SimpleAwaitWithBraces_Missing>().Check().ToArray();
            Console.WriteLine(Dump(result));
            Assert.IsFalse(result[0].HasConfigureAwaitFalse);
        }
        [Test]
        public void SimpleAwaitWithBracesAll_Fine()
        {
            var result = CreateChecker<SimpleAwaitWithBracesAll_Fine>().Check().ToArray();
            Console.WriteLine(Dump(result));
            Assert.IsTrue(result[0].HasConfigureAwaitFalse);
        }
        [Test]
        public void SimpleAwaitWithBracesTask_Fine()
        {
            var result = CreateChecker<SimpleAwaitWithBracesTask_Fine>().Check().ToArray();
            Console.WriteLine(Dump(result));
            Assert.IsTrue(result[0].HasConfigureAwaitFalse);
        }

        [Test]
        public void AwaitInIf_Missing()
        {
            var result = CreateChecker<AwaitInIf_Missing>().Check().ToArray();
            Console.WriteLine(Dump(result));
            Assert.IsFalse(result[0].HasConfigureAwaitFalse);
        }
        [Test]
        public void AwaitInIf_Fine()
        {
            var result = CreateChecker<AwaitInIf_Fine>().Check().ToArray();
            Console.WriteLine(Dump(result));
            Assert.IsTrue(result[0].HasConfigureAwaitFalse);
        }

        [Test]
        public void AwaitInUsing_Missing()
        {
            var result = CreateChecker<AwaitInUsing_Missing>().Check().ToArray();
            Console.WriteLine(Dump(result));
            Assert.IsFalse(result[0].HasConfigureAwaitFalse);
        }
        [Test]
        public void AwaitInUsing_Fine()
        {
            var result = CreateChecker<AwaitInUsing_Fine>().Check().ToArray();
            Console.WriteLine(Dump(result));
            Assert.IsTrue(result[0].HasConfigureAwaitFalse);
        }

        [Test]
        public void CallOnResult_Missing()
        {
            var result = CreateChecker<CallOnResult_Missing>().Check().ToArray();
            Console.WriteLine(Dump(result));
            Assert.IsFalse(result[0].HasConfigureAwaitFalse);
        }
        [Test]
        public void CallOnResult_Fine()
        {
            var result = CreateChecker<CallOnResult_Fine>().Check().ToArray();
            Console.WriteLine(Dump(result));
            Assert.IsTrue(result[0].HasConfigureAwaitFalse);
        }

        [Test]
        public void NestedFunctionCalls_MissingAll()
        {
            var result = CreateChecker<NestedFunctionCalls_MissingAll>().Check().ToArray();
            Console.WriteLine(Dump(result));
            Assert.IsFalse(result[0].HasConfigureAwaitFalse);
            Assert.IsFalse(result[1].HasConfigureAwaitFalse);
        }
        [Test]
        public void NestedFunctionCalls_MissingInner()
        {
            var result = CreateChecker<NestedFunctionCalls_MissingInner>().Check().ToArray();
            Console.WriteLine(Dump(result));
            Assert.IsTrue(result[0].HasConfigureAwaitFalse);
            Assert.IsFalse(result[1].HasConfigureAwaitFalse);
        }
        [Test]
        public void NestedFunctionCalls_MissingOuter()
        {
            var result = CreateChecker<NestedFunctionCalls_MissingOuter>().Check().ToArray();
            Console.WriteLine(Dump(result));
            Assert.IsFalse(result[0].HasConfigureAwaitFalse);
            Assert.IsTrue(result[1].HasConfigureAwaitFalse);
        }
        [Test]
        public void NestedFunctionCalls_Fine()
        {
            var result = CreateChecker<NestedFunctionCalls_Fine>().Check().ToArray();
            Console.WriteLine(Dump(result));
            Assert.IsTrue(result[0].HasConfigureAwaitFalse);
            Assert.IsTrue(result[1].HasConfigureAwaitFalse);
        }

        [Test]
        public void SimpleLambda_Missing()
        {
            var result = CreateChecker<SimpleLambda_Missing>().Check().ToArray();
            Console.WriteLine(Dump(result));
            Assert.IsFalse(result[0].HasConfigureAwaitFalse);
        }
        [Test]
        public void SimpleLambda_Fine()
        {
            var result = CreateChecker<SimpleLambda_Fine>().Check().ToArray();
            Console.WriteLine(Dump(result));
            Assert.IsTrue(result[0].HasConfigureAwaitFalse);
        }

        [Test]
        public void SimpleLambdaWithBraces_Missing()
        {
            var result = CreateChecker<SimpleLambdaWithBraces_Missing>().Check().ToArray();
            Console.WriteLine(Dump(result));
            Assert.IsFalse(result[0].HasConfigureAwaitFalse);
        }
        [Test]
        public void SimpleLambdaWithBraces_Fine()
        {
            var result = CreateChecker<SimpleLambdaWithBraces_Fine>().Check().ToArray();
            Console.WriteLine(Dump(result));
            Assert.IsTrue(result[0].HasConfigureAwaitFalse);
        }

        [Test]
        public void ExecutingAsyncLambda_MissingAll()
        {
            var result = CreateChecker<ExecutingAsyncLambda_MissingAll>().Check().ToArray();
            Console.WriteLine(Dump(result));
            Assert.IsFalse(result[0].HasConfigureAwaitFalse);
            Assert.IsFalse(result[1].HasConfigureAwaitFalse);
        }
        [Test]
        public void ExecutingAsyncLambda_MissingInner()
        {
            var result = CreateChecker<ExecutingAsyncLambda_MissingInner>().Check().ToArray();
            Console.WriteLine(Dump(result));
            Assert.IsTrue(result[0].HasConfigureAwaitFalse);
            Assert.IsFalse(result[1].HasConfigureAwaitFalse);
        }
        [Test]
        public void ExecutingAsyncLambda_MissingOuter()
        {
            var result = CreateChecker<ExecutingAsyncLambda_MissingOuter>().Check().ToArray();
            Console.WriteLine(Dump(result));
            Assert.IsFalse(result[0].HasConfigureAwaitFalse);
            Assert.IsTrue(result[1].HasConfigureAwaitFalse);
        }
        [Test]
        public void ExecutingAsyncLambda_Fine()
        {
            var result = CreateChecker<ExecutingAsyncLambda_Fine>().Check().ToArray();
            Console.WriteLine(Dump(result));
            Assert.IsTrue(result[0].HasConfigureAwaitFalse);
            Assert.IsTrue(result[1].HasConfigureAwaitFalse);
        }
    }
}
