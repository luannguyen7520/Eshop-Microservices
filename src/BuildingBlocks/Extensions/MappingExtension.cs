using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Extensions;

public static class MappingExtension
{
    public static void PatchTo<TSource, TDestination>(this TSource source, TDestination destination)
    {
        var srcProps = typeof(TSource).GetProperties();
        var dstProps = typeof(TDestination).GetProperties();

        foreach (var srcprop in srcProps )
        {
            var dstprop = dstProps.FirstOrDefault(p => p.Name == srcprop.Name && p.PropertyType == srcprop.PropertyType);

            if (dstprop != null && dstprop.CanWrite)
            {
                var value = srcprop.GetValue(source);
                if (value != null)
                {
                    dstprop.SetValue(destination, value);
                }
            }
        }
    }
}
