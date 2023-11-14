using System.ComponentModel.DataAnnotations;

namespace EcoCentre.Models.Domain.Payments
{
	public enum PaymentMethod
	{
		[Display(ResourceType = typeof(Resources.Model.Payment), Name = nameof(Resources.Model.Payment.MethodCash))]
		Cash = 1,

		[Display(ResourceType = typeof(Resources.Model.Payment), Name = nameof(Resources.Model.Payment.MethodCreditCard))]
		CreditCard = 2,

		[Display(ResourceType = typeof(Resources.Model.Payment), Name = nameof(Resources.Model.Payment.MethodVisaCreditCard))]
		VisaCreditCard = 3,

		[Display(ResourceType = typeof(Resources.Model.Payment), Name = nameof(Resources.Model.Payment.MethodMasterCardCreditCard))]
		MasterCardCreditCard = 4,

		[Display(ResourceType = typeof(Resources.Model.Payment), Name = nameof(Resources.Model.Payment.MethodAmexCreditCard))]
		AmexCreditCard = 5,

		[Display(ResourceType = typeof(Resources.Model.Payment), Name = nameof(Resources.Model.Payment.MethodCredit))]
		Credit = 6
	}
}