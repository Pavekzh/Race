using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class RaceFinishUI : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private TMP_Text state;
    [SerializeField] private TMP_Text time;
    [SerializeField] private Button quit;

    private bool isTimeSaved;
    private float bestTime;

    private FirebaseDatabaseService databaseService;
    private SceneLoader sceneLoader;

    public void Init(FirebaseDatabaseService databaseService,SceneLoader sceneLoader)
    {
        this.databaseService = databaseService;
        this.sceneLoader = sceneLoader;

        databaseService.GetBestTime(best => bestTime = best);

        panel.gameObject.SetActive(false);
        quit.onClick.AddListener(() => StartCoroutine(Quit()));
        quit.interactable = false;
    }

    /// <summary>
    /// Open race finish message DNF
    /// </summary>
    public void ShowUI(bool isWin)
    {
        panel.gameObject.SetActive(true);
        isTimeSaved = true;

        state.text = isWin ? "You Win" : "Game over";
        time.text = "DNF";
    }

    /// <summary>
    /// Open rce finish massage with finish time
    /// </summary>
    public void ShowUI(bool isWin, float timeInMinutes)
    {
        panel.gameObject.SetActive(true);

        if (bestTime == 0 || timeInMinutes < bestTime)
            databaseService.UpdateBestTime(timeInMinutes, () => isTimeSaved = true);
        else
            isTimeSaved = true;

        state.text = isWin ? "You Win" : "Game over";
        time.text = new DateTime().AddMinutes(timeInMinutes).ToString("m:ss.ff");
    }

    public void UnlockQuit()
    {
        quit.interactable = true;
    }

    private IEnumerator Quit()
    {
        yield return new WaitUntil(() => isTimeSaved);
        sceneLoader.LoadMenu();
    }
}

