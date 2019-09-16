using UnityEngine;

public class FadeAlpha : MonoBehaviour
{
    private float fadePerSecond = 2.5f;

    private void Update()
    {
        var material = GetComponent<Renderer>().material;
        var color = material.color;

        material.color = new Color(color.r, color.g, color.b, color.a - (fadePerSecond * Time.deltaTime));
    }
}