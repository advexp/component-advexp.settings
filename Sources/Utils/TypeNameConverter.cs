using System;
using System.Text.RegularExpressions;
using Polenter.Serialization.Advanced.Serializing;

namespace Advexp
{
    class TypeNameConverter : ITypeNameConverter
    {
        ITypeNameConverter m_customTypeNameConverter = null;

        //------------------------------------------------------------------------------
        public TypeNameConverter(ITypeNameConverter customTypeNameConverter)
        {
            m_customTypeNameConverter = customTypeNameConverter;
        }

        //------------------------------------------------------------------------------
        ITypeNameConverter GetDefaultTypeNameConverter()
        {
            return new Polenter.Serialization.Advanced.TypeNameConverter(
                SettingsBaseConfiguration.AdvancedConfiguration.SharpSerializerSettings.IncludeAssemblyVersionInTypeName,
                SettingsBaseConfiguration.AdvancedConfiguration.SharpSerializerSettings.IncludeCultureInTypeName,
                SettingsBaseConfiguration.AdvancedConfiguration.SharpSerializerSettings.IncludePublicKeyTokenInTypeName
            );
        }

        #region ITypeNameConverter implementation

        //------------------------------------------------------------------------------
        public Type ConvertToType(string typeName)
        {
            if (String.IsNullOrEmpty(typeName))
            {
                return null;
            }

            typeName = Regex.Replace(typeName, 
                @"Advexp.Wrapper, Advexp.Settings.Utils.(PCL|Standard)",
                @"Advexp.Wrapper, Advexp.Settings.Utils");

            if (m_customTypeNameConverter == null)
            {
                return GetDefaultTypeNameConverter().ConvertToType(typeName);
            }

            return m_customTypeNameConverter.ConvertToType(typeName);
        }

        //------------------------------------------------------------------------------
        public string ConvertToTypeName(Type type)
        {
            if (m_customTypeNameConverter == null)
            {
                return GetDefaultTypeNameConverter().ConvertToTypeName(type);
            }

            return m_customTypeNameConverter.ConvertToTypeName(type);
        }

        #endregion

    }
}
