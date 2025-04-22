using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchingGameManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform gridParent;

    public List<WordImagePair> allPairs = new List<WordImagePair>();

    private List<ButtonCard> spawnedCards = new List<ButtonCard>();

    private ButtonCard firstSelected, secondSelected;
    private bool isChecking = false;

    void Start()
    {
        SetupGame();
    }

    void SetupGame()
    {
        List<WordImagePair> selectedPairs = new List<WordImagePair>();
        while (selectedPairs.Count < 8)
        {
            WordImagePair candidate = allPairs[Random.Range(0, allPairs.Count)];
            if (!selectedPairs.Contains(candidate)) selectedPairs.Add(candidate);
        }

        List<ButtonCard.Data> cardDataList = new List<ButtonCard.Data>();
       foreach (var pair in selectedPairs)
{
    // Word card: has word only
        cardDataList.Add(new ButtonCard.Data { word = pair.word, sound = pair.sound, isWord = true });

    // Image card: also has the same word so we can match them
            cardDataList.Add(new ButtonCard.Data { word = pair.word, image = pair.image, sound = pair.sound, isWord = false });
}


        Shuffle(cardDataList);

        foreach (var data in cardDataList)
        {
            GameObject obj = Instantiate(buttonPrefab, gridParent);
            ButtonCard card = obj.GetComponent<ButtonCard>();
            card.SetData(data, this);
            spawnedCards.Add(card);
        }
    }

    public void OnCardSelected(ButtonCard card)
    {
        if (isChecking || card.IsRevealed || card == firstSelected) return;

        card.Reveal();

        if (firstSelected == null)
        {
            firstSelected = card;
        }
        else
        {
            secondSelected = card;
            isChecking = true;
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
{
    // Disable all buttons
    foreach (var card in spawnedCards)
        card.button.interactable = false;

    yield return new WaitForSeconds(0.5f); // Optional: wait briefly so players can *see* the second card

    if (firstSelected.Matches(secondSelected))
    {
        // Keep them revealed
    }
    else
    {
        yield return new WaitForSeconds(1f);
        firstSelected.Hide();
        secondSelected.Hide();
    }

    firstSelected = secondSelected = null;
    isChecking = false;

    // Re-enable only hidden cards
    foreach (var card in spawnedCards)
        if (!card.IsRevealed)
            card.button.interactable = true;
}


    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}

