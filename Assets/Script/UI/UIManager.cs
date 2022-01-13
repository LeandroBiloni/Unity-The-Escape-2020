using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _boyOnIcon;
    [SerializeField] private GameObject _boyOffIcon;
    
    [SerializeField] private GameObject _girlOnIcon;
    [SerializeField] private GameObject _girlOffIcon;

    [SerializeField] private Image _girlPowerBar;

    [SerializeField] private GameObject _canvas;
    // Start is called before the first frame update
    void Start()
    {
        var selector = FindObjectOfType<CharacterSelector>();
        selector.OnBoySelect += BoyIconOn;
        selector.OnGirlSelect += GirlIconOn;
        var girl = FindObjectOfType<Girl>();
        girl.OnTimerRunning += UpdateControlTimeBar;

        var cinematic = FindObjectOfType<Cinematic>();

        cinematic.OnCinematicEnd += ActivateCanvas;
        cinematic.OnCinematicStart += DeactivateCanvas;
    }

    private void GirlIconOn()
    {
        _girlOnIcon.SetActive(true);
        _boyOffIcon.SetActive(true);
        _girlOffIcon.SetActive(false);
        _boyOnIcon.SetActive(false);
        
        _girlPowerBar.transform.gameObject.SetActive(true);
    }

    private void BoyIconOn()
    {
        _boyOnIcon.SetActive(true);
        _girlOffIcon.SetActive(true);
        _boyOffIcon.SetActive(false);
        _girlOnIcon.SetActive(false);
        
        _girlPowerBar.transform.gameObject.SetActive(false);
    }

    private void UpdateControlTimeBar(float value)
    {
        _girlPowerBar.fillAmount = value;
    }

    private void ActivateCanvas()
    {
        _canvas.SetActive(true);
    }

    private void DeactivateCanvas()
    {
        _canvas.SetActive(false);
    }
}
