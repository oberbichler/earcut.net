using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EarcutNet.Tests
{
    public class JSTests
    {
        static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);

                return Path.GetDirectoryName(path);
            }
        }

        static TestCaseData LoadTestCaseData(string filename, int expectedTriangles, double expectedDeviation = 1e-14)
        {
            var path = Path.Combine(AssemblyDirectory, "fixtures", filename + ".json");

            var polylines = JArray.Parse(File.ReadAllText(path));

            var data = new List<double>();
            var holeIndices = new List<int>();

            foreach (var polyline in polylines)
            {
                if (data.Any())
                {
                    holeIndices.Add(data.Count / 2);
                }

                foreach (var point in polyline)
                {
                    data.Add((double)point[0]);
                    data.Add((double)point[1]);
                }
            }

            return new TestCaseData(data, holeIndices, expectedTriangles, expectedDeviation).SetName(filename);
        }

        static IEnumerable<TestCaseData> TestCases()
        {
            yield return LoadTestCaseData("building", 13);
            yield return LoadTestCaseData("dude", 106);
            yield return LoadTestCaseData("water", 2482, 0.0008);
            yield return LoadTestCaseData("water2", 1212);
            yield return LoadTestCaseData("water3", 197);
            yield return LoadTestCaseData("water3b", 25);
            yield return LoadTestCaseData("water4", 705);
            yield return LoadTestCaseData("water-huge", 5174, 0.0011);
            yield return LoadTestCaseData("water-huge2", 4461, 0.0028);
            yield return LoadTestCaseData("degenerate", 0);
            yield return LoadTestCaseData("bad-hole", 42, 0.019);
            yield return LoadTestCaseData("empty-square", 0);
            yield return LoadTestCaseData("issue16", 12);
            yield return LoadTestCaseData("issue17", 11);
            yield return LoadTestCaseData("steiner", 9);
            yield return LoadTestCaseData("issue29", 40);
            yield return LoadTestCaseData("issue34", 139);
            yield return LoadTestCaseData("issue35", 844);
            yield return LoadTestCaseData("self-touching", 124, 3.4e-14);
            yield return LoadTestCaseData("outside-ring", 64);
            yield return LoadTestCaseData("simplified-us-border", 120);
            yield return LoadTestCaseData("touching-holes", 57);
            yield return LoadTestCaseData("hole-touching-outer", 77);
            yield return LoadTestCaseData("hilbert", 1024);
            yield return LoadTestCaseData("issue45", 10);
            yield return LoadTestCaseData("eberly-3", 73);
            yield return LoadTestCaseData("eberly-6", 1429);
            yield return LoadTestCaseData("issue52", 109);
            yield return LoadTestCaseData("shared-points", 4);
            yield return LoadTestCaseData("bad-diagonals", 7);
            yield return LoadTestCaseData("issue83", 0, 1e-14);
        }

        [TestCaseSource(nameof(TestCases))]
        public static void AreaTest(List<double> data, List<int> holeIndices, int expectedTriangles, double expectedDeviation)
        {
            var triangles = Earcut.Tessellate(data, holeIndices);

            if (expectedTriangles > 0)
            {
                Assert.AreEqual(triangles.Count / 3, expectedTriangles);
            }

            var actualDeviation = Earcut.Deviation(data, holeIndices, triangles);

            Assert.IsTrue(actualDeviation < expectedDeviation);
        }
    }
}
