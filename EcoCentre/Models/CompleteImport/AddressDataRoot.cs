using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoCentre.Models.CompleteImport
{
	public class AddressDataRoot
	{
		public partial class dataroot
		{

			private datarootLUDIK_LUD_C_ADR[] lUDIK_LUD_C_ADRField;

			private System.DateTime generatedField;

			/// <remarks/>
			[System.Xml.Serialization.XmlElementAttribute("LUDIK_LUD_C_ADR")]
			public datarootLUDIK_LUD_C_ADR[] LUDIK_LUD_C_ADR
			{
				get
				{
					return this.lUDIK_LUD_C_ADRField;
				}
				set
				{
					this.lUDIK_LUD_C_ADRField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttributeAttribute()]
			public System.DateTime generated
			{
				get
				{
					return this.generatedField;
				}
				set
				{
					this.generatedField = value;
				}
			}
		}

		/// <remarks/>
		[System.SerializableAttribute()]
		[System.ComponentModel.DesignerCategoryAttribute("code")]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
		public partial class datarootLUDIK_LUD_C_ADR
		{

			private object[] itemsField;

			private ItemsChoiceType[] itemsElementNameField;

			/// <remarks/>
			[System.Xml.Serialization.XmlElementAttribute("APP", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("CODE_ARR", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("CODE_POST", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("CODE_UTIL_MODIF", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("DATE_MODIF", typeof(System.DateTime))]
			[System.Xml.Serialization.XmlElementAttribute("ID_RUE", typeof(ushort))]
			[System.Xml.Serialization.XmlElementAttribute("IND_ENV_POST", typeof(byte))]
			[System.Xml.Serialization.XmlElementAttribute("NOM_RUE", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("NOM_RUE_RECH", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("NO_CIV", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("NO_CLE_ADR", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("ORIENT", typeof(byte))]
			[System.Xml.Serialization.XmlElementAttribute("PAYS", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("PROV", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("PROV_ADR", typeof(byte))]
			[System.Xml.Serialization.XmlElementAttribute("SUFF", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("TYPE_ADR", typeof(byte))]
			[System.Xml.Serialization.XmlElementAttribute("VERSION", typeof(byte))]
			[System.Xml.Serialization.XmlElementAttribute("VILLE", typeof(string))]
			[System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
			public object[] Items
			{
				get
				{
					return this.itemsField;
				}
				set
				{
					this.itemsField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
			[System.Xml.Serialization.XmlIgnoreAttribute()]
			public ItemsChoiceType[] ItemsElementName
			{
				get
				{
					return this.itemsElementNameField;
				}
				set
				{
					this.itemsElementNameField = value;
				}
			}
		}

		/// <remarks/>
		[System.SerializableAttribute()]
		[System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema = false)]
		public enum ItemsChoiceType
		{

			/// <remarks/>
			APP,

			/// <remarks/>
			CODE_ARR,

			/// <remarks/>
			CODE_POST,

			/// <remarks/>
			CODE_UTIL_MODIF,

			/// <remarks/>
			DATE_MODIF,

			/// <remarks/>
			ID_RUE,

			/// <remarks/>
			IND_ENV_POST,

			/// <remarks/>
			NOM_RUE,

			/// <remarks/>
			NOM_RUE_RECH,

			/// <remarks/>
			NO_CIV,

			/// <remarks/>
			NO_CLE_ADR,

			/// <remarks/>
			ORIENT,

			/// <remarks/>
			PAYS,

			/// <remarks/>
			PROV,

			/// <remarks/>
			PROV_ADR,

			/// <remarks/>
			SUFF,

			/// <remarks/>
			TYPE_ADR,

			/// <remarks/>
			VERSION,

			/// <remarks/>
			VILLE,
		}
	}
}