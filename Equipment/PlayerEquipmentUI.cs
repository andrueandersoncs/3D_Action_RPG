using System;
using Equipment.Abilities;
using Inventory;
using UnityEngine;
using UnityEngine.UIElements;

namespace Equipment
{
    public class PlayerEquipmentUI : MonoBehaviour
    {
        public EquipItemAbility equipItemAbility;
        public UnequipItemAbility unequipItemAbility;
        
        private UIDocument _uiDocument;
        private Equipment _equipment;
        
        private void OnEnable()
        {
            _equipment = GetComponent<Equipment>();
            _uiDocument = FindObjectOfType<UIDocument>();
            _uiDocument.rootVisualElement.Query(className: "equipment-slot").ForEach(slot =>
            {
                void OnSlotClicked(MouseDownEvent evt)
                {
                    var draggableUI = DraggableUI.Dragging;
                    if (draggableUI == null) return;
                    
                    var item = draggableUI.GetComponent<EquipableItem>();
                    if (item == null) return;
                    
                    var equipmentSlotType = Enum.Parse<EquipmentSlot>(slot.name);

                    equipItemAbility.item = item;
                    equipItemAbility.callback = equipItemOutput =>
                    {
                        // TODO: handle swapped items
                        if (equipItemOutput is not EquipmentModule.EquipItemOutput.EquippedItem) return;

                        draggableUI.DropInto(slot);

                        DraggableUI.OnBeginDrag += OnBeginDrag;

                        void OnBeginDrag(DraggableUI ui)
                        {
                            if (ui.gameObject != draggableUI.gameObject) return;
                            DraggableUI.OnBeginDrag -= OnBeginDrag;

                            unequipItemAbility.equipment = _equipment;
                            unequipItemAbility.slot = equipmentSlotType;
                            unequipItemAbility.Play();
                        }
                    };
                    equipItemAbility.Play();
                }
                
                slot.RegisterCallback<MouseDownEvent>(OnSlotClicked);
            });
        }
    }
}