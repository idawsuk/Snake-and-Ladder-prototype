using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dice : MonoBehaviour
{
    public delegate void DiceThrow(int value);
    public DiceThrow OnDiceThrow;

    [SerializeField] TextMeshProUGUI diceText;
    [SerializeField] Button diceButton;

    [SerializeField] bool debug;
    [SerializeField] int debugValue;

    // Start is called before the first frame update
    void Start()
    {
        diceButton.onClick.AddListener(ThrowDice);
    }

    // Update is called once per frame
    void Update()
    {
        if(debug)
        {
            debug = false;
            OnDiceThrow?.Invoke(debugValue);
        }
    }

    public void ThrowDice()
    {
        int value = Random.Range(1, 7); // will randomize value from 1 to 7, because the second parameter is exclusive

        diceText.text = value.ToString();
        OnDiceThrow?.Invoke(value);

        EnableButton(false);
    }

    public void EnableButton(bool active)
    {
        diceButton.interactable = active;
    }
}
