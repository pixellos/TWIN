using System.Linq;
using System.Reflection;

namespace WebLedMatrix.Hubs
{
    public static class WrapperHeleprs
    {
        public static object[] DeserializeObject(this object[] thisObjects, int countOfParameters)
        {
            return ((object[])thisObjects).Take(countOfParameters).ToArray();
        }

        public static bool IsEquals(this ParameterInfo[] firstParameterInfos, ParameterInfo[] secParameterInfos)
        {
            bool isEveryMatching = true;
            foreach (var first in firstParameterInfos)
            {
                bool isAnyMatching = false;
                foreach (var second in secParameterInfos)
                {
                    if (isAnyMatching == false)
                    {
                        isAnyMatching = first.Name.Equals(second.Name) && first.ParameterType.Equals(second.ParameterType);
                    }
                }
                if (isAnyMatching == false)
                {
                    isEveryMatching = false;
                }
            }
            return isEveryMatching;
        }

        public static bool IsEquals(this ParameterInfo firstParameterInfos, ParameterInfo secParameterInfos)
        {
            return firstParameterInfos.ParameterType.Equals(secParameterInfos.ParameterType);
        }
    }
}