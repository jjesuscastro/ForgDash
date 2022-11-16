using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;

public class SongManager : MonoBehaviour
{
    public static MidiFile midiFile;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private Lane[] lanes;

    public float songDelayInSeconds;
    public double marginOfError; // in seconds
    public int inputDelayInMilliseconds;


    public string fileLocation;
    public float noteTime;
    public float noteSpawnX;
    public float noteTapX;
    public float noteDespawnX
    {
        get
        {
            return noteTapX - (noteSpawnX - noteTapX);
        }
    }

    #region Singleton
    public static SongManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("[SongManager.cs] - Multiple SongManager(s) found!");
            Destroy(gameObject);
        }
    }
    #endregion 

    private void Start()
    {
        string streamingAssetsPath = Application.streamingAssetsPath;
        if (streamingAssetsPath.StartsWith("http://") || streamingAssetsPath.StartsWith("https://"))
            StartCoroutine(ReadFromWebsite(streamingAssetsPath));
        else
            ReadFromFile(streamingAssetsPath);
    }

    private IEnumerator ReadFromWebsite(string streamingAssetsPath)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(streamingAssetsPath + "/" + fileLocation))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    midiFile = MidiFile.Read(stream);
                    GetDataFromMidi();
                }
            }
        }
    }

    private void ReadFromFile(string streamingAssetsPath)
    {
        midiFile = MidiFile.Read(streamingAssetsPath + "/" + fileLocation);
        GetDataFromMidi();
    }

    private void GetDataFromMidi()
    {
        ICollection<Note> notes = midiFile.GetNotes();
        Note[] array = new Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach (Lane lane in lanes) lane.SetTimeStamps(array);

        Invoke(nameof(StartSong), songDelayInSeconds);
    }

    private void StartSong()
    {
        audioSource.Play();
    }

    public static double GetAudioSourceTime()
    {
        return (double)instance.audioSource.timeSamples / instance.audioSource.clip.frequency;
    }
}