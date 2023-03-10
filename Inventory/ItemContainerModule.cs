using System.Linq;
using Items;
using UnityEngine;

namespace Inventory
{
    public static class ItemContainerModule
    {
        public struct AddItemInput
        {
            public ContainableItem ContainableItem;
            public ItemContainer ItemContainer;
        }
        
        public abstract class AddItemOutput
        {
            public class ItemAdded : AddItemOutput
            {
                public Vector2Int Position;
                public ContainableItem Item;
            }
            public class DimensionsOutOfBounds : AddItemOutput { }
            public class NoSpaceAvailable : AddItemOutput { }
        }

        public static AddItemOutput AddItem(AddItemInput input)
        {
            var item = input.ContainableItem;
            var container = input.ItemContainer;
            
            if (item.dimensions.x > container.dimensions.x || item.dimensions.y > container.dimensions.y)
            {
                return new AddItemOutput.DimensionsOutOfBounds();
            }

            for (var i = 0; i < container.dimensions.x; i++)
            {
                for (var j = 0; j < container.dimensions.y; j++)
                {
                    var position = new Vector2Int(i, j);
                    var rect = new RectInt(position, item.dimensions);
                    
                    var overlapsOther = container.items.Any(otherItem =>
                    {
                        var otherRect = new RectInt(otherItem.Key, otherItem.Value.dimensions);
                        return rect.Overlaps(otherRect);
                    });
                    
                    if (overlapsOther) continue;
                    
                    container.items.Add(position, item);

                    return new AddItemOutput.ItemAdded { Item = item, Position = new Vector2Int(i, j) };
                }
            }

            return new AddItemOutput.NoSpaceAvailable();
        }
        
        public struct InsertItemInput
        {
            public ContainableItem ContainableItem;
            public ItemContainer ItemContainer;
            public Vector2Int Position;
        }
        
        public abstract class InsertItemOutput
        {
            public class ItemAlreadyThere : InsertItemOutput { }
            public class DimensionsOutOfBounds : InsertItemOutput { }
            public class ItemInserted : InsertItemOutput
            {
                public Vector2Int Position;
                public ContainableItem Item;
            }
        }
        
        public static InsertItemOutput InsertItem(InsertItemInput input)
        {
            var position = input.Position;
            var container = input.ItemContainer;
            var items = container.items;
            var item = input.ContainableItem;

            if (items.ContainsKey(position))
            {
                return new InsertItemOutput.ItemAlreadyThere();
            }

            if (item.dimensions.x > container.dimensions.x || item.dimensions.y > container.dimensions.y)
            {
                return new InsertItemOutput.DimensionsOutOfBounds();
            }


            if (position.x + item.dimensions.x > container.dimensions.x || position.y + item.dimensions.y > container.dimensions.y)
            {
                return new InsertItemOutput.DimensionsOutOfBounds();
            }

            var rect = new RectInt(position, item.dimensions);
                    
            var overlapsOther = items.Any(otherItem =>
            {
                var otherRect = new RectInt(otherItem.Key, otherItem.Value.dimensions);
                return rect.Overlaps(otherRect);
            });

            if (overlapsOther)
            {
                return new InsertItemOutput.ItemAlreadyThere();
            }
                    
            items.Add(position, item);

            return new InsertItemOutput.ItemInserted { Item = item, Position = position };
        }
        
        public struct RemoveItemInput
        {
            public ContainableItem ContainableItem;
            public ItemContainer ItemContainer;
        }
        
        public abstract class RemoveItemOutput
        {
            public class ItemNotInContainer : RemoveItemOutput { }
            public class RemovedItem: RemoveItemOutput
            {
                public Vector2Int Position;
                public ContainableItem Item;
            }
        }
        
        public static RemoveItemOutput RemoveItem(RemoveItemInput input)
        {
            var items = input.ItemContainer.items;
            var item = input.ContainableItem;

            if (!items.Values.Contains(item))
            {
                return new RemoveItemOutput.ItemNotInContainer();
            }

            var positions = items.Where(pair => pair.Value == item).ToList();

            var originPosition = positions.Aggregate(positions[0],
                (topLeft, kvp) =>
                {
                    if (kvp.Key.x < topLeft.Key.x || kvp.Key.y < topLeft.Key.y) return kvp;
                    return topLeft;
                }).Key;
            
            foreach (var position in positions)
            {
                items.Remove(position.Key);
            }

            return new RemoveItemOutput.RemovedItem { Item = item, Position = originPosition };
        }
        
        public struct RemoveItemFromSlotInput
        {
            public ItemContainer ItemContainer;
            public Vector2Int Position;
        }
        
        public abstract class RemoveItemFromSlotOutput
        {
            public class ItemRemoved : RemoveItemFromSlotOutput
            {
                public ContainableItem Item;
                public Vector2Int Position;
            }
            public class SlotOutsideBounds : RemoveItemFromSlotOutput { }
            public class NoItemInSlot : RemoveItemFromSlotOutput { }
        }
        
        public static RemoveItemFromSlotOutput RemoveItemFromSlot(RemoveItemFromSlotInput input)
        {
            var dimensions = input.ItemContainer.dimensions;
            var items = input.ItemContainer.items;
            var position = input.Position;
            
            if (position.x > dimensions.x || position.x < 0 || position.y > dimensions.y || position.y < 0)
            {
                return new RemoveItemFromSlotOutput.SlotOutsideBounds();
            }

            if (!items.ContainsKey(position))
            {
                return new RemoveItemFromSlotOutput.NoItemInSlot();
            }
            
            var item = items[position];
            
            var positions = items.Where(pair => pair.Value == item).ToList();
            foreach (var keyValuePair in positions)
            {
                items.Remove(keyValuePair.Key);
            }

            return new RemoveItemFromSlotOutput.ItemRemoved { Item = item, Position = position };
        }
        
        public static void ReinsertItem(RemoveItemOutput removeItemOutput, ItemContainer container)
        {
            if (removeItemOutput is not RemoveItemOutput.RemovedItem removedItem) return;
            InsertItem(new InsertItemInput
            {
                ContainableItem = removedItem.Item,
                ItemContainer = container,
                Position = removedItem.Position
            });
        }
    }
}