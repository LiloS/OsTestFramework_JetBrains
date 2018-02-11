namespace JetBrains
{
    using System;

    public static class Guard
    {
        public static void ArgumentNotNullOrEmpty(string argument, string parameterName)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void ArgumentNotNull(object argument, string parameterName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
