using System;

namespace Advexp
{
    class ISettingsSerializerImpl : ISettingsSerializerBaseImpl
    {
        InternalApplicationDataContainerSerializer m_ApplicationDataContainerSerializer = null;

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
                throw new ExceptionForUser(new NotSupportedException("Secure settings are not supported by Advexp.Settings for UWP"));
            }
            else
            {
                if (m_ApplicationDataContainerSerializer == null)
                {
                    m_ApplicationDataContainerSerializer = new InternalApplicationDataContainerSerializer();
                }

                serializer = m_ApplicationDataContainerSerializer;
            }

            return serializer;
        }

    }
}
