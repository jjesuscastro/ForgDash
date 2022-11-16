using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public NoteName noteRestriction;
    public KeyCode input;
    public KeyCode pairInput;
    public GameObject notePrefab;
    public GameObject explosionPrefab;
    public bool isPairInput;

    private List<double> timeStamps = new List<double>();
    private List<TargetNote> notes = new List<TargetNote>();

    private int spawnIndex = 0;
    private int inputIndex = 0;

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (Melanchall.DryWetMidi.Interaction.Note note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                MetricTimeSpan metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }

    private void Update()
    {
        if (spawnIndex < timeStamps.Count)
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.instance.noteTime)
            {
                GameObject note = Instantiate(notePrefab, new Vector3(SongManager.instance.noteSpawnX, 0, 0), Quaternion.identity, transform);
                TargetNote tNote = note.GetComponent<TargetNote>();
                tNote.assignedTime = (float)timeStamps[spawnIndex];
                notes.Add(tNote);
                spawnIndex++;
            }
        }

        if (inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = SongManager.instance.marginOfError;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.instance.inputDelayInMilliseconds / 1000.0);

            if ((isPairInput && Input.GetKey(input) && Input.GetKey(pairInput)) || (!isPairInput && Input.GetKeyDown(input)))
            {
                if (Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    Hit();
                    print($"Hit on {inputIndex} note. {Math.Abs(audioTime - timeStamp)} accuracy");
                    if (explosionPrefab != null)
                        Instantiate(explosionPrefab, notes[inputIndex].transform.position, Quaternion.identity, transform);
                    Destroy(notes[inputIndex].gameObject);
                    inputIndex++;
                }
                else
                {
                    // print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                }
            }

            if (timeStamp + marginOfError <= audioTime)
            {
                Miss();
                // print($"Missed {inputIndex} note");
                inputIndex++;
            }
        }

    }
    private void Hit()
    {
        ScoreManager.Hit();
    }
    private void Miss()
    {
        ScoreManager.Miss();
    }
}