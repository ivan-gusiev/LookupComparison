using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LookupComparison
{
    class TestDriver
    {
        // count of test iterations
        const int Iterations = 1000;

        // max size of array to compare
        const int MaxSize    = 1000000;

        #region Helper random functions
        
        /// <summary>
        /// Creates a random string
        /// </summary>
        static string GetRandomString()
        {
            return Guid.NewGuid().ToString();
        }

        #endregion

        static void Main(string[] args)
        {
            Console.WriteLine("Generating {0:##,#} keys and values...", MaxSize);
            
            var testKeys = Enumerable.Repeat(0, MaxSize).Select(_ => GetRandomString()).ToArray();
            var testValues = testKeys.Select(x => x.GetHashCode()).ToArray();

            for (int i = 10; i <= MaxSize; i *= 10)
            {
                RunTest(testKeys, testValues, i);
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static void RunTest(string[] testKeys, int[] testValues, int iterations)
        {
            Console.WriteLine();
            Console.WriteLine("Running test for {0} elements", iterations);

            var sw = Stopwatch.StartNew();
            var arrayMap = CreateArrayMap2(testKeys, testValues, iterations);
            Console.WriteLine("Creating array-based map took {0} ms", sw.ElapsedMilliseconds);

            sw = Stopwatch.StartNew();
            var hashMap = CreateHashMap(testKeys, testValues, iterations);
            Console.WriteLine("Creating hash-based map took {0} ms", sw.ElapsedMilliseconds);

            sw = Stopwatch.StartNew();
            var treeMap = CreateTreeMap(testKeys, testValues, iterations);
            Console.WriteLine("Creating tree-based map took {0} ms", sw.ElapsedMilliseconds);

            sw = Stopwatch.StartNew();
            var sortedListMap = CreateSortedListMap(testKeys, testValues, iterations);
            Console.WriteLine("Creating sortedlist-based map took {0} ms", sw.ElapsedMilliseconds);
        }

        static Tuple<string[], int[]> CreateArrayMap(string[] testKeys, int[] testValues, int elementCount)
        {
            var keys = testKeys.Take(elementCount).ToArray();
            var values = testValues.Take(elementCount).ToArray();
            
            Array.Sort(keys, values);
            return Tuple.Create(keys, values);
        }

        static void LookupArrayMap(string[] testKeys, int[] testValues, Tuple<string[], int[]> map, int iterationCount)
        {
            for (int i = 0; i < iterationCount; i++)
            {
                var testKey = testKeys[i];
                var expectedValue = testValues[i];

                var actualValue = 
            }
        }

        static Tuple<string[], int[]> CreateArrayMap2(string[] testKeys, int[] testValues, int elementCount)
        {
            var keys = testKeys.Take(elementCount).ToArray();
            var values = testValues.Take(elementCount).ToArray();

            ParallelSort.QuicksortParallel(keys, values);
            return Tuple.Create(keys, values);
        }

        static Dictionary<string, int> CreateHashMap(string[] testKeys, int[] testValues, int elementCount)
        {
            var dic = new Dictionary<string, int>(elementCount);

            for (int i = 0; i < elementCount; i++)
            {
                dic.Add(testKeys[i], testValues[i]);
            }

            return dic;
        }

        static SortedDictionary<string, int> CreateTreeMap(string[] testKeys, int[] testValues, int elementCount)
        {
            var dic = new SortedDictionary<string, int>();

            for (int i = 0; i < elementCount; i++)
            {
                dic.Add(testKeys[i], testValues[i]);
            }

            return dic;
        }

        static SortedList<string, int> CreateSortedListMap(string[] testKeys, int[] testValues, int elementCount)
        {
            var tmp = CreateHashMap(testKeys, testValues, elementCount);
            var dic = new SortedList<string, int>(tmp);
            return dic;
        }
    }
}
