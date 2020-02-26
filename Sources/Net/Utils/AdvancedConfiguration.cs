using Polenter.Serialization;

namespace Advexp
{
    public class AdvancedConfiguration
    {
        SharpSerializerBinarySettings m_SharpSerializerSettings = null;

        public SharpSerializerBinarySettings SharpSerializerSettings
        {
            get
            {
                if (m_SharpSerializerSettings == null)
                {
                    m_SharpSerializerSettings = new SharpSerializerBinarySettings()
                    {
                        Mode = BinarySerializationMode.SizeOptimized,
                    };
                }

                return m_SharpSerializerSettings;
            }
            set
            {
                m_SharpSerializerSettings = value;
            }
        }
    }
}

