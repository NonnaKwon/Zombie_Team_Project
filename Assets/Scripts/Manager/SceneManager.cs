using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : Singleton<SceneManager>
{
    [SerializeField] Image fade;
    [SerializeField] Image ending;
    [SerializeField] Slider loadingBar;
    [SerializeField] float fadeTime;

    private BaseScene curScene;

    public BaseScene GetCurScene()
    {
        if (curScene == null)
        {
            curScene = FindObjectOfType<BaseScene>();
        }
        return curScene;
    }

    public T GetCurScene<T>() where T : BaseScene
    {
        if (curScene == null)
        {
            curScene = FindObjectOfType<BaseScene>();
        }
        return curScene as T;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadingRoutine(sceneName));
    }

    public void LoadEndingScene(string sceneName,Define.EndingType ending)
    {
        StartCoroutine(EndingRoutine(sceneName, ending));
    }

    IEnumerator LoadingRoutine(string sceneName)
    {
        fade.gameObject.SetActive(true);
        yield return FadeOut(fade);

        Manager.Pool.ClearPool();
        Manager.Sound.StopSFX();
        Manager.UI.ClearPopUpUI();
        Manager.UI.ClearWindowUI();
        Manager.UI.CloseInGameUI();

        Time.timeScale = 0f;
        loadingBar.gameObject.SetActive(true);

        AsyncOperation oper = UnitySceneManager.LoadSceneAsync(sceneName);
        while (oper.isDone == false)
        {
            loadingBar.value = oper.progress;
            yield return null;
        }

        Manager.UI.EnsureEventSystem();

        BaseScene curScene = GetCurScene();
        yield return curScene.LoadingRoutine();

        loadingBar.gameObject.SetActive(false);
        Time.timeScale = 1f;

        yield return FadeIn(fade);
        fade.gameObject.SetActive(false);
    }

    IEnumerator EndingRoutine(string sceneName,Define.EndingType endingType)
    {
        ending.gameObject.SetActive(true);
        yield return FadeOut(ending);
        Time.timeScale = 0f;
        if (endingType == Define.EndingType.Breakthrough || endingType == Define.EndingType.InsatiableHunger)
            ending.GetComponentInChildren<TMP_Text>().text = "You Die";
        else
            ending.GetComponentInChildren<TMP_Text>().text = "Clear";
        Manager.Pool.ClearPool();
        Manager.Sound.StopSFX();
        Manager.UI.ClearPopUpUI();
        Manager.UI.ClearWindowUI();
        Manager.UI.CloseInGameUI();
        AsyncOperation oper = UnitySceneManager.LoadSceneAsync(sceneName);

        while (oper.isDone == false)
        {
            yield return null;
        }

        Manager.UI.EnsureEventSystem();

        EndingScene curScene = GetCurScene() as EndingScene;
        curScene.Ending = endingType;

        Time.timeScale = 1f;

        yield return FadeIn(ending);
        ending.gameObject.SetActive(false);

        yield return curScene.LoadingRoutine();
    }

    IEnumerator FadeOut(Image fade)
    {
        float rate = 0;
        Color fadeOutColor = new Color(fade.color.r, fade.color.g, fade.color.b, 1f);
        Color fadeInColor = new Color(fade.color.r, fade.color.g, fade.color.b, 0f);

        while (rate <= 1)
        {
            rate += Time.unscaledDeltaTime / fadeTime;
            fade.color = Color.Lerp(fadeInColor, fadeOutColor, rate);
            yield return null;
        }
    }

    IEnumerator FadeIn(Image fade)
    {
        float rate = 0;
        Color fadeOutColor = new Color(fade.color.r, fade.color.g, fade.color.b, 1f);
        Color fadeInColor = new Color(fade.color.r, fade.color.g, fade.color.b, 0f);

        while (rate <= 1)
        {
            rate += Time.unscaledDeltaTime / fadeTime;
            fade.color = Color.Lerp(fadeOutColor, fadeInColor, rate);
            yield return null;
        }
    }
}
