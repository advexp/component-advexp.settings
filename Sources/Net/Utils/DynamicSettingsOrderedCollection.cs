using System;
using System.Collections.Generic;
using System.Linq;


namespace Advexp
{
    class DynamicSettingsOrderedCollection
    {
        Dictionary<string/*setting name*/, DynamicSettingInfo> m_dynamicSettings = new Dictionary<string, DynamicSettingInfo>();
        List<string> m_dynamicSettingsOrder = new List<string>();

        List<string> m_dynamicSettingsCustomOrder = null;

        //------------------------------------------------------------------------------
        public DynamicSettingsOrderedCollection()
        {
        }

        //------------------------------------------------------------------------------
        public void SetCustomOrder(IEnumerable<string> fullSettingNameCustomOrder)
        {
            if (fullSettingNameCustomOrder != null)
            {
                // берём только то, что есть в каждой коллекции
                var updatedCustomOrder = fullSettingNameCustomOrder.Intersect(m_dynamicSettingsOrder);

                m_dynamicSettingsCustomOrder = new List<string>(updatedCustomOrder);
            }
            else
            {
                m_dynamicSettingsCustomOrder = null;
            }
        }

        //------------------------------------------------------------------------------
        public void Clear()
        {
            m_dynamicSettings.Clear();
            m_dynamicSettingsOrder.Clear();
            m_dynamicSettingsCustomOrder = null;
        }

        //------------------------------------------------------------------------------
        public void Add(string fullSettingName, DynamicSettingInfo dynamicSettingInfo)
        {
            if (m_dynamicSettings.ContainsKey(fullSettingName))
            {
                Debug.Assert(m_dynamicSettingsOrder.Contains(fullSettingName));

                m_dynamicSettings[fullSettingName] = dynamicSettingInfo;
            }
            else
            {
                Debug.Assert(!m_dynamicSettingsOrder.Contains(fullSettingName));

                m_dynamicSettings.Add(fullSettingName, dynamicSettingInfo);
                m_dynamicSettingsOrder.Add(fullSettingName);
            }

            if (m_dynamicSettingsCustomOrder != null)
            {
                m_dynamicSettingsCustomOrder.Add(fullSettingName);
            }
        }

        //------------------------------------------------------------------------------
        public IEnumerable<string> GetFullSettingsNamesWithDefaultOrder()
        {            
            return new List<string>(m_dynamicSettingsOrder);
        }

        //------------------------------------------------------------------------------
        public IEnumerable<string> GetFullSettingsNamesWithCustomOrder()
        {
            if (m_dynamicSettingsCustomOrder == null)
            {
                return null;
            }

            return new List<string>(m_dynamicSettingsCustomOrder);
        }

        //------------------------------------------------------------------------------
        public void Remove(string fullSettingName)
        {
            m_dynamicSettings.Remove(fullSettingName);
            m_dynamicSettingsOrder.Remove(fullSettingName);
            if (m_dynamicSettingsCustomOrder != null)
            {
                m_dynamicSettingsCustomOrder.Remove(fullSettingName);
            }
        }

        //------------------------------------------------------------------------------
        public bool Contains(string fullSettingName)
        {
            Debug.Assert(m_dynamicSettings.ContainsKey(fullSettingName) == m_dynamicSettingsOrder.Contains(fullSettingName));

            if (m_dynamicSettingsCustomOrder != null)
            {
                return m_dynamicSettingsCustomOrder.Contains(fullSettingName);
            }

            return m_dynamicSettings.ContainsKey(fullSettingName);
        }

        //------------------------------------------------------------------------------
        public bool TryGetValue(string fullSettingName, out DynamicSettingInfo value)
        {
            return m_dynamicSettings.TryGetValue(fullSettingName, out value);
        }

        //------------------------------------------------------------------------------
        public DynamicSettingInfo this[string fullSettingName]
        {
            get
            {
                Debug.Assert(m_dynamicSettings.ContainsKey(fullSettingName) == m_dynamicSettingsOrder.Contains(fullSettingName));

                if (m_dynamicSettingsCustomOrder != null)
                {
                    Debug.Assert(m_dynamicSettingsCustomOrder.Contains(fullSettingName));
                }

                return m_dynamicSettings[fullSettingName];
            }
            set
            {
                Debug.Assert(m_dynamicSettings.ContainsKey(fullSettingName) == m_dynamicSettingsOrder.Contains(fullSettingName));

                if (m_dynamicSettingsCustomOrder != null)
                {
                    Debug.Assert(m_dynamicSettingsCustomOrder.Contains(fullSettingName));
                }

                m_dynamicSettings[fullSettingName] = value;
            }
        }

        //------------------------------------------------------------------------------
        public int Count
        {
            get
            {
                Debug.Assert(m_dynamicSettings.Count == m_dynamicSettingsOrder.Count);

                if (m_dynamicSettingsCustomOrder != null)
                {
                    return m_dynamicSettingsCustomOrder.Count;
                }

                return m_dynamicSettings.Count;
            }
        }
    }
}
