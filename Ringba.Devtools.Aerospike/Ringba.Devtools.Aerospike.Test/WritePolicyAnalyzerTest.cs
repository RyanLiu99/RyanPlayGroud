using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = Ringba.Devtools.Aerospike.Test.CSharpCodeFixVerifier<
    Ringba.Devtools.Aerospike.WritePolicyAnalyzer,
    Ringba.Devtools.Aerospike.WritePolicyCodeFixProvider>;

namespace Ringba.Devtools.Aerospike.Test
{
    [TestClass]
    public class WritePolicyAnalyzerTest
    {
        // No diagnostics expected to show up
        [TestMethod]
        public async Task TestNoDiagnosticsExpectedWhenNotRelated()
        {
            var test = @"
   
    public class TestClass
    {
        public void Method()
        {
            // No WritePolicy usage here
        }
    }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }


        // No diagnostics expected to show up
        [TestMethod]
        public async Task TestNoDiagnosticsExpectedWhenAlreadyHasRightValue()
        {
            var test = @"
    using Aerospike.Client;
    public class TestClass
    {
        public WritePolicy Method()
        {
            var writePolicy = new WritePolicy() { durableDelete = true };
            return writePolicy;
        }
    }";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        // Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task CanChangeDurableDeleteFromFalseToTrue()
        {
            var originalSrc = @"
    using Aerospike.Client;

    public class TestClass
    {
        public WritePolicy Method()
        {
            var writePolicy = new WritePolicy() { durableDelete = false };
            return writePolicy;
        }
    }";

            var updatedSrc = @"
    using Aerospike.Client;

    public class TestClass
    {
        public WritePolicy Method()
        {
            var writePolicy = new WritePolicy() { durableDelete = true };
            return writePolicy;
        }
    }";

            var expectedDiagnostic = VerifyCS.Diagnostic(WritePolicyAnalyzer.DiagnosticId)
                .WithSpan(8, 31, 8, 74) // Specify line and column range of the diagnostic
                .WithArguments("durableDelete");

            await VerifyCS.VerifyCodeFixAsync(originalSrc, expectedDiagnostic, updatedSrc);
        }

        // Diagnostic and CodeFix both triggered and checked for when durableDelete is not set
        [TestMethod]
        public async Task CanAddDurableDeleteWhenNotSet()
        {
            var originalSrc = @"
    using Aerospike.Client;

    public class TestClass
    {
        public WritePolicy Method()
        {
            var writePolicy = new WritePolicy();
            return writePolicy;
        }
    }";

            var updatedSrc = @"
    using Aerospike.Client;

    public class TestClass
    {
        public WritePolicy Method()
        {
            var writePolicy = new WritePolicy() { durableDelete = true };
            return writePolicy;
        }
    }";

            var expectedDiagnostic = VerifyCS.Diagnostic(WritePolicyAnalyzer.DiagnosticId)
                .WithSpan(8, 31, 8, 48) // Specify line and column range of the diagnostic
                .WithArguments("durableDelete");

            await VerifyCS.VerifyCodeFixAsync(originalSrc, expectedDiagnostic, updatedSrc);
        }

       
    }
}