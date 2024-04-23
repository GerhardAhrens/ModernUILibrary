namespace ModernIU.Base
{
    using System.Reflection;

    public class CommonUtil
    {
        public static object GetPropertyValue(object obj, string path)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            bool flag = !string.IsNullOrEmpty(path);
            object result;
            if (flag == true)
            {
                PropertyInfo property = obj.GetType().GetProperty(path);
                bool flag2 = property != null;
                if (flag2 == true)
                {
                    result = property.GetValue(obj, null);
                    return result;
                }
            }

            result = obj;
            return result;
        }
    }
}
