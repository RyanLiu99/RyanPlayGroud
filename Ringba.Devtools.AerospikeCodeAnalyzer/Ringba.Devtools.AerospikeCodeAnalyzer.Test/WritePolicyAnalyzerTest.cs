using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ringba.Devtools.AerospikeCodeAnalyzer;
using VerifyCS = Ringba.Devtools.AerospikeCodeAnalyzer.Test.CSharpCodeFixVerifier<
    Ringba.Devtools.AerospikeCodeAnalyzer.WritePolicyAnalyzer,
    Ringba.Devtools.AerospikeCodeAnalyzer.WritePolicyCodeFixProvider>;

namespace Ringba.Devtools.AerospikeCodeAnalyzer.Test
{
    [TestClass]
    public class WritePolicyAnalyzerTest
    {
        // No diagnostics expected to show up
        [TestMethod]
        public async Task NoDiagnosticsExpectedWhenNotRelated()
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
        public async Task NoDiagnosticsExpectedWhenDurableDeleteAlreadyTrue()
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