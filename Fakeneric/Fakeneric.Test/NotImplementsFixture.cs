using Fakeneric.Analyzer;
using Fakeneric.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Fakeneric.Test.CSharpAnalyzerVerifier<Fakeneric.Analyzer.FakenericAnalyzer>;

namespace Fakeneric.Test
{
    [TestClass]
    public class NotImplementsFixture : Fixture
    {

        [TestMethod]
        public async Task NotImplements1Test()
        {
            var test = @$"
{await GetPre()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<string>
    {{
    }}

    class GenericType<T>
        : Where<T, NotImplements<System.Text.StringBuilder>>
    {{
    }}

}}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task NotImplements2Test()
        {
            var test = @$"
{await GetPre()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<string>
    {{
    }}

    class GenericType<T>
        : Where<T, NotImplements<System.Collections.Generic.List<int>>>
    {{
    }}

}}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task NotImplementsWrongType1Test()
        {
            var test = @$"
{await GetPre()}

namespace TestProject
{{
    class {{|#0:DerivedType|}}<T2> : GenericType<string, T2>
    {{
    }}

    class GenericType<T, T2>
        : Where<T, NotImplements<System.Collections.IEnumerable>>
    {{
    }}

}}
";

            var expected = VerifyCS.Diagnostic(DiagnosticDescriptors.ConstraintViolationDescriptor)
                .WithLocation(0)
                .WithArguments(NotImplementsActualConstraint.GetErrorMessage("string", "global::System.Collections.IEnumerable"))
                ;
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }


    }

}
