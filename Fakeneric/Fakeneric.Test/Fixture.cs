using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Fakeneric.Test
{
    public abstract class Fixture
    {
        protected static async Task<string> GetInfraAsync()
        {
            using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("Fakeneric.Test.Infra.cs"))
            {
                using (var ss = new StreamReader(s))
                {
                    var r = await ss.ReadToEndAsync();
                    return r;
                }
            }
        }
    }

}
