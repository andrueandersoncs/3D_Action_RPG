using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UIElements;

namespace Inventory
{
    public class ContainableItemUI : MonoBehaviour, IUserInterface
    {
        private const float CellSize = 50;

        public Texture2D icon;
        
        private VisualElement _visualElement;
        private ContainableItem _item;
        
        private void OnEnable()
        {
            _item = GetComponent<ContainableItem>();
            _visualElement = new VisualElement
            {
                style =
                {
                    display = DisplayStyle.None,
                    width = _item.dimensions.x * CellSize,
                    height = _item.dimensions.y * CellSize,
                    backgroundImage = icon,
                    position = Position.Absolute,
                    borderBottomColor = Color.black,
                    borderBottomWidth = 1,
                    borderLeftColor = Color.black,
                    borderLeftWidth = 1,
                    borderRightColor = Color.black,
                    borderRightWidth = 1,
                    borderTopColor = Color.black,
                    borderTopWidth = 1,
                }
            };
        }

        public VisualElement GetVisualElement()
        {
            return _visualElement;
        }
    }
}