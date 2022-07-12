using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text Highscore;
    
    private bool m_Started = false;
    private int m_Points;
    private int m_hPoints;
    
    private bool m_GameOver = false;
    private string m_pName ;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        string path = Application.persistentDataPath+"/savefile.json";
        m_pName = Manager.Instance.m_name;

        if(File.Exists(path)){
            SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
            m_hPoints = data.HighScore;
            Highscore.text = $"Best Score : {data.HighScore}";
        }
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }

        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        string path = Application.persistentDataPath+"/savefile.json";
     
        string s = "" ;
        if(File.Exists(path)){
            SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
            s = data.Name;
        }else{
            s = m_pName;
        }
       
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        if(m_Points >= m_hPoints){
            m_hPoints  = m_Points;
            s = m_pName;
        }
        Highscore.text = $"Best Score : {s} {m_hPoints}";
    }

    public void GameOver()
    {
        SaveData data = new SaveData();
        data.HighScore = m_hPoints;
        data.Name= m_pName;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath+"/savefile.json",json);
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
