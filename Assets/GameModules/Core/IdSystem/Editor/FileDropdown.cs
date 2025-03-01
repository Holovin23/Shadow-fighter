using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace TFPlay.Modules.Core.IdSystem
{
    public class FileDropdown : AdvancedDropdown
    {
        /// <summary>
        /// Specifies the name of the top-level node. If not set, the root directory name will be used.
        /// </summary>
        public string rootName;

        private Dictionary<string, List<string>> categoryAndItems;
        private readonly Action<CallbackInfo> onFileSelected;
        private readonly Action<CallbackInfo, object> onFileSelectedAdvanced;
        private readonly object userData;

        public FileDropdown(AdvancedDropdownState state, Dictionary<string, List<string>> categoryAndItems,
            Action<CallbackInfo> onFileSelected)
            : this(state, categoryAndItems)
        {
            this.onFileSelected = onFileSelected;
        }

        public FileDropdown(AdvancedDropdownState state, Dictionary<string, List<string>> categoryAndItems,
            Action<CallbackInfo, object> onFileSelected, object userData)
            : this(state, categoryAndItems)
        {
            this.onFileSelectedAdvanced = onFileSelected;
            this.userData = userData;
        }

        private FileDropdown(AdvancedDropdownState state, Dictionary<string, List<string>> categoryAndItems) :
            base(state)
        {
            this.minimumSize = new Vector2(200, 300);
            this.categoryAndItems = categoryAndItems;
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            if (string.IsNullOrEmpty(rootName)) rootName = "IDs";

            var root = new AdvancedDropdownItem(rootName);
            AddFileSystemEntries(root, categoryAndItems);
            return root;
        }

        private void AddFileSystemEntries(AdvancedDropdownItem root, Dictionary<string, List<string>> directory)
        {
            foreach (var rootV in directory.Keys)
            {
                var folder = new FileDropdownItem(rootV, rootV);

                if (directory[rootV].Count > 0)
                {
                    foreach (var items in directory[rootV])
                        folder.AddChild(new FileDropdownItem(items, items));

                    root.AddChild(folder);
                }
                else
                    root.AddChild(new FileDropdownItem(rootV, rootV));
            }
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            var fileItem = (FileDropdownItem)item;
            var info = new CallbackInfo(fileItem.name, fileItem.fullName);

            onFileSelected?.Invoke(info);
            onFileSelectedAdvanced?.Invoke(info, userData);
        }

        private class FileDropdownItem : AdvancedDropdownItem
        {
            public readonly string fullName;

            public FileDropdownItem(string name, string fullName) : base(name)
            {
                this.fullName = fullName.Replace(@"\", "/");
            }
        }

        /// <summary>
        /// Provides information about the selected file or directory.
        /// </summary>
        public struct CallbackInfo
        {
            /// <summary>
            /// The name of the file (including its extension) or directory.
            /// </summary>
            public readonly string name;

            /// <summary>
            /// The full path of the file or directory.
            /// </summary>
            public readonly string fullName;

            public CallbackInfo(string name, string fullName)
            {
                this.name = name;
                this.fullName = fullName;
            }
        }
    }
}