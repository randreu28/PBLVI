using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparenterV2 : MonoBehaviour
{
    [SerializeField]
    private float _duration;

    private List<float> _defaultAlpha;

    [SerializeField]
    private float[] _activeAlpha;

    [SerializeField]
    private Material[] _materials;

    // Start is called before the first frame update
    void Start()
    {
        _defaultAlpha = new List<float>(_materials.Length);

        for (int i = 0; i < _materials.Length; i++)
        {
            _defaultAlpha.Add(_materials[i].GetColor("_BaseColor").a);
        }
    }

    private IEnumerator ChangeAlpha(Material material, float alpha, float duration)
    {
        Color color = material.GetColor("_BaseColor");
        float startAlpha = color.a;
        float endAlpha = alpha;
        for (float t = 0; t <= duration; t += Time.deltaTime)
        {
            float x = Mathf.Clamp01(t / duration);
            float f = 3 * Mathf.Pow(x, 2) - 2 * Mathf.Pow(x, 3);
            color.a = Mathf.Lerp(startAlpha, endAlpha, f);
            material.SetColor("_BaseColor", color);
            yield return null;
        }
        color.a = endAlpha;
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            for (int i = 0; i < _materials.Length; i++)
            {
                Debug.Log("StartColor: " + _materials[i].GetColor("_BaseColor"));
                StartCoroutine(ChangeAlpha(_materials[i], _activeAlpha[i], _duration));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            for (int i = 0; i < _materials.Length; i++)
            {
                StartCoroutine(ChangeAlpha(_materials[i], _defaultAlpha[i], _duration));
            }
        }
    }
}
