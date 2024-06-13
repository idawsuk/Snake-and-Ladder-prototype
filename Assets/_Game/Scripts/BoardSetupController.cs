using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardSetupController : MonoBehaviour
{
    [SerializeField] Slider ladderCount;
    [SerializeField] Slider snakeCount;
    [SerializeField] TMPro.TextMeshProUGUI ladderCountText;
    [SerializeField] TMPro.TextMeshProUGUI snakeCountText;

    // Start is called before the first frame update
    void Start()
    {
        ladderCount.value = PlayerPrefs.GetInt("ladder", 2);
        snakeCount.value = PlayerPrefs.GetInt("snake", 2);

        ladderCountText.text = ladderCount.value.ToString();
        snakeCountText.text = snakeCount.value.ToString();

        ladderCount.onValueChanged.AddListener(LadderCountOnValueChanged);
        snakeCount.onValueChanged.AddListener(SnakeCountOnValueChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LadderCountOnValueChanged(float value)
    {
        ladderCountText.text = value.ToString();
    }

    void SnakeCountOnValueChanged(float value)
    {
        snakeCountText.text = value.ToString();
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("ladder", (int)ladderCount.value);
        PlayerPrefs.SetInt("snake", (int)snakeCount.value);
    }
}
