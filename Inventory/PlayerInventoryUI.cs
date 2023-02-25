using System;
using Inventory;
using Keyboard;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class PlayerInventoryUI: MonoBehaviour, IUserInterface
    {
        public int CellSize;
        public string visualElementName;
        
        private UIDocument _uiDocument;
        private VisualElement _visualElement;
        private ItemContainer _container;
        
        private void Start()
        {
            _uiDocument = FindObjectOfType<UIDocument>();

            var inventory = _uiDocument.rootVisualElement.Q("Inventory");
            
            var inventoryToggle = _uiDocument.rootVisualElement.Q<Toggle>("InventoryToggle");
            inventoryToggle
                .ObserveEveryValueChanged(i => i.value)
                .Subscribe(v => UserInterface.Toggle(inventory, v))
                .AddTo(this);
            
            Keybindings.Bind(KeyCode.I, () => inventoryToggle.value = !inventoryToggle.value);
            
            _visualElement = _uiDocument.rootVisualElement.Q<VisualElement>(visualElementName);
            _visualElement.RegisterCallback<MouseDownEvent>(HandleMouseDown);

            // Situation: Dragging something and we click an inventory slot
            void HandleMouseDown(MouseDownEvent evt)
            {
                var cell = evt.localMousePosition / CellSize;
                if (DraggableUI.Dragging == null) return;
                
                // Situation: Dragging something
                var item = DraggableUI.Dragging.GetComponent<ContainableItem>();
                var insertItemOutput = ItemContainerModule.InsertItem(new ItemContainerModule.InsertItemInput
                {
                    ItemContainer = _container,
                    ContainableItem = item,
                    Position = new Vector2Int((int) cell.x, (int) cell.y)
                });
                if (insertItemOutput is not ItemContainerModule.InsertItemOutput.ItemInserted) return;
                
                // Situation: inserted an item into the inventory
                var pos = new Vector2Int((int)cell.x, (int)cell.y);
                DraggableUI.Dragging.DropInto(_visualElement, pos * CellSize);
                DraggableUI.OnBeginDrag += OnBeginDrag;
                
                void OnBeginDrag(DraggableUI ui)
                {
                    if (ui.gameObject != item.gameObject) return;
                    
                    // Situation: we began dragging the item previously dropped
                    DraggableUI.OnBeginDrag -= OnBeginDrag;
                    ItemContainerModule.RemoveItem(new ItemContainerModule.RemoveItemInput
                    {
                        ItemContainer = _container,
                        ContainableItem = item
                    });
                }
            }

            _container = GetComponent<ItemContainer>();
            _container.items.ObserveAdd().Subscribe(OnAddItem);
            _container.items.ObserveRemove().Subscribe(OnRemoveItem);
        }

        private void OnAddItem(DictionaryAddEvent<Vector2Int, ContainableItem> addEvent)
        {
            var itemUI = addEvent.Value.GetComponent<ContainableItemUI>();
            InsertItem(itemUI, addEvent.Key.x, addEvent.Key.y);
            
            DraggableUI.OnBeginDrag += OnBeginDrag;
                
            void OnBeginDrag(DraggableUI ui)
            {
                if (ui.gameObject != itemUI.gameObject) return;
                    
                // Situation: we began dragging the item previously dropped
                DraggableUI.OnBeginDrag -= OnBeginDrag;
                ItemContainerModule.RemoveItem(new ItemContainerModule.RemoveItemInput
                {
                    ItemContainer = _container,
                    ContainableItem = addEvent.Value
                });
            }
        }
        
        private void OnRemoveItem(DictionaryRemoveEvent<Vector2Int, ContainableItem> removeEvent)
        {
            var itemUI = removeEvent.Value.GetComponent<ContainableItemUI>();
            var itemUIVisualElement = itemUI.GetVisualElement();
            if (_visualElement.Contains(itemUIVisualElement))
                _visualElement.Remove(itemUIVisualElement);
        }

        private void InsertItem(IUserInterface itemUI, int x, int y)
        {
            var visualElement = itemUI.GetVisualElement();
            visualElement.style.display = DisplayStyle.Flex;
            visualElement.style.left = x * CellSize;
            visualElement.style.top = y * CellSize;
            visualElement.BringToFront();
            _visualElement.Add(visualElement);
        }

        public VisualElement GetVisualElement()
        {
            return _visualElement;
        }
    }
}