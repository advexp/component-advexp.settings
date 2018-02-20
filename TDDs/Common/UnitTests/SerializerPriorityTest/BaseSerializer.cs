using System;
using Advexp;

namespace TDD
{
    public class BaseSerializer : ISettingsSerializer
    {
        //------------------------------------------------------------------------------
        public BaseSerializer()
        {
        }

        #region ISettingsSerializer implementation

        //------------------------------------------------------------------------------
        public bool Load(string settingName, bool secure, out object value)
        {
            value = null;
            return false;
        }

        //------------------------------------------------------------------------------
        public void Save(string settingName, bool secure, object value)
        {
        }

        //------------------------------------------------------------------------------
        public void Delete(string settingName, bool secure)
        {
        }

        //------------------------------------------------------------------------------
        public bool Contains(string settingName, bool secure)
        {
            throw new NotImplementedException();
        }

        //------------------------------------------------------------------------------
        public void Synchronize()
        {
        }

        #endregion
    }
}

