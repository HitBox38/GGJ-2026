using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class ScoreCalculatorHelper
{
    public static int CalculateScore(ItemData[] items)
    {
        return items.Aggregate(0, (current, item) => (int)(current + item.value));
    }

    public static ItemData[] ConvertWorldItemsToData(List<WorldItem> original)
    {
        return original.Select(wi => wi.GetItemData).ToArray();
    }
}