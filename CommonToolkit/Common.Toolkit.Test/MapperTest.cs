using Common.Toolkit.Helper;
using Common.Toolkit.Test.Model;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Common.Toolkit.Test
{

    [TestClass]
    public class MapperTest
    {
        [TestMethod]
        public void MapperAutoMapperTest()
        {
            Stopwatch watch = new Stopwatch();

            //testString
            var data = new MapperModelB
            {
                V = "1",
                V1 = "1",
            };
            var result = MapperHelper.AutoMap<MapperModelA, MapperModelA>(data);
            Console.WriteLine(JsonConvert.SerializeObject(result));
        }

    }
}
