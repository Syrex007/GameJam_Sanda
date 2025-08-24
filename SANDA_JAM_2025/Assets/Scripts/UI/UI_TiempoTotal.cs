using UnityEngine;
using TMPro;

public class UI_TiempoTotal : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tiempoTotalText;

    void Start()
    {
        MostrarTiempoTotal();
    }

    public void MostrarTiempoTotal()
    {
        if (GameManager.instance == null) return;

        // Aseg�rate de que el tiempo total est� calculado
        GameManager.instance.tiempoTotal = 0f;
        for (int i = 0; i < GameManager.instance.times.Length; i++)
        {
            GameManager.instance.tiempoTotal += GameManager.instance.times[i];
        }

        float totalSeconds = GameManager.instance.tiempoTotal;
        int totalMilliseconds = Mathf.FloorToInt(totalSeconds * 1000f);

        int minutes = totalMilliseconds / 60000;
        int seconds = (totalMilliseconds % 60000) / 1000;
        int milliseconds = (totalMilliseconds % 1000) / 10; // cent�simas

        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}", minutes, seconds, milliseconds);
        tiempoTotalText.text = formattedTime;
    }
}
