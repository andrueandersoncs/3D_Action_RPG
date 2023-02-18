namespace Equipment
{
    public static class EquipmentModule
    {
        public struct EquipItemInput
        {
            public Equipment Equipment;
            public EquipableItem Item;
            public EquipmentSlot Slot;
        }
    
        public abstract class EquipItemOutput {
            public class EquippedItem : EquipItemOutput {}
            public class IncorrectSlot : EquipItemOutput {}
            public class SwappedEquippedItems : EquipItemOutput
            {
                public UnequipItemOutput UnequipItemOutput;
                public EquipItemOutput EquipItemOutput;
            }
        }
        
        public static EquipItemOutput EquipItem(EquipItemInput input)
        {
            var item = input.Item;
            var slot = input.Slot;
            var equipment = input.Equipment.items;

            if (item.slot != slot) return new EquipItemOutput.IncorrectSlot(); 

            if (equipment.ContainsKey(slot))
            {
                var unequipOutput = UnequipItem(new UnequipItemInput
                {
                    Equipment = input.Equipment,
                    Slot = slot
                });

                return new EquipItemOutput.SwappedEquippedItems
                {
                    EquipItemOutput = EquipItem(input),
                    UnequipItemOutput = unequipOutput
                };
            }

            equipment.Add(slot, item);
            return new EquipItemOutput.EquippedItem();
        }
        
        public struct UnequipItemInput
        {
            public EquipmentSlot Slot;
            public Equipment Equipment;
        }
    
        public abstract class UnequipItemOutput
        {
            public class DidNotContainItem : UnequipItemOutput { }
        
            public class UnequippedItem : UnequipItemOutput
            {
                public EquipableItem Item;
            }
        }
    
        public static UnequipItemOutput UnequipItem(UnequipItemInput input)
        {
            var equipment = input.Equipment.items;
            var slot = input.Slot;
        
            if (!equipment.ContainsKey(slot)) return new UnequipItemOutput.DidNotContainItem();

            var item = equipment[slot];
            
            equipment.Remove(slot);
            
            return new UnequipItemOutput.UnequippedItem { Item = item };
        }
    }
}