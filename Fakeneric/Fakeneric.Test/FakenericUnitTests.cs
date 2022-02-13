using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using VerifyCS = Fakeneric.Test.CSharpAnalyzerVerifier<Fakeneric.FakenericAnalyzer>;

namespace Fakeneric.Test
{
    [TestClass]
    public class OutOfScopeFixture : Fixture
    {
        [TestMethod]
        public async Task EmptyTest()
        {
            var test = @$"
{await GetPre()}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }


        [TestMethod]
        public async Task NoGenericTest()
        {
            var test = @$"
{await GetPre()}

namespace TestProject
{{
    class NonGenericType
    {{
    }}
}}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task GenericTest()
        {
            var test = @$"
{await GetPre()}

namespace TestProject
{{
    class DerivedType : GenericType<string>
    {{
    }}

    class GenericType<T>
    {{
    }}
}}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task GenericWhereTest()
        {
            var test = @$"
{await GetPre()}

namespace TestProject
{{
    class DerivedType : GenericType<object>
    {{
    }}

    class GenericType<T>
        where T : class, new()
    {{
    }}
}}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

    }

}
