using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

//[RequireComponent(typeof(WaveformVisualElement))]
public class AudioClipTrimmerWindow : EditorWindow
{
    // References to all UI elements created
    private ObjectField audioClipField;
    private VisualElement waveformVisualization;
    private MinMaxSlider audioTrimmerSlider;
    private TextField startTimeField;
    private TextField endTimeField;
    private TextField lengthTimeField_readOnly;
    private Button previewButton;
    private Button exportButton;
    private Slider volumeSlider;

    private AudioClip audioClipTrim;

    [MenuItem("Tools/Audio Clip Trimmer")]
    public static void ShowWindow()
    {
        AudioClipTrimmerWindow wnd = GetWindow<AudioClipTrimmerWindow>();
        wnd.titleContent = new GUIContent("Audio Clip Trimmer");
        // TODO: Adjust size according to height and width of standard waveform
        // visualization element size.
      //wnd.maxSize...
        wnd.minSize = new Vector2(345, 182);
    }

    private void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
            "Assets/Editor/AudioClipTrimmer/Resources/UIDocuments/AudioClipTrimmerUITemplate.uxml");

        VisualElement tree = visualTree.Instantiate();
        root.Add(tree);

        // Assign elements
        audioClipField = root.Q<ObjectField>();
        waveformVisualization = root.Q<VisualElement>("placeholder-element");
        audioTrimmerSlider = root.Q<MinMaxSlider>();
        startTimeField = root.Q<TextField>("start-field");
        endTimeField = root.Q<TextField>("end-field");
        lengthTimeField_readOnly = root.Q<TextField>("length-field");
        previewButton = root.Q<Button>("preview-button");
        exportButton = root.Q<Button>("export-button");
        volumeSlider = root.Q<Slider>();

        // Assign callbacks
        audioClipField.RegisterValueChangedCallback<Object>(AudioClipLoaded);
        //(?)waveformVisualization.RegisterCallback<VisualElement>(WaveformLoaded);
        audioTrimmerSlider.RegisterValueChangedCallback(evt => 
                                    OnAudioTrimmerSliderChanged(evt.newValue.x, evt.newValue.y));
        startTimeField.RegisterValueChangedCallback(evt => OnStartTimeAdjusted(evt.newValue));
        endTimeField.RegisterValueChangedCallback(evt => OnEndTimeAdjusted(evt.newValue));
        //lengthTimeField_readOnly
        previewButton.clicked += () => OnPreviewButtonClicked();
        exportButton.clicked += () => ExportAudioTrim(audioClipTrim);
        volumeSlider.RegisterValueChangedCallback(evt => OnVolumeSliderChanged(evt.newValue));
    }

    private void OnVolumeSliderChanged(float newValue)
    {
        throw new System.NotImplementedException();
    }

    private void AudioClipLoaded(ChangeEvent<Object> evt)
    {
        throw new System.NotImplementedException();
    }

    private void OnAudioTrimmerSliderChanged(float newStartValue, float newEndValue)
    {
        throw new System.NotImplementedException();
    }
    private void OnStartTimeAdjusted(string newValue)
    {
        throw new System.NotImplementedException();
    }
    private void OnEndTimeAdjusted(string newValue)
    {
        throw new System.NotImplementedException();
    }
    private void OnPreviewButtonClicked()
    {
        throw new System.NotImplementedException();
    }
    private void ExportAudioTrim(AudioClip audioClipTrim)
    {
        throw new System.NotImplementedException();
    }
}

