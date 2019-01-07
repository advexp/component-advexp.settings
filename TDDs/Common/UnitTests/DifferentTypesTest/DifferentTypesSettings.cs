using System;
using System.Collections.Generic;
using Advexp;

namespace TDD
{
    public enum EEnumValues
    {
        Zero,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
    }

    [Advexp.Preserve(AllMembers = true)]
    // if the Foo is a reference type, the Equals(Object) method tests for reference equality
    // this inapplicable for m_FooList
    // therefore Foo is struct
    [Serializable] // This attribute is only required by FormatMigration unit test
    public struct Foo
    {
        public Int32 Value {get; set;}
    }

    public class DifferentTypesLocalSettings : Advexp.Settings<TDD.DifferentTypesLocalSettings>
    {
        [Setting(Name = "DifferentTypesLocalSettings.m_Boolean")]
        public Boolean m_Boolean = false;
        [Setting(Name = "DifferentTypesLocalSettings.m_Char")]
        public Char m_Char = Char.MaxValue;
        [Setting(Name = "DifferentTypesLocalSettings.m_Byte")]
        public Byte m_Byte = Byte.MaxValue;
        [Setting(Name = "DifferentTypesLocalSettings.m_SByte")]
        public SByte m_SByte = SByte.MaxValue;
        [Setting(Name = "DifferentTypesLocalSettings.m_Int16")]
        public Int16 m_Int16 = Int16.MaxValue;
        [Setting(Name = "DifferentTypesLocalSettings.m_UInt16")]
        public UInt16 m_UInt16 = UInt16.MaxValue;
        [Setting(Name = "DifferentTypesLocalSettings.m_Int32")]
        public Int32 m_Int32 = Int32.MaxValue;
        [Setting(Name = "DifferentTypesLocalSettings.m_UInt32")]
        public UInt32 m_UInt32 = UInt32.MaxValue;
        [Setting(Name = "DifferentTypesLocalSettings.m_Int64")]
        public Int64 m_Int64 = Int64.MaxValue;
        [Setting(Name = "DifferentTypesLocalSettings.m_UInt64")]
        public UInt64 m_UInt64 = UInt64.MaxValue;
        [Setting(Name = "DifferentTypesLocalSettings.m_Single")]
        public Single m_Single = Single.MaxValue;
        [Setting(Name = "DifferentTypesLocalSettings.m_Double")]
        public Double m_Double = Double.MaxValue;
        [Setting(Name = "DifferentTypesLocalSettings.m_Decimal")]
        public Decimal m_Decimal = Decimal.MaxValue;
        [Setting(Name = "DifferentTypesLocalSettings.m_DateTime")]
        public DateTime m_DateTime = DateTime.MaxValue;
        [Setting(Name = "DifferentTypesLocalSettings.m_String")]
        public String m_String = String.Empty;
        [Setting(Name = "DifferentTypesLocalSettings.m_Enum")]
        public EEnumValues m_Enum = EEnumValues.Zero;
        [Setting(Name = "DifferentTypesLocalSettings.m_NullableInt32")]
        public Int32? m_NullableInt32 = null;
        [Setting(Name = "DifferentTypesLocalSettings.m_NullString")]
        public String m_NullString = null;
        [Setting(Name = "DifferentTypesLocalSettings.m_Dict")]
        public Dictionary<String, Object> m_Dict = new Dictionary<String, Object>();
        [Setting(Name = "DifferentTypesLocalSettings.m_List")]
        public List<Int32> m_List = new List<Int32>();
        [Setting(Name = "DifferentTypesLocalSettings.m_FooList")]
        public List<Foo> m_FooList = new List<Foo>();

        //------------------------------------------------------------------------------
        public DifferentTypesLocalSettings()
        {
            RandomizeValues();
        }

        //------------------------------------------------------------------------------
        public void RandomizeValues()
        {
            Random rand = new MyRandom();

            m_Boolean = rand.NextBoolean();
            m_Char = rand.NextChar();
            m_Byte = (Byte)rand.Next(Byte.MinValue, Byte.MaxValue);
            m_SByte = (SByte)rand.Next(SByte.MinValue, SByte.MaxValue);
            m_Int16 = (Int16)rand.Next(Int16.MinValue, Int16.MaxValue);
            m_UInt16 = (UInt16)rand.Next(UInt16.MinValue, UInt16.MaxValue);
            m_Int32 = rand.NextInt32();
            m_UInt32 = (UInt32)rand.Next(0, Int32.MaxValue);
            m_Int64 = rand.NextInt32();
            m_UInt64 = (UInt64)rand.Next(0, Int32.MaxValue);
            m_Single = rand.NextSingle();
            m_Double = rand.NextDouble();
            m_Decimal = rand.NextDecimal();
            m_DateTime = rand.NextDateTime();
            m_String = rand.NextString(8);
            m_Enum = (EEnumValues)rand.Next(0, 10);

            if (rand.NextBoolean())
            {
                m_NullableInt32 = null;
            }
            else
            {
                m_NullableInt32 = rand.NextInt32();
            }

            m_NullString = null;

            m_Dict = new Dictionary<String, Object>();
            m_Dict.Add("one", 1);
            m_Dict.Add("two", 2);
            m_Dict.Add("three", 3);

            m_List = new List<Int32>();
            for(int i=0; i<10; i++ )
            {
                var value = rand.NextInt32();
                m_List.Add(value);
            }

            m_FooList = new List<Foo>();
            for(int i=0; i<10; i++ )
            {
                var foo = new Foo();
                foo.Value = rand.NextInt32();

                m_FooList.Add(foo);
            }
        }
    }

    public class DifferentTypesSecureSettings : Advexp.Settings<TDD.DifferentTypesSecureSettings>
    {
        [Setting(Name = "DifferentTypesSecureSettings.m_Boolean", Secure = true)]
        public Boolean m_Boolean = false;
        [Setting(Name = "DifferentTypesSecureSettings.m_Char", Secure = true)]
        public Char m_Char = Char.MaxValue;
        [Setting(Name = "DifferentTypesSecureSettings.m_Byte", Secure = true)]
        public Byte m_Byte = Byte.MaxValue;
        [Setting(Name = "DifferentTypesSecureSettings.m_SByte", Secure = true)]
        public SByte m_SByte = SByte.MaxValue;
        [Setting(Name = "DifferentTypesSecureSettings.m_Int16", Secure = true)]
        public Int16 m_Int16 = Int16.MaxValue;
        [Setting(Name = "DifferentTypesSecureSettings.m_UInt16", Secure = true)]
        public UInt16 m_UInt16 = UInt16.MaxValue;
        [Setting(Name = "DifferentTypesSecureSettings.m_Int32", Secure = true)]
        public Int32 m_Int32 = Int32.MaxValue;
        [Setting(Name = "DifferentTypesSecureSettings.m_UInt32", Secure = true)]
        public UInt32 m_UInt32 = UInt32.MaxValue;
        [Setting(Name = "DifferentTypesSecureSettings.m_Int64", Secure = true)]
        public Int64 m_Int64 = Int64.MaxValue;
        [Setting(Name = "DifferentTypesSecureSettings.m_UInt64", Secure = true)]
        public UInt64 m_UInt64 = UInt64.MaxValue;
        [Setting(Name = "DifferentTypesSecureSettings.m_Single", Secure = true)]
        public Single m_Single = Single.MaxValue;
        [Setting(Name = "DifferentTypesSecureSettings.m_Double", Secure = true)]
        public Double m_Double = Double.MaxValue;
        [Setting(Name = "DifferentTypesSecureSettings.m_Decimal", Secure = true)]
        public Decimal m_Decimal = Decimal.MaxValue;
        [Setting(Name = "DifferentTypesSecureSettings.m_DateTime", Secure = true)]
        public DateTime m_DateTime = DateTime.MaxValue;
        [Setting(Name = "DifferentTypesSecureSettings.m_String", Secure = true)]
        public String m_String = String.Empty;
        [Setting(Name = "DifferentTypesSecureSettings.m_Enum", Secure = true)]
        public EEnumValues m_Enum = EEnumValues.Zero;
        [Setting(Name = "DifferentTypesSecureSettings.m_NullableInt32", Secure = true)]
        public Int32? m_NullableInt32 = null;
        [Setting(Name = "DifferentTypesSecureSettings.m_NullString", Secure = true)]
        public String m_NullString = null;
        [Setting(Name = "DifferentTypesSecureSettings.m_Dict", Secure = true)]
        public Dictionary<String, Object> m_Dict = new Dictionary<String, Object>();
        [Setting(Name = "DifferentTypesSecureSettings.m_List", Secure = true)]
        public List<Int32> m_List = new List<Int32>();
        [Setting(Name = "DifferentTypesSecureSettings.m_FooList", Secure = true)]
        public List<Foo> m_FooList = new List<Foo>();

        //------------------------------------------------------------------------------
        public DifferentTypesSecureSettings()
        {
            RandomizeValues();
        }

        //------------------------------------------------------------------------------
        public void RandomizeValues()
        {
            Random rand = new MyRandom();

            m_Boolean = rand.NextBoolean();
            m_Char = rand.NextChar();
            m_Byte = (Byte)rand.Next(Byte.MinValue, Byte.MaxValue);
            m_SByte = (SByte)rand.Next(SByte.MinValue, SByte.MaxValue);
            m_Int16 = (Int16)rand.Next(Int16.MinValue, Int16.MaxValue);
            m_UInt16 = (UInt16)rand.Next(UInt16.MinValue, UInt16.MaxValue);
            m_Int32 = rand.NextInt32();
            m_UInt32 = (UInt32)rand.Next(0, Int32.MaxValue);
            m_Int64 = rand.NextInt32();
            m_UInt64 = (UInt64)rand.Next(0, Int32.MaxValue);
            m_Single = rand.NextSingle();
            m_Double = rand.NextDouble();
            m_Decimal = rand.NextDecimal();
            m_DateTime = rand.NextDateTime();
            m_String = rand.NextString(8);
            m_Enum = (EEnumValues)rand.Next(0, 10);

            if (rand.NextBoolean())
            {
                m_NullableInt32 = null;
            }
            else
            {
                m_NullableInt32 = rand.NextInt32();
            }

            m_NullString = null;

            m_List = new List<Int32>();
            for(int i=0; i<10; i++ )
            {
                var value = rand.NextInt32();
                m_List.Add(value);
            }

            m_FooList = new List<Foo>();
            for(int i=0; i<10; i++ )
            {
                var foo = new Foo();
                foo.Value = rand.NextInt32();

                m_FooList.Add(foo);
            }
        }
    }
}
