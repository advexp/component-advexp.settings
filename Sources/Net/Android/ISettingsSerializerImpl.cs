namespace Advexp
{
    class ISettingsSerializerImpl : ISettingsSerializerBaseImpl
    {
        KeyChainSerializer m_keyChainSerializer = null;
        InternalSharedPreferencesSerializer m_sharedPreferencesSerializer = null;

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
                    m_keyChainSerializer = new KeyChainSerializer();
                }

                serializer = m_keyChainSerializer;
            }
            else
            {
                if (m_sharedPreferencesSerializer == null)
                {
                    m_sharedPreferencesSerializer = new InternalSharedPreferencesSerializer();
                }

                serializer = m_sharedPreferencesSerializer;
            }

            Debug.Assert(serializer != null);

            return serializer;
        }
    }
}
