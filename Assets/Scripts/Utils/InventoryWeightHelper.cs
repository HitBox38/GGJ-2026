public class InventoryWeightHelper
{
    public static float GetWeightPercentage(InventoryManager invManager)
    {
        var currentWeight = invManager.currentCarryWeight;
        var maxWeight = invManager.maxCarryWeight;
        return currentWeight / maxWeight;
    }
}