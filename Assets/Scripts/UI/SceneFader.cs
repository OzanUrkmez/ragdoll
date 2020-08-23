using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image img;
    public AnimationCurve curve;

    //Fades in at the start
    void Start ()
    {
        StartCoroutine(FadeIn());
    }

    //Fades into a scene
    //scene is the name of the scene being called
    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }


    //Animates slowly over time
    IEnumerator FadeIn ()
    {
        float time = 1f;
        while (time>0f)
        {
            time -= Time.deltaTime;
            float a = curve.Evaluate(time);
            img.color = new Color (0f, 0f, 0f, a); //Starts off black and slowly loses its colour and becomes transparent
            yield return 0;
        }
    }

    //Animates slowly over time
    IEnumerator FadeOut(string scene)
    {
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime;
            float a = curve.Evaluate(time);
            img.color = new Color(0f, 0f, 0f, a); //Starts off transparent and slowly become black
            yield return 0;
        }

        SceneManager.LoadScene(scene);
    }

}
