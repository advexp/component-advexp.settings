using System;

namespace Advexp
{
    public class SettingBaseAttribute : Attribute
    {
        bool m_defaultValueInUse = false;
        object m_defaultValue = null;
        //bool m_mandatoryName = false;

        //------------------------------------------------------------------------------
        public string Name {get; set;}

        //------------------------------------------------------------------------------
        //public bool MandatoryName
        //{
        //    get
        //    {
        //        return m_mandatoryName;
        //    }
        //    set
        //    {
        //        m_mandatoryName = value;
        //    }
        //}

        //------------------------------------------------------------------------------
        public object Default 
        {
            get
            {
                return m_defaultValue;
            }
            set
            {
                m_defaultValue = value;
                m_defaultValueInUse = true;
            }
        }

        //------------------------------------------------------------------------------
        internal bool DefaultValueInUse
        {
            get
            {
                return m_defaultValueInUse;
            }
        }

        //------------------------------------------------------------------------------
        protected SettingBaseAttribute()
        {
            //this.Name = String.Empty;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class SettingAttribute : SettingBaseAttribute
    {
        //------------------------------------------------------------------------------
        public bool Secure { get; set; }
    }
}

