using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public Image Cover;
    public AnimationCurve CoverCurve;

    
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(ShowText(new[]
        {
            "One puzzle's end",
            "leads to another.",
            "The wheel of life.",
        }));
        
        yield return new WaitForSeconds(2.5f);

        yield return StartCoroutine(ShowText(new[]
        {
            "Made by Torreng North",
            "Enes Ugur Sekerci: Design / Programming",
            "Atil Kockar: Design / Programming",
            "Damla Kockar: Music / SFX",
        }));
        
        yield return new WaitForSeconds(1.5f);
        
        yield return StartCoroutine(ShowText(new[]
        {
            "Thank you for playing",
        }));

        yield return new WaitForSeconds(1.5f);
        const float coverDuration = 1f;
        for (float f = 0; f < coverDuration; f += Time.deltaTime)
        {
            Color c = Cover.color;
            c.a = Mathf.Lerp(0f, 1f, CoverCurve.Evaluate(f / coverDuration));
            Cover.color = c;
            yield return null;
        }

        SceneManager.LoadScene("Splash");
    }

    private IEnumerator ShowText(string[] lines)
    {
        Text.text = "";
        foreach (string line in lines)
        {
            foreach (char ch in line)
            {
                Text.text += ch;
                yield return new WaitForSeconds(0.05f); // Wait per char
            }
            
            yield return new WaitForSeconds(0.2f); // Wait per line
            Text.text += "\n";
        }
    }
}