using Sdurlanik.Merge2.Items;
using UnityEngine;

namespace Sdurlanik.Merge2.Editor.Settings
{
    [CreateAssetMenu(fileName = "ItemSOEditorSettings", menuName = "Merge2/Editor/Item Editor Settings")]
    public class ItemSOEditorSettings : ScriptableObject
    {
        public Item DefaultItemPrefab;
        public string IconSpritePrefix = "icon_sprite_";
        public string FileNamePrefix = "so_item_";
    }
}