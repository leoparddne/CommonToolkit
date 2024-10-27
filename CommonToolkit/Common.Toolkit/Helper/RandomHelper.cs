namespace Common.Toolkit.Helper
{
    public class RandomHelper
    {
        public static int GetRandom(int minNumber, int maxNumber)
        {
            var random = new Random();

            return random.Next(minNumber, maxNumber);
        }
    }
}
