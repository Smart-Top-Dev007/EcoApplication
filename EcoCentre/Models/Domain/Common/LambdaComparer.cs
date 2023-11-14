using System;
using System.Collections.Generic;

namespace EcoCentre.Models.Domain.Common
{
    public class LambdaComparer<T> : IComparer<T>
    {
        private readonly Func<T, T, int> _compare;
        private readonly bool _reverse;

        public LambdaComparer(Func<T,T,int> compare, bool reverse)
        {
            _compare = compare;
            _reverse = reverse;
        }

        public int Compare(T x, T y)
        {
            var result = _compare(x, y);

            return _reverse ? result*-1 : result;
        }
    }
}