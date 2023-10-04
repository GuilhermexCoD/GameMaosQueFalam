using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnswerUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{

    public event EventHandler<AnswerStateArgs> onAnswerStateChanged;
    public class AnswerStateArgs : EventArgs
    {
        public bool isSelected;
        public Answer answer;
    }

    [SerializeField] private bool _isInteractable = true;
    [SerializeField] private bool _isSelected = false;
    [SerializeField] private ColorBlock _colors;
    [SerializeField] private Graphic _targetGraphic;
    [SerializeField] private TextMeshProUGUI _answerText;

    private Answer _answer;

    private void SetSelected(bool isSelected)
    {
        _isSelected = isSelected;
        onAnswerStateChanged?.Invoke(this, new AnswerStateArgs
        {
            isSelected = _isSelected,
            answer = _answer
        });
    }

    public void SetAnswer(Answer answer)
    {
        _answer = answer;
        _answerText.text = answer.content;
    }

    public Answer GetAnswer()
    {
        return _answer;
    }

    public bool IsSelected()
    {
        return _isSelected;
    }

    private void SetColor(Color color)
    {
        _targetGraphic.color = color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_isInteractable)
            return;

        SetSelected(!_isSelected);

        if (_isSelected)
            SetColor(_colors.select);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_isInteractable)
            return;

        SetColor(_colors.highlight);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isInteractable)
            return;

        if (_isSelected)
            SetColor(_colors.select);
        else
            SetColor(_colors.normal);
    }

    public void SetIsInteractable(bool isInteractable)
    {
        _isInteractable = isInteractable;
    }

    public void UpdateIsCorrect()
    {
        SetColor(_answer.isRight ? _colors.correct : _colors.incorrect);
    }

    public void Reset()
    {
        _isSelected = false;
        _isInteractable = true;
        SetColor(_colors.normal);
    }


    [Serializable]
    public class ColorBlock
    {
        public Color normal = Color.white;
        public Color highlight = Color.white;
        public Color select = Color.white;
        public Color correct = Color.white;
        public Color incorrect = Color.white;
    }
}
