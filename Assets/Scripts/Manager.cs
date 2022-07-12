using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public string m_name ;
    public GameObject InputTextField;
    public Text name;

    public static Manager Instance;

    private void Awake()
    {
        //helps to maintain singleton
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
       
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void onStart(){
        SceneManager.LoadScene(1);
    }

    public void onTextGiven(string s){
       
        m_name = InputTextField.GetComponent<Text>().text;
        name.text = $"welcome {m_name}";
        
    }
}
