using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonCard : MonoBehaviour
{
    public struct Data
    {
        public string word;
        public Sprite image;
        public bool isWord;
        public AudioClip sound;
    }

    public TMP_Text wordText;
    public Image imageDisplay;
    public Button button;


    public AudioSource audioSource;
    private Data data;
    private MatchingGameManager manager;
    public bool IsRevealed { get; private set; } = false;

    public void SetData(Data newData, MatchingGameManager mgr)
    {
        data = newData;
        manager = mgr;
        Hide();
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        manager.OnCardSelected(this);
    }

    public void Reveal()
    {
        IsRevealed = true;
        button.interactable = false;

        if (data.isWord)
        {
            wordText.text = data.word;
            wordText.gameObject.SetActive(true);
            imageDisplay.gameObject.SetActive(false);
        }
        else
        {
            imageDisplay.sprite = data.image;
            imageDisplay.gameObject.SetActive(true);
            wordText.gameObject.SetActive(false);
        }
        if (audioSource != null && data.sound != null)
        {
            audioSource.PlayOneShot(data.sound);
        }
    }

    public void Hide()
    {
        IsRevealed = false;
        button.interactable = true;
        wordText.gameObject.SetActive(false);
        imageDisplay.gameObject.SetActive(false);
    }

    public bool Matches(ButtonCard other)
    {
            return (this.data.word == other.data.word && this.data.isWord != other.data.isWord);
    }
}
