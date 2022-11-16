using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TMP_Text scoreText;
    public ScoreType scoreType;

    [Header("Combo Elements")]
    [SerializeField]
    private GameObject[] comboObjects;

    [Header("Scale Tween")]
    public bool tween;
    public float tweenTime = 0.5f;

    void Start()
    {
        ScoreManager.onHit.AddListener(UpdateScore);
        ScoreManager.onMiss.AddListener(Missed);

        scoreText.text = "0";
    }

    private void UpdateScore()
    {
        int score = scoreType == ScoreType.SCORE ? ScoreManager.score : ScoreManager.combo;
        scoreText.text = score.ToString();

        if (tween)
            LeanTween.scale(scoreText.gameObject, Vector3.one * 1.25f, tweenTime).setEasePunch();

        if (scoreType == ScoreType.COMBO && score >= 5)
        {
            foreach (GameObject obj in comboObjects)
                obj.SetActive(true);
        }
    }

    private void Missed()
    {
        if (scoreType == ScoreType.COMBO)
        {
            foreach (GameObject obj in comboObjects)
                obj.SetActive(false);
        }
    }
}

public enum ScoreType
{
    SCORE,
    COMBO
}
