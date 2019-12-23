using System;
using System.Reflection;

namespace Advexp
{
    static class TypeExtentions
    {
        /// <summary>
        /// Returns a representation of the public method associated with the given name in the given type,
        /// including inherited methods.
        /// </summary>
        /// <remarks>
        /// This has a few differences compared with Type.GetMethod in the desktop framework. It will throw
        /// if there is an ambiguous match even between a private method and a public one, but it *won't* throw
        /// if there are two overloads at different levels in the type hierarchy (e.g. class Base declares public void Foo(int) and
        /// class Child : Base declares public void Foo(long)).
        /// </remarks>
        /// <exception cref="AmbiguousMatchException">One type in the hierarchy declared more than one method with the same name</exception>
        public static MethodInfo GetMethod(this Type target, string name)
        {
            // GetDeclaredMethod only returns methods declared in the given type, so we need to recurse.
            while (target != null)
            {
                var typeInfo = target.GetTypeInfo();
                var ret = typeInfo.GetDeclaredMethod(name);
                if (ret != null && ret.IsPublic)
                {
                    return ret;
                }
                target = typeInfo.BaseType;
            }
            return null;
        }
    }
}
