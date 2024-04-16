using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_EndingScene : InGameUI
{
    public List<Sprite> _images;
    public Image _fade;

    private EndingType _ending;
    private float _fadeTime = 1.3f;
    enum GameObjects
    {
        One,
        Two,
        Three,
        Ending,
        FadeImg,
        VideoImg,
        EndingTitle,
        EndingName,
        NextButton,
        NextButton2,
        GoToTitleButton
    }

    private void Start()
    {
        GetUI<Button>(GameObjects.GoToTitleButton.ToString()).onClick.AddListener(() => Manager.Scene.LoadScene("TitleScene"));
        GetUI<Button>(GameObjects.NextButton.ToString()).onClick.AddListener(OnClickNextButton);
        GetUI<Button>(GameObjects.NextButton2.ToString()).onClick.AddListener(OnClickNextButton2);


        GetUI(GameObjects.Two.ToString()).SetActive(false);
        GetUI(GameObjects.Three.ToString()).SetActive(false);
    }

    public void FlowEnding()
    {
        if (_ending == EndingType.Breakthrough)
            StartCoroutine(CoEndingFlowTwo());
        else
            GetUI(GameObjects.One.ToString()).SetActive(true);
        _fade.gameObject.SetActive(false);
    }

    public void SetImage(EndingType ending)
    {
        _ending = ending;
        Image img = GetUI<Image>(GameObjects.Ending.ToString());
        if (img != null)
            img.sprite = _images[(int)_ending];
    }
    IEnumerator CoEndingFlowTwo()
    {
        GetUI(GameObjects.Two.ToString()).SetActive(true);
        GetUI(GameObjects.NextButton2.ToString()).SetActive(false);
        yield return new WaitForSeconds(15f);
        GetUI(GameObjects.NextButton2.ToString()).SetActive(true);
    }

    IEnumerator FadeOut()
    {
        _fade.gameObject.SetActive(true);
        float rate = 0;
        Color fadeOutColor = new Color(_fade.color.r, _fade.color.g, _fade.color.b, 1f);
        Color fadeInColor = new Color(_fade.color.r, _fade.color.g, _fade.color.b, 0f);

        while (rate <= 1)
        {
            rate += Time.unscaledDeltaTime / _fadeTime;
            _fade.color = Color.Lerp(fadeInColor, fadeOutColor, rate);
            yield return null;
        }
    }
    IEnumerator FadeIn()
    {
        float rate = 0;
        Color fadeOutColor = new Color(_fade.color.r, _fade.color.g, _fade.color.b, 1f);
        Color fadeInColor = new Color(_fade.color.r, _fade.color.g, _fade.color.b, 0f);

        while (rate <= 1)
        {
            rate += Time.unscaledDeltaTime / _fadeTime;
            _fade.color = Color.Lerp(fadeOutColor, fadeInColor, rate);
            yield return null;
        }

        _fade.gameObject.SetActive(false);
    }

    public void OnClickNextButton()
    {
        if(_ending == EndingType.Breakthrough)
            StartCoroutine(Next(GameObjects.One, GameObjects.Two));
        else
            StartCoroutine(Next(GameObjects.Two, GameObjects.Three));
    }

    public void OnClickNextButton2()
    {
        StartCoroutine(Next(GameObjects.Two, GameObjects.Three));
    }
    IEnumerator Next(GameObjects from, GameObjects to)
    {
        yield return FadeOut();
        GetUI(from.ToString()).SetActive(false);
        GetUI(to.ToString()).SetActive(true);
        yield return FadeIn();
    }
}
