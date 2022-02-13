using Fakeneric.Analyzer;
using Fakeneric.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Fakeneric.Test.CSharpAnalyzerVerifier<Fakeneric.Analyzer.FakenericAnalyzer>;

namespace Fakeneric.Test
{
    [TestClass]
    public class ImplementsFixture : Fixture
    {

        [TestMethod]
        public async Task Implements1Test()
        {
            var test = @$"
{await GetPre()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<string>
    {{
    }}

    class GenericType<T>
        : Where<T, Implements<System.Collections.IEnumerable>>
    {{
    }}

}}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task ImplementsWrongType1Test()
        {
            var test = @$"
{await GetPre()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<string>
    {{
    }}

    class GenericType<T>
        : Where<T, Implements<System.Collections.Generic.List<int>>>
    {{
    }}

}}
";

            var expected = VerifyCS.Diagnostic(DiagnosticDescriptors.ConstraintViolationDescriptor)
                .WithLocation(0)
                .WithArguments(ImplementsActualConstraint.GetErrorMessage("string", "global::System.Collections.Generic.List<int>"))
                ;
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task Implements2Test()
        {
            var test = @$"
{await GetPre()}

namespace TestProject
{{
    class {{|#0:DerivedType|}}<T2> : GenericType<string, T2>
    {{
    }}

    class GenericType<T, T2>
        : Where<T, Implements<System.Collections.IEnumerable>>
    {{
    }}

}}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

    }

}
