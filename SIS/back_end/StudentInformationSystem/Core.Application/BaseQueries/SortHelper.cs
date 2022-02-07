using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;


namespace Core.Application.QueryUtil
{
    public static class SortHelper
    {
        /// <summary>
        /// Applies specified sorting
        /// 
        /// Example sortString: "grade desc,lastname,firstname"
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="sortString">Example: "lastname,firstname desc"</param>
        public static IQueryable<TEntity> ApplySort<TEntity>(this IQueryable<TEntity> entities, string sortString)
        {
            if (entities == null || !entities.Any()) return entities;
            if (string.IsNullOrWhiteSpace(sortString)) return entities;

            var sortParams = sortString.Trim().Split(',');
            var properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var sortQuery = new StringBuilder();

            foreach (var param in sortParams)
            {
                if (string.IsNullOrWhiteSpace(param)) continue;

                var property = GetProperty(properties, param);
                if (property == null) continue;

                var order = GetOrder(param);

                sortQuery.Append($"{property.Name} {order}, ");
            }

            var finalQuery = sortQuery.ToString().TrimEnd(',', ' ');
            if (string.IsNullOrWhiteSpace(finalQuery)) return entities;

            return entities.OrderBy(finalQuery);
        }

        private static string GetOrder(string param)
        {
            return param.EndsWith(" desc") ? "descending" : "ascending";
        }

        private static PropertyInfo GetProperty(PropertyInfo[] properties, string param)
        {
            var propertyName = param.Split(" ")[0];
            return properties.FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}