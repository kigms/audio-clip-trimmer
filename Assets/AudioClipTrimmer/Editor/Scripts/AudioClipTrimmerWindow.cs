using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

//[RequireComponent(typeof(WaveformVisualElement))]
public class AudioClipTrimmerWindow : EditorWindow
{
    WaveformVisualElement waveformVisualElement;
    AudioPlayer audioPlayer;

    // References to all UI elements created
    private ObjectField audioClipField;
    
    private MinMaxSlider audioTrimmerSlider;
    float minSliderValue = 0f;
    float maxSliderValue = 50f;
    


    private TextField startTimeField;
    private TextField endTimeField;
    private TextField lengthTimeField_readOnly;
    private Button previewButton;
    private Button exportButton;
    private Slider volumeSlider;

    private AudioClip selectedAudioClip;
    private string outputName;
    private AudioClip outputAudioClip;

    private GameObject audioPlayerObj;

    private void OnEnable()
    {
        Debug.Log("minSliderValue ON ENABLE: " + minSliderValue);
        Debug.Log("maxSliderValue ON ENABLE: " + maxSliderValue);

        audioPlayer = FindObjectOfType<AudioPlayer>();
        if (audioPlayer == null)
        {
            audioPlayerObj = new GameObject("AudioPlayer");
            audioPlayer = audioPlayerObj.AddComponent<AudioPlayer>();

            Debug.Log("Audio Player GameObject destroyed.");
        }
    }

    private void OnDisable()
    {
        if (audioPlayerObj != null)
        {
            DestroyImmediate(audioPlayerObj);

        }
    }

    [MenuItem("Tools/Audio Clip Trimmer")]
    public static void ShowWindow()
    {
        AudioClipTrimmerWindow wnd = GetWindow<AudioClipTrimmerWindow>();
        wnd.titleContent = new GUIContent("Audio Clip Trimmer");
        // TODO: Adjust size according to height and width of standard waveform
        // visualization element size.
        //wnd.maxSize = new Vector2(500, 500);
        wnd.minSize = new Vector2(550, 300);
    }

    private void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
            "Assets/AudioClipTrimmer/Editor/Resources/UIDocuments/AudioClipTrimmerUITemplate.uxml");
        VisualElement tree = visualTree.Instantiate();
        root.Add(tree);


        // Assign elements
        audioClipField = root.Q<ObjectField>();
        audioTrimmerSlider = root.Q<MinMaxSlider>();
        startTimeField = root.Q<TextField>("start-field");
        endTimeField = root.Q<TextField>("end-field");
        lengthTimeField_readOnly = root.Q<TextField>("length-field");
        previewButton = root.Q<Button>("preview-button");
        exportButton = root.Q<Button>("export-button");
        volumeSlider = root.Q<Slider>();

        // Assign callbacks
        audioClipField.RegisterValueChangedCallback<Object>(OnAudioClipLoaded);
        audioTrimmerSlider.RegisterValueChangedCallback(evt =>
                                    OnAudioTrimmerSliderChanged(evt.newValue.x, evt.newValue.y));
        //lengthTimeField_readOnly
        previewButton.clicked += () => OnPreviewButtonClicked(minSliderValue, maxSliderValue);
        exportButton.clicked += () => OnExportButtonClicked();
        volumeSlider.RegisterValueChangedCallback(evt => OnVolumeSliderChanged(evt.newValue));
    }

    private void OnAudioClipLoaded(ChangeEvent<Object> evt)
    {
        if (evt.newValue == null)
        {
            selectedAudioClip = null;
            return;
        }
        outputName = evt.newValue.name + "Trimmed";
        selectedAudioClip = evt.newValue as AudioClip;

        ProcessWaveform(selectedAudioClip);
        audioPlayer.SetAudioClip(selectedAudioClip);
    }

    private void ProcessWaveform(AudioClip selectedAudioClip)
    {
        waveformVisualElement = new WaveformVisualElement(selectedAudioClip, minSliderValue, maxSliderValue);
        VisualElement tree = rootVisualElement.Q<TemplateContainer>();
        tree.Insert(2, waveformVisualElement);
    }

    private void OnAudioTrimmerSliderChanged(float newStartValue, float newEndValue)
    {
        minSliderValue = newStartValue;
        maxSliderValue = newEndValue;
        Debug.Log("new start value: " + newStartValue);
        Debug.Log("new end value: " + newEndValue);
        var waveformVisualElement = rootVisualElement.Q<WaveformVisualElement>(); // Find the WaveformVisualElement instance in the UI hierarchy

        if (waveformVisualElement != null)
        {
            waveformVisualElement.VisualizationUpdate(minSliderValue, maxSliderValue);
        }

        float minTime = minSliderValue * selectedAudioClip.length / selectedAudioClip.samples;
        float maxTime = maxSliderValue * selectedAudioClip.length / selectedAudioClip.samples;

        // Update the text fields with the formatted time strings
        startTimeField.value = FormatTime(minTime);
        endTimeField.value = FormatTime(maxTime);
        lengthTimeField_readOnly.value = FormatTime(maxTime - minTime);
        Debug.Log(startTimeField.value);
        Debug.Log(endTimeField.value);
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 1000) % 1000);
        int microseconds = Mathf.FloorToInt((timeInSeconds * 1000000) % 1000000);

        string formattedTime = $"{minutes:00}:{seconds:00}:{milliseconds:000}:{microseconds:000000}";
        return formattedTime;
    }


    private void OnPreviewButtonClicked(float minSliderValue, float maxSliderValue)
    {
        float duration = selectedAudioClip.length;
        int totalSamples = selectedAudioClip.samples;

        if ((audioPlayer != null) && minSliderValue < maxSliderValue)
        {
            // Convert slider values to sample positions
            int startSample = Mathf.FloorToInt(Mathf.Lerp(0, totalSamples / 10, minSliderValue));
            int endSample = Mathf.FloorToInt(Mathf.Lerp(0, totalSamples / 10, maxSliderValue));
           
            audioPlayer.PlaySelectedRegion(totalSamples, duration, startSample, endSample);
        }
    }

    private void OnExportButtonClicked()
    {
        int startSample = Mathf.FloorToInt(Mathf.Lerp(0, selectedAudioClip.samples / 10, minSliderValue));
        int endSample = Mathf.FloorToInt(Mathf.Lerp(0, selectedAudioClip.samples / 10, maxSliderValue));
        Debug.Log(selectedAudioClip.samples);
        Debug.Log(startSample);
        Debug.Log(endSample);
        Debug.Log(minSliderValue);
        Debug.Log(maxSliderValue);

        AudioClip trimmedClip = CreateTrimmedAudioClip(selectedAudioClip, startSample, endSample);
        SaveAudioClipAsset(trimmedClip);
    }

    private void SaveAudioClipAsset(AudioClip trimmedClip)
    {
        string path = EditorUtility.SaveFilePanel("Save Trimmed Audio", "Assets", "trimmed_audio", "asset");
        if (!string.IsNullOrEmpty(path))
        {
            path = FileUtil.GetProjectRelativePath(path);
            AssetDatabase.CreateAsset(trimmedClip, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    private AudioClip CreateTrimmedAudioClip(AudioClip selectedAudioClip, int startSample, int endSample)
    {
        float[] samples = new float[endSample - startSample];
        Debug.Log(samples.Length);

        selectedAudioClip.GetData(samples, startSample);
        AudioClip trimmedClip = AudioClip.Create("TrimmedAudio", samples.Length, selectedAudioClip.channels, selectedAudioClip.frequency, false);
        trimmedClip.SetData(samples, 0);
        return trimmedClip;
    }

    private void OnVolumeSliderChanged(float newValue)
    {
        audioPlayer.AdjustVolume(newValue);
    }
}

