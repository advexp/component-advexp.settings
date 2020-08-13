using System;
using System.Reflection;

namespace Advexp
{
    class MethodSerializer : ISettingsSerializer
    {
        object m_serializerObj;

        string m_saveMethod;
        string m_loadMethod;
        string m_deleteMethod;
        string m_containsMethod;
        string m_synchronizeMethod;

        //------------------------------------------------------------------------------
        public MethodSerializer(MethodSerializerAttribute attribute, object serializerObj)
        {
            UpdateMethods(attribute);

            m_serializerObj = serializerObj;
        }

        //------------------------------------------------------------------------------
        public void UpdateMethods(MethodSerializerAttribute attribute)
        {
            m_loadMethod = attribute.LoadMethod;
            m_saveMethod = attribute.SaveMethod;
            m_deleteMethod = attribute.DeleteMethod;
            m_containsMethod = attribute.ContainsMethod;
            m_synchronizeMethod = attribute.SynchronizeMethod;
        }

        #region ISettingsSerializer implementation

        //------------------------------------------------------------------------------
        public bool Load(string settingName, bool secure, SettingBaseAttribute attr, out object value)
        {
            value = null;

            if (String.IsNullOrEmpty(m_loadMethod))
            {
                return false;
            }

            var methodParams = new object[] {settingName, secure, attr, null};
            bool loaded = (bool)Helper(m_loadMethod, methodParams);
            if (loaded)
            {
                value = methodParams[3];
            }

            return loaded;
        }

        //------------------------------------------------------------------------------
        public void Save(string settingName, bool secure, SettingBaseAttribute attr, object value)
        {
            if (String.IsNullOrEmpty(m_saveMethod))
            {
                return;
            }

            var methodParams = new object[] {settingName, secure, attr, value};
            Helper(m_saveMethod, methodParams);
        }

        //------------------------------------------------------------------------------
        public void Delete(string settingName, bool secure, SettingBaseAttribute attr)
        {
            if (String.IsNullOrEmpty(m_deleteMethod))
            {
                return;
            }

            var methodParams = new object[] {settingName, secure, attr};
            Helper(m_deleteMethod, methodParams);
        }

        //------------------------------------------------------------------------------
        public bool Contains(string settingName, bool secure, SettingBaseAttribute attr)
        {
            if (String.IsNullOrEmpty(m_containsMethod))
            {
                return false;
            }

            var methodParams = new object[] { settingName, secure, attr };
            return (bool)Helper(m_containsMethod, methodParams);
        }

        //------------------------------------------------------------------------------
        public void Synchronize()
        {
            if (String.IsNullOrEmpty(m_synchronizeMethod))
            {
                return;
            }

            var methodParams = new object[] {};
            Helper(m_synchronizeMethod, methodParams);
        }

        #endregion

        //------------------------------------------------------------------------------
        object Helper(string methodName, object[] methodParams)
        {
            string exceptionMsg = String.Format("cant find method: {0} {1}", m_serializerObj.GetType().FullName, methodName);

            var methods = m_serializerObj.GetType().GetTypeInfo().GetDeclaredMethods(methodName);
            if (methods == null)
            {
                throw new Exception(exceptionMsg);
            }

            var methodsEnumerator = methods.GetEnumerator();
            if (methodsEnumerator.MoveNext())
            {
                var method = methodsEnumerator.Current;
                return method.Invoke(method.IsStatic ? null : m_serializerObj, methodParams);
            }
            else
            {
                throw new Exception(exceptionMsg);
            }

        }
    }

    //------------------------------------------------------------------------------
    class MethodSerializerForClass : MethodSerializer
    {
        public MethodSerializerForClass(MethodSerializerAttribute attribute, object serializerObj) 
            : base(attribute, serializerObj)
        {
        }
    }

    //------------------------------------------------------------------------------
    class MethodSerializerForField : MethodSerializer
    {
        public MethodSerializerForField(MethodSerializerAttribute attribute, object serializerObj) 
            : base(attribute, serializerObj)
        {
        }
    }
}

