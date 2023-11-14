using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoCentre.Models.CompleteImport
{
	public class ClientDataRoot
	{
		[System.SerializableAttribute()]
		[System.ComponentModel.DesignerCategoryAttribute("code")]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
		[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
		public partial class dataroot
		{

			private datarootLUDIK_LUD_C_PERS[] lUDIK_LUD_C_PERSField;

			private System.DateTime generatedField;

			/// <remarks/>
			[System.Xml.Serialization.XmlElementAttribute("LUDIK_LUD_C_PERS")]
			public datarootLUDIK_LUD_C_PERS[] LUDIK_LUD_C_PERS
			{
				get
				{
					return this.lUDIK_LUD_C_PERSField;
				}
				set
				{
					this.lUDIK_LUD_C_PERSField = value;
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
		public partial class datarootLUDIK_LUD_C_PERS
		{

			private object[] itemsField;

			private ItemsChoiceType[] itemsElementNameField;

			/// <remarks/>
			[System.Xml.Serialization.XmlElementAttribute("ADR_EMAIL", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("CODE_UTIL_CREAT", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("CODE_UTIL_MODIF", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("CODE_UTIL_VALIDATION", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("CRIT_1", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("DATE_CREAT", typeof(System.DateTime))]
			[System.Xml.Serialization.XmlElementAttribute("DATE_EXP_NAM", typeof(System.DateTime))]
			[System.Xml.Serialization.XmlElementAttribute("DATE_FIN_MEMBRE", typeof(System.DateTime))]
			[System.Xml.Serialization.XmlElementAttribute("DATE_MODIF", typeof(System.DateTime))]
			[System.Xml.Serialization.XmlElementAttribute("DATE_NAIS", typeof(System.DateTime))]
			[System.Xml.Serialization.XmlElementAttribute("DATE_VALIDATION", typeof(System.DateTime))]
			[System.Xml.Serialization.XmlElementAttribute("DONNEE_50CAR_1", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("DONNEE_50CAR_2", typeof(byte))]
			[System.Xml.Serialization.XmlElementAttribute("DONNEE_80CAR_3", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("ID", typeof(ushort))]
			[System.Xml.Serialization.XmlElementAttribute("IND_AIDE_SOC", typeof(byte))]
			[System.Xml.Serialization.XmlElementAttribute("IND_LIEN_MIL", typeof(byte))]
			[System.Xml.Serialization.XmlElementAttribute("IND_NO_SOLICITATION", typeof(byte))]
			[System.Xml.Serialization.XmlElementAttribute("IND_TRSF_BIBLIO", typeof(byte))]
			[System.Xml.Serialization.XmlElementAttribute("LANG_CORR", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("NAM", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("NAS", typeof(uint))]
			[System.Xml.Serialization.XmlElementAttribute("NIVEAU_VALID_DOSS", typeof(byte))]
			[System.Xml.Serialization.XmlElementAttribute("NOM", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("NOM_RECH", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("NO_AIDE_SOC", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("NO_PERS", typeof(uint))]
			[System.Xml.Serialization.XmlElementAttribute("PNOM", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("PNOM_RECH", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("REM", typeof(string))]
			[System.Xml.Serialization.XmlElementAttribute("RESID", typeof(byte))]
			[System.Xml.Serialization.XmlElementAttribute("SEXE", typeof(byte))]
			[System.Xml.Serialization.XmlElementAttribute("TEL_MAIS", typeof(ulong))]
			[System.Xml.Serialization.XmlElementAttribute("VERSION", typeof(byte))]
			[System.Xml.Serialization.XmlElementAttribute("WEB_IND_MAILING", typeof(byte))]
			[System.Xml.Serialization.XmlElementAttribute("WEB_IND_OK_AMI", typeof(byte))]
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
			ADR_EMAIL,

			/// <remarks/>
			CODE_UTIL_CREAT,

			/// <remarks/>
			CODE_UTIL_MODIF,

			/// <remarks/>
			CODE_UTIL_VALIDATION,

			/// <remarks/>
			CRIT_1,

			/// <remarks/>
			DATE_CREAT,

			/// <remarks/>
			DATE_EXP_NAM,

			/// <remarks/>
			DATE_FIN_MEMBRE,

			/// <remarks/>
			DATE_MODIF,

			/// <remarks/>
			DATE_NAIS,

			/// <remarks/>
			DATE_VALIDATION,

			/// <remarks/>
			DONNEE_50CAR_1,

			/// <remarks/>
			DONNEE_50CAR_2,

			/// <remarks/>
			DONNEE_80CAR_3,

			/// <remarks/>
			ID,

			/// <remarks/>
			IND_AIDE_SOC,

			/// <remarks/>
			IND_LIEN_MIL,

			/// <remarks/>
			IND_NO_SOLICITATION,

			/// <remarks/>
			IND_TRSF_BIBLIO,

			/// <remarks/>
			LANG_CORR,

			/// <remarks/>
			NAM,

			/// <remarks/>
			NAS,

			/// <remarks/>
			NIVEAU_VALID_DOSS,

			/// <remarks/>
			NOM,

			/// <remarks/>
			NOM_RECH,

			/// <remarks/>
			NO_AIDE_SOC,

			/// <remarks/>
			NO_PERS,

			/// <remarks/>
			PNOM,

			/// <remarks/>
			PNOM_RECH,

			/// <remarks/>
			REM,

			/// <remarks/>
			RESID,

			/// <remarks/>
			SEXE,

			/// <remarks/>
			TEL_MAIS,

			/// <remarks/>
			VERSION,

			/// <remarks/>
			WEB_IND_MAILING,

			/// <remarks/>
			WEB_IND_OK_AMI,
		}
	}
}