using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject Player;
    public GameObject GameOverCanvas;
    float Timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーが存在しない（Destroyされた）場合
        if(!Player){
            Timer += Time.deltaTime;
            // 1.5秒待ってからゲームオーバー画面を出す
            if(Timer >= 1.5){
                GameOverCanvas.SetActive(true);
                Timer = 0;
            }
        }
        
    }

    // ゲームを最初からやり直す
    public void GameReStart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // バグなどで位置がおかしくなった時の緊急リセット用
    public void BugReset(){
       Vector3 tmp = GameObject.Find("unitychan").transform.position;
       GameObject.Find("unitychan").transform.position = new Vector3(25f, 3f, 68f);
    }
}