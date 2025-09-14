namespace Learnify.Discount.API.Features;

public static class DiscountCodeGenerator
{
    private const string Allowed = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public static string Generate(int length = 10)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);

        char[] buffer = new char[length];
        for (int i = 0; i < length; i++)
        {
            int idx = RandomNumberGenerator.GetInt32(Allowed.Length); // uniform, bias yok
            buffer[i] = Allowed[idx];
        }

        return new string(buffer);
    }
}
