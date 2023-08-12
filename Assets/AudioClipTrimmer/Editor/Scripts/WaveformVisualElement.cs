using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WaveformVisualElement : VisualElement
{
    private AudioClip audioClip;
    private float[] amplitudeData;
    private int downscaleFactor = 10;

    public WaveformVisualElement(AudioClip audioClip)
    {
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
        VisualizationUpdate();
    }

    public void VisualizationUpdate()
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
                    float height = Mathf.Lerp(0f, maxHeight, scaledAmplitude);
                    float xPos = i * widthStep;

                    var waveformSegment = new Box
                    {
                        style =
                        {
                            position = Position.Absolute,
                            left = Length.Percent(xPos),
                            top = Length.Percent(100f - height),
                            width = Length.Percent(widthStep),
                            height = Length.Percent(height),
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
