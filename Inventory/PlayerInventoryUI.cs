using System.Linq;
using DefaultNamespace;
using Keyboard;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace Inventory
{
    public class PlayerInventoryUI: MonoBehaviour, IUserInterface
    {
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
            
            var rows = _visualElement.Children();
            var y = 0;
            foreach (var row in rows)
            {
                var columns = row.Children();
                var x = 0;
                foreach (var column in columns)
                {
                    column.RegisterCallback(HandleClickSlot(x, y, column));
                    x++;
                }
                y++;
            }

            // Only handles dropping items into slots
            // - This is because picking up an item from a slot is already handled in the draggableUI component
            EventCallback<MouseDownEvent> HandleClickSlot(int x, int y, VisualElement target) => (evt) =>
            {
                if (DraggableUI.Dragging == null) return;
                
                var item = DraggableUI.Dragging.GetComponent<ContainableItem>();
                var insertItemOutput = ItemContainerModule.InsertItem(new ItemContainerModule.InsertItemInput
                {
                    ItemContainer = _container,
                    ContainableItem = item,
                    Position = new Vector2Int(x, y)
                });
                
                // Early return when item is not actually inserted
                if (insertItemOutput is not ItemContainerModule.InsertItemOutput.ItemInserted) return;
                
                DraggableUI.Dragging.DropInto(target, Vector2.zero);
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
            };

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
            var position = removeEvent.Key;
            var slot = _visualElement.Children().ElementAt(position.y).Children().ElementAt(position.x);
            if (slot.Contains(itemUIVisualElement)) slot.Remove(itemUIVisualElement);
        }

        private void InsertItem(IUserInterface itemUI, int x, int y)
        {
            var visualElement = itemUI.GetVisualElement();
            visualElement.style.display = DisplayStyle.Flex;
            visualElement.style.left = 0;
            visualElement.style.top = 0;
            visualElement.BringToFront();
            _visualElement.Children().ElementAt(y).Children().ElementAt(x).Add(visualElement);
        }

        public VisualElement GetVisualElement()
        {
            return _visualElement;
        }
    }
}