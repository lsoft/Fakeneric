using Fakeneric.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Fakeneric.Test.CSharpAnalyzerVerifier<Fakeneric.FakenericAnalyzer>;

namespace Fakeneric.Test
{
    [TestClass]
    public class ParameterlessConstructorFixture : Fixture
    {
        [TestMethod]
        public async Task NoConstructorTest()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithNoParameter>
    {{
    }}

    class Payload
    {{
    }}
}}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task PrivateConstructorTest()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithNoParameter>
    {{
    }}

    class Payload
    {{
        private Payload() {{ }}
    }}
}}
";

            var expected = VerifyCS.Diagnostic(DiagnosticDescriptors.ConstraintViolationDescriptor)
                .WithLocation(0)
                .WithArguments(HasConstructorActualConstraint.GetErrorMessage("global::TestProject.Payload()"))
                ;
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task PublicConstructorTest()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithNoParameter>
    {{
    }}

    class Payload
    {{
        public Payload() {{ }}
    }}
}}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task PrivateWrongConstructorTest()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithNoParameter>
    {{
    }}

    class Payload
    {{
        private Payload(object a) {{ }}
    }}
}}
";

            var expected = VerifyCS.Diagnostic(DiagnosticDescriptors.ConstraintViolationDescriptor)
                .WithLocation(0)
                .WithArguments(HasConstructorActualConstraint.GetErrorMessage("global::TestProject.Payload()"))
                ;
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task PublicWrongConstructorTest()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithNoParameter>
    {{
    }}

    class Payload
    {{
        public Payload(object a) {{ }}
    }}
}}
";

            var expected = VerifyCS.Diagnostic(DiagnosticDescriptors.ConstraintViolationDescriptor)
                .WithLocation(0)
                .WithArguments(HasConstructorActualConstraint.GetErrorMessage("global::TestProject.Payload()"))
                ;
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task PublicWrongPrivateCorrectConstructorTest()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithNoParameter>
    {{
    }}

    class Payload
    {{
        private Payload() {{ }}

        public Payload(object a) {{ }}
    }}
}}
";

            var expected = VerifyCS.Diagnostic(DiagnosticDescriptors.ConstraintViolationDescriptor)
                .WithLocation(0)
                .WithArguments(HasConstructorActualConstraint.GetErrorMessage("global::TestProject.Payload()"))
                ;
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }
    }

    public abstract class HasConstructorFixture : Fixture
    {
        [TestClass]
        public class BothProtected : HasConstructorFixture
        {
            protected override string CorrectConstructorVisibility => "protected";
            protected override string WrongConstructorVisibility => "protected";
        }

        [TestClass]
        public class BothInternal : HasConstructorFixture
        {
            protected override string CorrectConstructorVisibility => "internal";
            protected override string WrongConstructorVisibility => "internal";
        }

        [TestClass]
        public class BothInternalProtected : HasConstructorFixture
        {
            protected override string CorrectConstructorVisibility => "internal protected";
            protected override string WrongConstructorVisibility => "internal protected";
        }

        [TestClass]
        public class BothPublic : HasConstructorFixture
        {
            protected override string CorrectConstructorVisibility => "public";
            protected override string WrongConstructorVisibility => "public";
        }

        [TestClass]
        public class PublicPrivate : HasConstructorFixture
        {
            protected override string CorrectConstructorVisibility => "public";
            protected override string WrongConstructorVisibility => "private";
        }


        protected abstract string CorrectConstructorVisibility
        {
            get;
        }

        protected abstract string WrongConstructorVisibility
        {
            get;
        }

        [TestMethod]
        public async Task HasConstructor0Test()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithNoParameter>
    {{
    }}

    class Payload
    {{
    }}
}}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task HasConstructor0WrongTest()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithNoParameter>
    {{
    }}

    class Payload
    {{
        {WrongConstructorVisibility} Payload(object a) {{ }}
    }}
}}
";

            var expected = VerifyCS.Diagnostic(DiagnosticDescriptors.ConstraintViolationDescriptor)
                .WithLocation(0)
                .WithArguments(HasConstructorActualConstraint.GetErrorMessage("global::TestProject.Payload()"))
                ;
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task HasConstructor1Test()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithParameter<byte>>
    {{
    }}

    class Payload
    {{
        {CorrectConstructorVisibility} Payload(byte a) {{ }}
    }}
}}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task HasConstructor1WrongTest()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithParameter<byte>>
    {{
    }}

    class Payload
    {{
        {WrongConstructorVisibility} Payload(object a) {{ }}
    }}
}}
";

            var expected = VerifyCS.Diagnostic(DiagnosticDescriptors.ConstraintViolationDescriptor)
                .WithLocation(0)
                .WithArguments(HasConstructorActualConstraint.GetErrorMessage("global::TestProject.Payload(byte)"))
                ;
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task HasConstructor2Test()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithParameter<byte, short>>
    {{
    }}

    class Payload
    {{
        {CorrectConstructorVisibility} Payload(byte a, short b) {{ }}
    }}
}}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task HasConstructor2WrongTest()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithParameter<byte, short>>
    {{
    }}

    class Payload
    {{
        {WrongConstructorVisibility} Payload(byte a, object b) {{ }}
    }}
}}
";

            var expected = VerifyCS.Diagnostic(DiagnosticDescriptors.ConstraintViolationDescriptor)
                .WithLocation(0)
                .WithArguments(HasConstructorActualConstraint.GetErrorMessage("global::TestProject.Payload(byte, short)"))
                ;
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }


        [TestMethod]
        public async Task HasConstructor3Test()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithParameter<byte, short, int>>
    {{
    }}

    class Payload
    {{
        {CorrectConstructorVisibility} Payload(byte a, short b, int c) {{ }}
    }}
}}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task HasConstructor3WrongTest()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithParameter<byte, short, int>>
    {{
    }}

    class Payload
    {{
        {WrongConstructorVisibility} Payload(byte a, short b, object c) {{ }}
    }}
}}
";

            var expected = VerifyCS.Diagnostic(DiagnosticDescriptors.ConstraintViolationDescriptor)
                .WithLocation(0)
                .WithArguments(HasConstructorActualConstraint.GetErrorMessage("global::TestProject.Payload(byte, short, int)"))
                ;
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }


        [TestMethod]
        public async Task HasConstructor4Test()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithParameter<byte, short, int, long>>
    {{
    }}

    class Payload
    {{
        {CorrectConstructorVisibility} Payload(byte a, short b, int c, long d) {{ }}
    }}
}}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task HasConstructor4WrongTest()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithParameter<byte, short, int, long>>
    {{
    }}

    class Payload
    {{
        {WrongConstructorVisibility} Payload(byte a, short b, int c, object d) {{ }}
    }}
}}
";

            var expected = VerifyCS.Diagnostic(DiagnosticDescriptors.ConstraintViolationDescriptor)
                .WithLocation(0)
                .WithArguments(HasConstructorActualConstraint.GetErrorMessage("global::TestProject.Payload(byte, short, int, long)"))
                ;
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }


        [TestMethod]
        public async Task HasConstructor5Test()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithParameter<byte, short, int, long, sbyte>>
    {{
    }}

    class Payload
    {{
        {CorrectConstructorVisibility} Payload(byte a, short b, int c, long d, sbyte e) {{ }}
    }}
}}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task HasConstructor5WrongTest()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithParameter<byte, short, int, long, sbyte>>
    {{
    }}

    class Payload
    {{
        {WrongConstructorVisibility} Payload(byte a, short b, int c, long d, object e) {{ }}
    }}
}}
";

            var expected = VerifyCS.Diagnostic(DiagnosticDescriptors.ConstraintViolationDescriptor)
                .WithLocation(0)
                .WithArguments(HasConstructorActualConstraint.GetErrorMessage("global::TestProject.Payload(byte, short, int, long, sbyte)"))
                ;
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }


        [TestMethod]
        public async Task HasConstructor6Test()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithParameter<byte, short, int, long, sbyte, ushort>>
    {{
    }}

    class Payload
    {{
        {CorrectConstructorVisibility} Payload(byte a, short b, int c, long d, sbyte e, ushort f) {{ }}
    }}
}}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task HasConstructor6WrongTest()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithParameter<byte, short, int, long, sbyte, ushort>>
    {{
    }}

    class Payload
    {{
        {WrongConstructorVisibility} Payload(byte a, short b, int c, long d, sbyte e, object f) {{ }}
    }}
}}
";

            var expected = VerifyCS.Diagnostic(DiagnosticDescriptors.ConstraintViolationDescriptor)
                .WithLocation(0)
                .WithArguments(HasConstructorActualConstraint.GetErrorMessage("global::TestProject.Payload(byte, short, int, long, sbyte, ushort)"))
                ;
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }


        [TestMethod]
        public async Task HasConstructor7Test()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithParameter<byte, short, int, long, sbyte, ushort, uint>>
    {{
    }}

    class Payload
    {{
        {CorrectConstructorVisibility} Payload(byte a, short b, int c, long d, sbyte e, ushort f, uint g) {{ }}
    }}
}}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task HasConstructor7WrongTest()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithParameter<byte, short, int, long, sbyte, ushort, uint>>
    {{
    }}

    class Payload
    {{
        {WrongConstructorVisibility} Payload(byte a, short b, int c, long d, sbyte e, ushort f, object g) {{ }}
    }}
}}
";

            var expected = VerifyCS.Diagnostic(DiagnosticDescriptors.ConstraintViolationDescriptor)
                .WithLocation(0)
                .WithArguments(HasConstructorActualConstraint.GetErrorMessage("global::TestProject.Payload(byte, short, int, long, sbyte, ushort, uint)"))
                ;
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }


        [TestMethod]
        public async Task HasConstructor8Test()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithParameter<byte, short, int, long, sbyte, ushort, uint, ulong>>
    {{
    }}

    class Payload
    {{
        {CorrectConstructorVisibility} Payload(byte a, short b, int c, long d, sbyte e, ushort f, uint g, ulong j) {{ }}
    }}
}}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task HasConstructor8WrongTest()
        {
            var test = @$"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Fakeneric.Infrastructure;

{await GetInfraAsync()}

namespace TestProject
{{
    class {{|#0:DerivedType|}} : GenericType<Payload>
    {{
    }}

    class GenericType<T>
        : Where<T, HasConstructorWithParameter<byte, short, int, long, sbyte, ushort, uint, ulong>>
    {{
    }}

    class Payload
    {{
        {WrongConstructorVisibility} Payload(byte a, short b, int c, long d, sbyte e, ushort f, uint g, object j) {{ }}
    }}
}}
";

            var expected = VerifyCS.Diagnostic(DiagnosticDescriptors.ConstraintViolationDescriptor)
                .WithLocation(0)
                .WithArguments(HasConstructorActualConstraint.GetErrorMessage("global::TestProject.Payload(byte, short, int, long, sbyte, ushort, uint, ulong)"))
                ;
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }



    }

}
