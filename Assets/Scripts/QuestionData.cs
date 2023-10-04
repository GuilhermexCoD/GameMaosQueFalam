using System;
using UnityEngine;
using UnityEngine.Video;
using System.Linq;

[CreateAssetMenu(fileName = "Question Data", menuName = "ScriptableObjects/Question Data", order = 1)]
public class QuestionData : ScriptableObject
{
#if UNITY_EDITOR

    private void OnValidate()
    {
        if (answers.Length > 4)
        {
            var aux = answers.Take(4);
            answers = aux.ToArray();
        }
    }

#endif

    public string question;
    public VideoClip videoClip;
    public Answer[] answers;
}

[Serializable]
public class Answer
{
    public string content;
    public bool isRight;
}