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
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Diagnostics;
//using Fakeneric.Infrastructure;

//{await GetInfraAsync()}

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
