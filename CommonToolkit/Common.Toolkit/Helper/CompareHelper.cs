namespace Common.Toolkit.Helper
{
    public enum CompareType
    {
        Equal = 0,
        LessThan = 10,
        GreaterThan = 20,
        NotSupport = 999,
    }
    public static class CompareHelper
    {
        public static CompareType Compare(object first, object second)
        {
            Type typeFirst = first.GetType();
            Type secondFirst = second.GetType();

            //只检测第一个字段类型,尝试将第二个字段转为第一个字段相同的类型

            switch (first)
            {
                case int intValue:
                    return IntCompare(first, second);
                    break;
                case short shortValue:
                    break;
                case ushort ushortValue:
                    break;
                case long longValue:
                    break;
                case float floatValue:
                    break;
                case double doubleValue:
                    return DoubleCompare(first, second);
                    break;
                case decimal decimalValue:
                    break;
                case DateTime dateTimeValue:
                    break;
                case string stringValue:
                    return StringCompare(first, second);
                    break;
                default:
                    break;
            }

            return CompareType.NotSupport;
        }



        public static CompareType IntCompare(object first, object second)
        {
            bool firstParse = int.TryParse(first.ToString(), out int firstValue);
            bool secondParse = int.TryParse(second.ToString(), out int secondValue);

            if (!(firstParse && secondParse))
            {
                //return CompareType.NotSupport;
                throw new ArgumentException("CompareTypeNotEqual");
            }

            if (firstValue == secondValue)
            {
                return CompareType.Equal;
            }
            else
            {
                if (firstValue > secondValue)
                {
                    return CompareType.GreaterThan;
                }
                else
                {
                    return CompareType.LessThan;
                }
            }
        }

        public static CompareType DoubleCompare(object first, object second)
        {
            bool firstParse = double.TryParse(first.ToString(), out double firstValue);
            bool secondParse = double.TryParse(second.ToString(), out double secondValue);

            if (!(firstParse && secondParse))
            {
                //return CompareType.NotSupport;
                throw new ArgumentException("CompareTypeNotEqual");

            }

            if (firstValue == secondValue)
            {
                return CompareType.Equal;
            }
            else
            {
                if (firstValue > secondValue)
                {
                    return CompareType.GreaterThan;
                }
                else
                {
                    return CompareType.LessThan;
                }
            }
        }

        public static CompareType StringCompare(object first, object second)
        {
            string firstValue = first.ToString();
            string secondValue = second.ToString();


            if (firstValue == secondValue)
            {
                return CompareType.Equal;
            }
            else
            {
                if (firstValue.CompareTo(secondValue) < 0)
                {
                    return CompareType.GreaterThan;
                }
                else
                {
                    return CompareType.LessThan;
                }
            }
        }
    }
}
