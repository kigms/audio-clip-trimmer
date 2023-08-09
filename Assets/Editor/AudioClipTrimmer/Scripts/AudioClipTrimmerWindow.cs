using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(WaveformVisualElement))]
public class AudioClipTrimmerWindow : EditorWindow
{
    [MenuItem("Window/Audio Clip Trimmer")]
    public static void ShowWindow()
    {
        GetWindow<AudioClipTrimmerWindow>("Audio Clip Trimmer Window");
    }

    private void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
            "Assets/Editor/AudioClipTrimmer/Resources/UIDocuments/AudioClipTrimmerUITemplate.uxml");

        VisualElement tree = visualTree.Instantiate();
        root.Add(tree);
    }
}
