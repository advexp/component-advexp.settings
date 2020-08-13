using System;
using System.Reflection;

namespace Advexp
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class SerializerAttribute : Attribute
    {
        Type m_SerializerType = null;

        //------------------------------------------------------------------------------
        public Type Type 
        {
            get
            {
                return m_SerializerType;
            }
            set
            {
                m_SerializerType = value;
            }
        }

        //------------------------------------------------------------------------------
        public SerializerAttribute()
        {
        }

        //------------------------------------------------------------------------------
        public SerializerAttribute(Type serializerType)
        {
            Type = serializerType;
        }
    }
}

