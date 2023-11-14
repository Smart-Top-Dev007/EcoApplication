using System;

namespace EcoCentre.Models.MoneticoPayments
{
	public static class ByteArrayExtentions
	{
		public static string ToHex(this byte[] bytes)
		{
			return BitConverter.ToString(bytes).Replace("-", string.Empty);
		}
	}
}