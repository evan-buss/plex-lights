using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;

namespace PlexLights.Infrastructure
{
    public static class Extensions
    {
        public static IQueryable<T> WriteQueryString<T>([NotNull] this IQueryable<T> source)
        {
            _ = source ?? throw new ArgumentNullException(nameof(source));

            if (source.Provider.Execute<IEnumerable>(source.Expression) is IQueryingEnumerable queryingEnumerable)
            {
                Console.WriteLine(queryingEnumerable.ToQueryString());
            }

            return source;
        }
    }
}