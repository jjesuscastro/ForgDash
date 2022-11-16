using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetNote : MonoBehaviour
{
    private double timeInstantiated;

    [HideInInspector]
    public float assignedTime;

    private void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
    }

    private void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.instance.noteTime * 2));

        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(Vector3.right * SongManager.instance.noteSpawnX, Vector3.right * SongManager.instance.noteDespawnX, t);
            // GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}