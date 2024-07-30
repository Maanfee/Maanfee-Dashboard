// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace MudBlazor;

#nullable enable
public class MudTreeViewItemComparerOld<T> : IEqualityComparer<MudTreeViewItemOld<T>>
{
    private readonly IEqualityComparer<T?> _valueComparer;

    public MudTreeViewItemComparerOld(IEqualityComparer<T?> valueComparer)
    {
        _valueComparer = valueComparer;
    }

    public bool Equals(MudTreeViewItemOld<T>? x, MudTreeViewItemOld<T>? y)
    {
        if (x == null && y == null)
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        return _valueComparer.Equals(x.Value, y.Value);
    }

    public int GetHashCode(MudTreeViewItemOld<T> obj)
    {
        return obj.Value is not null ? _valueComparer.GetHashCode(obj.Value) : 0;
    }
}
