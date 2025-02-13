using UnityEngine;

public class TutorialController : MonoBehaviour 
{
    public GameObject startCanvas; // ссылка на канвас туториала
    public GameObject nextCanvas;

    private void Start() 
    {
        startCanvas.SetActive(true); // открываем канвас после перехода на сцену
    }

    private void Update() 
    {
        if (Input.anyKeyDown) 
        {
            // переключаем на следующий канвас при нажатии любой клавиши
            nextCanvas.SetActive(true);
            startCanvas.SetActive(false); // скрываем текущий канвас
        }
    }

    public void HideTutorialCanvas()
    {
        if (startCanvas == null) 
            return;
        nextCanvas.SetActive(true);
        startCanvas.SetActive(false); // скрываем канвас при необходимости
    }
}