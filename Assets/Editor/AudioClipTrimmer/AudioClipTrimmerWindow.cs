using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

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
        Label title = new Label("Audio Clip Trimmer");
        root.Add(title);

        Button loadButton = new Button(() =>
        {
            // Think this is like an OnClick() setup
            // TODO: Handle loading audio clip
        });
        loadButton.text = "Load Audio";
        root.Add(loadButton);
    }
}
