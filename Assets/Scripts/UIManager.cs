using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    
    [SerializeField]
    private TMP_Text _scoreText;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private TMP_Text _gameOverText;
    [SerializeField]
    private TMP_Text _restartText;
    [SerializeField]
    private Slider _speedBoostGauge;
    [SerializeField]
    private Slider _healthGauge;
    [SerializeField]
    private TMP_Text _ammoDisplay;
    


    private GameManager _gameManager;
    void Start()
    {
        _ammoDisplay.enabled = false;
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _speedBoostGauge.value = 0;

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL.");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
      
        _LivesImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            
          GameOverSequence(); 

        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void SpeedBoostGauge (float boostAmount)
    {
        _speedBoostGauge.value = boostAmount;

        _speedBoostGauge.maxValue = 100f;
        _speedBoostGauge.minValue = 0f;
    }
    
    /*public void HealthGauge (float healthAmount)
    {
        _healthGauge.value = healthAmount;

        _healthGauge.maxValue = 100f;
        _healthGauge.minValue = 0f;
    }*/

   public void UpdateAmmoDisplay(int currentAmmo, int maxAmmo)
    {
        _ammoDisplay.text = currentAmmo + "/" + maxAmmo;
    }
    
    public void SetAmmoEnabled()
    {
        _ammoDisplay.enabled = true;
    }

    public void SetAmmoDisabled()
    {
        _ammoDisplay.enabled = false;
    }
    
    
    
   
  
}
