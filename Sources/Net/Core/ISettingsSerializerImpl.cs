using System;

namespace Advexp
{
    class ISettingsSerializerImpl : ISettingsSerializerBaseImpl
    {
        InternalIsolatedStorageSerializer m_IsolatedStorageSerializer = null;

        //------------------------------------------------------------------------------
        // issue4 Default constructor not found for type Advexp.ISettingsSerializerImpl
        // https://bitbucket.org/advexp/component-advexp.settings/issues/4/default-constructor-not-found-for-type
        [Advexp.Preserve]
        public ISettingsSerializerImpl()
        {
        }

        //------------------------------------------------------------------------------
        protected override IInternalSettingsSerializer GetSerializer(bool secure)
        {
            IInternalSettingsSerializer serializer = null;

            if (secure)
            {
                throw new ExceptionForUser(new NotSupportedException("Secure settings are not supported by Advexp.Settings for Windows Core"));
            }
            else
            {
                if (m_IsolatedStorageSerializer == null)
                {
                    m_IsolatedStorageSerializer = new InternalIsolatedStorageSerializer();
                }

                serializer = m_IsolatedStorageSerializer;
            }

            return serializer;
        }

    }
}
