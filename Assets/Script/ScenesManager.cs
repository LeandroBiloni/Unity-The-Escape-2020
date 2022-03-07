using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ScenesManager : MonoBehaviour
{
    private Memory _memory;
    private string _sceneName;
    private void Start()
    {
        _sceneName = SceneManager.GetActiveScene().name;
        _memory = FindObjectOfType<Memory>();

        if (_sceneName == "Beta" || _sceneName == "Gamma") //CUANDO HAYA MAS NIVELES HAY QUE AGREGARLOS ACA
            _memory.activeLevel = _sceneName;
    }

	//PONER EL NOMBRE DE LA ESCENA EN CADA FUNCION
	public void Play()
    {
        _memory.activeLevel = "Gamma";
        SceneManager.LoadScene("LoadScreen"); 
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(_memory.activeLevel); //CARGA EL ULTIMO NIVEL JUGADO (PARA CUANDO HAYA MAS NIVELES)
    }
    public void LoseScreen()
    {
        Debug.Log("lose");
        SceneManager.LoadScene("Lose");
    }

    public void WinScreen()
    {
        SceneManager.LoadScene("Win");
    }

    public void Menu()
    {
        Time.timeScale = 1;
        Memory.Instance.activeLevel = "";
        SceneManager.LoadScene("Menu");
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene("LoadScreen"); 
    }

    public void Controls()
    {
        SceneManager.LoadScene("Controls");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
