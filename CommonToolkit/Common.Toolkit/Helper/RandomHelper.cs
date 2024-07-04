namespace Common.Toolkit.Helper
{
    public class RandomHelper
    {
        #region GetRandom
        public static int GetRandom(int minNumber, int maxNumber)
        {
            Random random = new Random();

            return random.Next(minNumber, maxNumber);
        }
        #endregion
    }
}
