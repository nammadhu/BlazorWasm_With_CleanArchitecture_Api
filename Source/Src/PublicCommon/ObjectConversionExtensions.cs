using System.Reflection;

namespace PublicCommon
    {
    public static class ObjectConversionExtensions
        {

        public static TTarget To<TSource, TTarget>(this TSource source)
            where TSource : class
            where TTarget : new()
            {
            if (source == null)
                {
                return default; // Handle null case explicitly
                }

            var target = new TTarget();
            var sourceProperties = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var targetProperties = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var sourceProperty in sourceProperties)
                {
                if (!sourceProperty.CanRead)
                    {
                    continue; // Skip properties not readable in source
                    }

                var targetProperty = targetProperties.FirstOrDefault(p => p.Name == sourceProperty.Name); // Look for exact match only
                if (targetProperty != null && targetProperty.CanWrite)
                    {
                    targetProperty.SetValue(target, sourceProperty.GetValue(source));
                    }
                }

            return target;
            }

        }
    }
