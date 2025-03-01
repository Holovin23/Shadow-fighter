using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UIElements.Layouts
{
    public class DynamicLayoutUI : MonoBehaviour
    {
        [SerializeField] private DynamicLayoutItemHolder _layoutItemHolderPrefab;
        [SerializeField] private RectTransform _itemsHolder;
        [SerializeField] private int _initialItemsCount;
        [SerializeField] private int _visibleItemsCount;

        [SerializeField] private DynamicLayoutAnimations _animations;

        private List<DynamicLayoutItemHolder> _holders = new();

        public ResourcePanelUI this[int index] => GetHolderByElementIndex(index).PlacedElement;

        public void Initialize()
        {
            for (var i = 0; i < _initialItemsCount; i++) 
                CreateHolder();
        }
        
        public void AddElement(ResourcePanelUI element)
        {
            var holder = GetOrCreateFreeHolder();

            holder.LoadElement(element);
            holder.Enable();
        }

        public void AddElements(List<ResourcePanelUI> elements)
        {
            foreach (var item in elements)
                AddElement(item);

            RefreshOnInsertLayout();
        }

        public void InsertElement(int index, ResourcePanelUI element)
        {
            if (!HasFreeHolder()) CreateHolder();
            
            index = _holders.ClampWithSize(index);

            for (var i = GetFreeHolderIndex(); i --> index;) 
                MoveElementToIndex(i + 1, i);
            

            _holders[index].PlaceElement(element);

            _animations.ShowElement(element);

        }

        public void RemoveElement(ResourcePanelUI element)
        {
            var holder = GetHolderWithElement(element);
            holder.RemovePlacedElement();
        }
        
        public void RemoveElement(int index)
        {
            var holder = GetHolderByElementIndex(index);
            
            if(!holder.IsFree)
                holder.RemovePlacedElement();
        }

        public void RefreshLayout()
        {
            foreach (var holder in _holders)
                if (!holder.IsFree) holder.AdjustElementToHolder();
        }

        public void Clear()
        {
            foreach (var holder in _holders) 
                holder.Disable();
        }

        public bool ContainsElement(ResourcePanelUI element)
        {
            return _holders.Any(holder => holder.PlacedElement == element);
        }


        public void MoveToIndex(ResourcePanelUI element, int to = 0)
        {
            var itemIndex = GetIndexByElement(element);
            if (itemIndex == -1)
                return;
            if (itemIndex == to)
                return;
            MoveAndDropdown(to, itemIndex);
        }

        private void MoveAndDropdown(int nextIndex, int currentIndex)
        {
            var nextIterationElement = _holders[currentIndex].PlacedElement;
            _holders[currentIndex].RemovePlacedElement();

            int delta = Math.Sign(currentIndex - nextIndex);

            while (currentIndex != nextIndex)
            {
                var swapElement = nextIterationElement;
                nextIterationElement = _holders[nextIndex].PlacedElement;
                _holders[nextIndex].PlaceElement(swapElement, true);

                nextIndex += delta;
            }

            _holders[nextIndex].PlaceElement(nextIterationElement, true);
        }


        private void CreateHolder()
        {
            var holder = Instantiate(_layoutItemHolderPrefab, _itemsHolder);
            //holder.Init(_itemsAdjustsDuration, _itemsAdjustEase);
            holder.Init();
            _holders.Add(holder);

            LayoutRebuilder.ForceRebuildLayoutImmediate(_itemsHolder); //Layout group update
        }

        private void MoveElementToIndex(int nextIndex, int currentIndex)
        {
            if (_holders.Count < 2) return;

            _holders[nextIndex].PlaceElement(_holders[currentIndex].PlacedElement,true);
            _holders[currentIndex].RemovePlacedElement();
        }

        private void RefreshOnInsertLayout()
        {
            for (var i = 0; i < _holders.Count; i++)
            {
                if (_holders[i].IsFree) continue;

                var freeHolderIndex = GetFreeHolderIndex();
                if (freeHolderIndex >= 0 && freeHolderIndex < i)
                    MoveElementToIndex(freeHolderIndex, i);
            }

            DisableFreeHolders();
        }

        private void DisableFreeHolders()
        {
            foreach (var holder in _holders)
                if (holder.IsFree)
                    holder.Disable();
        }

        private DynamicLayoutItemHolder GetOrCreateFreeHolder()
        {
            if (HasFreeHolder())
                return _holders.First(h => h.IsFree);

            CreateHolder();
            return _holders.Last();
        }

        private int GetFreeHolderIndex()
        {
            return _holders.FindIndex(holder => holder.IsFree);
        }

        private int GetIndexByElement(ResourcePanelUI element)
        {
            return _holders.FindIndex(holder => holder.PlacedElement == element);
        }
        
        private DynamicLayoutItemHolder GetHolderByElementIndex(int index)
        {
            return _holders.Where(holder => !holder.IsFree).ElementAt(index);
        }

        private DynamicLayoutItemHolder GetHolderWithElement(ResourcePanelUI element)
        {
            return _holders.First(holder => holder.PlacedElement == element);
        }

        private bool HasFreeHolder()
        {
            return _holders.Count > 0 && _holders.Any(item => item.IsFree);
        }

    }
}