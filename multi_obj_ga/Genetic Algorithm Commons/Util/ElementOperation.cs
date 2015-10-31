using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetic_Algorithm_Commons.Util {
    static class IListExtensions {
        public static void Swap<T>(this IList<T> list, int firstIndex, int secondIndex) {
            /*
            if (list == null) {
                throw new ArgumentNullException("list");
            }
            if (firstIndex < 0 || firstIndex >= list.Count) {
                throw new ArgumentOutOfRangeException("firstIndex");
            }
            if (secondIndex < 0 || secondIndex >= list.Count) {
                throw new ArgumentOutOfRangeException("secondIndex");
            }
            if (firstIndex == secondIndex) {
                return;
            }
            */
            T temp = list[firstIndex];
            list[firstIndex] = list[secondIndex];
            list[secondIndex] = temp;
        }
    }

}
