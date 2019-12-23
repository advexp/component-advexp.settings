namespace Advexp
{
    class ISettingsSerializerImpl : ISettingsSerializerBaseImpl
    {
        InternalIsolatedStorageSerializer m_IsolatedStorageSerializer = null;
        DPAPIIsolatedStorageSerializer m_DPAPIIsolatedStorageSerializer = null;

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
                if (m_DPAPIIsolatedStorageSerializer == null)
                {
                    m_DPAPIIsolatedStorageSerializer = new DPAPIIsolatedStorageSerializer();
                }

                serializer = m_DPAPIIsolatedStorageSerializer;
            }
            else
            {
                if (m_IsolatedStorageSerializer == null)
                {
                    m_IsolatedStorageSerializer = new InternalIsolatedStorageSerializer();
                }

                serializer = m_IsolatedStorageSerializer;
            }

            Debug.Assert(serializer != null);

            return serializer;
        }

    }
}
