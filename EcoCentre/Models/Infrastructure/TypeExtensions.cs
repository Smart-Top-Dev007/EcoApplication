using System;

namespace EcoCentre.Models.Infrastructure
{
    public static class TypeExtensions
    {

        internal static bool IsDerivativeOfGeneric(this Type t, Type typeToCompare)
        {
            if (t == null) throw new NullReferenceException();
            if (t.BaseType == null) return false;
            if (t.BaseType.IsGenericType)
                t = t.BaseType.GetGenericTypeDefinition();
            return t == typeToCompare || t.BaseType.IsDerivativeOfGeneric(typeToCompare);
        }
    }
}