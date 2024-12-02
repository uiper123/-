using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Dialog : MonoBehaviour
{
    public string[] lines;
    public float speedText;
    public TMP_Text dialogText;
    public int index;


    public float delay_dialog = 2f;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
      

        NextLine();
    }
    void StartDialog()
    { 
        dialogText.text = string.Empty;

        StartCoroutine(TypeLine());

    }

    IEnumerator TypeLine()
    {
        Debug.Log("ÇÀÏÓÑÊ ÂÛÂÎÄÀ");
        foreach (char c in lines[index].ToCharArray())
        {
            dialogText.text += c;
            yield return new WaitForSeconds(speedText);
        }
    }


    IEnumerator TypeLine2()
    {
        yield return new WaitForSeconds(delay_dialog);
        NextLine();

    }
    public void NextLine()
    {
        if(index>lines.Length-1)
        {
            index++;
            StartDialog();
            Debug.Log("index>0");
        }
        else
        {
            index = 0;
            StartDialog();
            Debug.Log("index<0");
        }
   
    }

}
