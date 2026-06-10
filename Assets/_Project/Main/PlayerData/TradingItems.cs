using System.Collections.Generic;

namespace _Project.Main.PlayerData
{
    public class TradingItems
    {
        private List<TradingItem> _tradingItems = new List<TradingItem>();
        
        public List<TradingItem> TradingItemsList => _tradingItems;
        
        public void Load(TradingItemsSaveData saveData)
        {
            foreach (var tradingItemSaveData in saveData.tradingItems)
            {
                var tradingItem = new TradingItem
                {
                    ItemId = tradingItemSaveData.itemId,
                    Count = tradingItemSaveData.count,
                };
                
                _tradingItems.Add(tradingItem);
            }
        }
    }
}