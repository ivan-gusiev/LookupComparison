using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LookupComparison
{
    /// <summary>
    /// Parallel quicksort algorithm.
    /// </summary>
    public class ParallelSort
    {
        #region Public Static Methods

        /// <summary>
        /// Sequential quicksort.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        public static void QuicksortSequential<T>(T[] arr) where T : IComparable<T>
        {
            QuicksortSequential(arr, 0, arr.Length - 1);
        }

        /// <summary>
        /// Sequential quicksort for two items.
        /// </summary>
        public static void QuicksortSequential<T, U>(T[] arr, U[] arr2) where T : IComparable<T>
        {
            QuicksortSequential(arr, arr2, 0, arr.Length - 1);
        }

        /// <summary>
        /// Parallel quicksort.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        public static void QuicksortParallel<T>(T[] arr) where T : IComparable<T>
        {
            QuicksortParallel(arr, 0, arr.Length - 1);
        }

        /// <summary>
        /// Parallel quicksort for two items.
        /// </summary>
        public static void QuicksortParallel<T, U>(T[] arr, U[] arr2) where T : IComparable<T>
        {
            QuicksortParallel(arr, arr2, 0, arr.Length - 1);
        }

        #endregion

        #region Private Static Methods

        private static void QuicksortSequential<T>(T[] arr, int left, int right)
            where T : IComparable<T>
        {
            if (right > left)
            {
                int pivot = Partition(arr, left, right);
                QuicksortSequential(arr, left, pivot - 1);
                QuicksortSequential(arr, pivot + 1, right);
            }
        }

        private static void QuicksortSequential<T, U>(T[] arr, U[] arr2, int left, int right)
            where T : IComparable<T>
        {
            if (right > left)
            {
                int pivot = Partition(arr, arr2, left, right);
                QuicksortSequential(arr, arr2, left, pivot - 1);
                QuicksortSequential(arr, arr2, pivot + 1, right);
            }
        }

        private static void QuicksortParallel<T>(T[] arr, int left, int right)
            where T : IComparable<T>
        {
            const int SEQUENTIAL_THRESHOLD = 2048;
            if (right > left)
            {
                if (right - left < SEQUENTIAL_THRESHOLD)
                {
                    QuicksortSequential(arr, left, right);
                }
                else
                {
                    int pivot = Partition(arr, left, right);
                    Parallel.Invoke(new Action[] { delegate {QuicksortParallel(arr, left, pivot - 1);},
                                               delegate {QuicksortParallel(arr, pivot + 1, right);}
                });
                }
            }
        }

        private static void QuicksortParallel<T, U>(T[] arr, U[] arr2, int left, int right)
            where T : IComparable<T>
        {
            const int SEQUENTIAL_THRESHOLD = 2048;
            if (right > left)
            {
                if (right - left < SEQUENTIAL_THRESHOLD)
                {
                    QuicksortSequential(arr, arr2, left, right);
                }
                else
                {
                    int pivot = Partition(arr, left, right);
                    Parallel.Invoke(new Action[] { delegate {QuicksortParallel(arr, arr2, left, pivot - 1);},
                                               delegate {QuicksortParallel(arr, arr2, pivot + 1, right);}
                });
                }
            }
        }

        private static void Swap<T>(T[] arr, int i, int j)
        {
            T tmp = arr[i];
            arr[i] = arr[j];
            arr[j] = tmp;
        }
        
        private static void Swap<T, U>(T[] arr, U[] arr2, int i, int j)
        {
            T tmp = arr[i];
            arr[i] = arr[j];
            arr[j] = tmp;

            U tmp2 = arr2[i];
            arr2[i] = arr2[j];
            arr2[j] = tmp2;
        }

        private static int Partition<T>(T[] arr, int low, int high)
            where T : IComparable<T>
        {
            // Simple partitioning implementation
            int pivotPos = (high + low) / 2;
            T pivot = arr[pivotPos];
            Swap(arr, low, pivotPos);

            int left = low;
            for (int i = low + 1; i <= high; i++)
            {
                if (arr[i].CompareTo(pivot) < 0)
                {
                    left++;
                    Swap(arr, i, left);
                }
            }

            Swap(arr, low, left);
            return left;
        }

        private static int Partition<T, U>(T[] arr, U[] arr2, int low, int high)
            where T : IComparable<T>
        {
            // Simple partitioning implementation
            int pivotPos = (high + low) / 2;
            T pivot = arr[pivotPos];
            Swap(arr, arr2, low, pivotPos);

            int left = low;
            for (int i = low + 1; i <= high; i++)
            {
                if (arr[i].CompareTo(pivot) < 0)
                {
                    left++;
                    Swap(arr, arr2, i, left);
                }
            }

            Swap(arr, arr2, low, left);
            return left;
        }

        #endregion
    }
}
