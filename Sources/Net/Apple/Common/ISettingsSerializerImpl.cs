namespace Advexp
{
    class ISettingsSerializerImpl : ISettingsSerializerBaseImpl
    {
        KeyChainSerializer m_keyChainSerializer = null;
        InternalUserDefaultsSerializer m_userDefaultsSerializer = null;

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
                if (m_keyChainSerializer == null)
                {
                    m_keyChainSerializer = new KeyChainSerializer(
                        secAccessible: SettingsConfiguration.KeyChainSecAccessible, 
                        synchronizable: false);
                }

                serializer = m_keyChainSerializer;
            }
            else
            {
                if (m_userDefaultsSerializer == null)
                {
                    m_userDefaultsSerializer = new InternalUserDefaultsSerializer();
                }

                serializer = m_userDefaultsSerializer;
            }

            Debug.Assert(serializer != null);

            return serializer;
        }
    }
}
