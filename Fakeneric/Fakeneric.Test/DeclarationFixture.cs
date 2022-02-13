using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Fakeneric.Test.CSharpAnalyzerVerifier<Fakeneric.FakenericAnalyzer>;

namespace Fakeneric.Test
{
    [TestClass]
    public class DeclarationFixture : Fixture
    {
//        [TestMethod]
//        public async Task ImplementsSameTest()
//        {
//            var test = @$"
//{await GetPre()}

//namespace TestProject
//{{
//    class {{|#0:GenericType|}}<T> :
//        Where<T, Implements<System.Collections.Generic.List<int>>>
//        , Where<T, Implements<System.Collections.Generic.List<int>>>
//    {{
//    }}

//}}
//";

//            var expected = VerifyCS.Diagnostic(DiagnosticDescriptors.ConstraintDeclarationDescriptor).WithLocation(0);
//            await VerifyCS.VerifyAnalyzerAsync(test, expected);
//        }

}

}
