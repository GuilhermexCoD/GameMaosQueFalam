using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class QuestionMinigameUI : MonoBehaviour
{
    [SerializeField] private QuestionData _questionData;
    [SerializeField] private TextMeshProUGUI _questionText;
    [SerializeField] private Transform _answerContainerTransform;
    [SerializeField] private AnswerUI _answerPrefab;
    private Transform _topAnswerContainer;
    private Transform _bottomAnswerContainer;

    private List<AnswerUI> _selectedAnswers = new List<AnswerUI>();
    private List<AnswerUI> _answersUIs = new List<AnswerUI>();

    void Awake()
    {
        _topAnswerContainer = _answerContainerTransform.GetChild(0).transform;
        _bottomAnswerContainer = _answerContainerTransform.GetChild(1).transform;

        CreateAnswers();
    }

    private void ClearContainer(Transform container)
    {
        for (int i = container.childCount - 1; i >= 0; i--)
            Destroy(container.GetChild(i));
    }

    private void CreateAnswers()
    {
        if (_questionData == null)
            return;

        _answersUIs.Clear();
        ClearContainer(_topAnswerContainer);
        ClearContainer(_bottomAnswerContainer);

        var answers = _questionData.answers;
        for (int i = 0; i < answers.Length; i++)
        {
            var answer = answers[i];

            var container = (i < 2) ? _topAnswerContainer : _bottomAnswerContainer;

            var answerUI = Instantiate(_answerPrefab, container);
            answerUI.onAnswerStateChanged += OnAnswerStateChanged;
            answerUI.SetAnswer(answer);
            _answersUIs.Add(answerUI);
        }
    }

    private void OnAnswerStateChanged(object sender, AnswerUI.AnswerStateArgs args)
    {
        var answerUI = sender as AnswerUI;

        if (args.isSelected)
            _selectedAnswers.Add(answerUI);
        else
            _selectedAnswers.Remove(answerUI);
    }

    public bool IsAnswerCorrect()
    {
        if (_selectedAnswers.Count == 0)
            return false;

        for (int i = 0; i < _selectedAnswers.Count; i++)
        {
            var selectedAnswer = _selectedAnswers[i];
            if (!selectedAnswer.GetAnswer().isRight)
                return false;
        }

        return true;
    }

    public List<AnswerUI> GetCorrectAnswers()
    {
        return _answersUIs.Where(answer => answer.GetAnswer().isRight).ToList();
    }

    public void Reset()
    {
        _selectedAnswers.Clear();
        foreach (var answerUI in _answersUIs)
            answerUI.Reset();
    }

    public void ConfirmButton()
    {
        foreach (var answerUI in _answersUIs)
        {
            answerUI.SetIsInteractable(false);
            if (answerUI.GetAnswer().isRight || answerUI.IsSelected())
                answerUI.UpdateIsCorrect();
        }
        
        Debug.Log($"Confirm: {IsAnswerCorrect()}");
    }
}
