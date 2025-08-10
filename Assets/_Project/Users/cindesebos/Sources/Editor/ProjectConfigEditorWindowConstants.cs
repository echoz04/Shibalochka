#if UNITY_EDITOR

namespace Sources.Editor
{
    public static class ProjectConfigEditorWindowConstants
    {
        public const string WINDOW_TITLE = "Project Configs Editor";
        public const string MENU_ITEM_PATH = "Tools/Project/Open Project Config Editor";
        public const string ASSET_PATH = "t:ProjectConfig";
        public const string NO_PROJECT_CONFIG_FOUND_WARNING = "ProjectConfig asset not found in project.";
        public const string ADD_ITEM_BUTTON_TITLE = "New Item";
        public const string NEW_ITEM_PATH = "Assets/Resources/Configs/Items/";
        public const string SUCCESSFULLY_CREATED_ITEM_MESSAGE = "Was Added New Item";
        public const string CHECK_FOR_DUPLICATE_BUTTON_TITLE = "Check For Duplicates TypeIds";
        public const string ALL_TYPES_UNIQUE_MESSAGE = "All TypeIds are unique";
        public const string DUPLICATE_TYPEIDS_FOUND_WARNING = "Duplicate TypeId was found";
    }
}

#endif