using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WaveformVisualElement : VisualElement
{
    private AudioClip audioClip;
    //private int sampleSize = 1024; // Adjust as needed
    private float[] amplitudeData;
    private int downscaleFactor = 10;

    // Constructor to initialize your waveform visualization element
    public WaveformVisualElement(AudioClip audioClip, float minSliderValue, float maxSliderValue)
    {
        // Add initialization code here
        style.overflow = Overflow.Visible;
        style.backgroundColor = Color.grey;
        style.flexGrow = 1;
        style.flexShrink = 1;
        style.flexDirection = FlexDirection.Row;
        style.flexWrap = Wrap.Wrap;
        style.justifyContent = Justify.Center;
        style.width = Length.Percent(100);
        style.height = Length.Percent(30);
        style.minHeight = 80;
        this.audioClip = audioClip;
        CalculateAmplitudeData();
        //UpdateVisualization();
        //BasicTest();
        VisualizationUpdate(minSliderValue, maxSliderValue);
    }

    public void VisualizationUpdate(float newMinSliderValue, float newMaxSliderValue)
    {
        {
            Clear();

            if (amplitudeData != null && amplitudeData.Length > 0)
            {
                float widthStep = 100f / amplitudeData.Length;
                float maxHeight = 100f;
                float maxAmplitude = Mathf.Max(amplitudeData);

                for (int i = 0; i < amplitudeData.Length; i += downscaleFactor)
                {
                    Debug.Log("Iteration #");
                    float scaledAmplitude = amplitudeData[i] / maxAmplitude;
                    //float avgAmplitude = CalculateAverageAmplitude(i, downscaleFactor);
                    // avgAmplitude instead of scaled*
                    float height = Mathf.Lerp(0f, maxHeight, scaledAmplitude); // Adjust the range and height as needed
                    float xPos = i * widthStep;

                    // Determine the selected region based on min and max samples
                    //int selectedMinSample = Mathf.FloorToInt(newMinSliderValue * audioClip.samples);
                    //int selectedMaxSample = Mathf.CeilToInt(newMaxSliderValue * audioClip.samples);

                    // Determine if the current segment is within the selected region
                    //bool isInSelectedRegion = i >= selectedMinSample && i <= selectedMaxSample;

                    // Set color based on whether the segment is in the selected region
                    //Color segmentColor = isInSelectedRegion ? Color.red : Color.white;

                    var waveformSegment = new Box
                    {
                        style =
                        {
                            position = Position.Absolute,
                            left = Length.Percent(xPos), // Set x position as a percentage
                            top = Length.Percent(100f - height), // Set y position as a percentage
                            width = Length.Percent(widthStep), // Set width as a percentage
                            height = Length.Percent(height), // Set height as a percentage
                            //backgroundColor = segmentColor
                        }
                    };

                    Add(waveformSegment);
                }
            }
        }
    }

    public void CalculateAmplitudeData()
    {
        if (audioClip != null)
        {
            // Set Load Type to Decompress on Load for compressed files
            int totalSamples = audioClip.samples;
            amplitudeData = new float[totalSamples];
            audioClip.GetData(amplitudeData, 0);
            
        }
        else
        {
            Debug.LogWarning("No AudioClip assigned to WaveformVisualElement");
        }
    }
}
