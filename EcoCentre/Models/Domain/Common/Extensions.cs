namespace EcoCentre.Models.Domain.Common
{
    public static class Extensions
    {
        public static int ToInt(this string str)
        {
            int result;
            int.TryParse(str, out result);
            return result;
        }
    }
}