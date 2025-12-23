using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    float Timer;
    public float ChangeTime;
    public float EnemySpeed;

    GameObject Target;
    // Start is called before the first frame update
    void Start()
    {
        //Destroy(this.gameObject, time);
    }

    // Update is called once per frame
    void Update()
    {
        var speed = Vector3.zero;
        speed.z = EnemySpeed;
        var rot = transform.eulerAngles;

        // ターゲット（プレイヤー）がいる場合は追いかける
        if(Target){
            transform.LookAt(Target.transform);
            this.transform.Translate(speed);
        }else{
            // ターゲットがいない場合はランダム移動
            Timer += Time.deltaTime;
            // 一定時間ごとに向きを変える
            if(ChangeTime <= Timer){
                Timer = 0;
                float rand = Random.Range(0, 360);
                rot.y = rand;
                rot.x = 0;
                rot.z = 0;
                transform.eulerAngles = rot;
            }
            this.transform.Translate(speed);
        }
    }

    // プレイヤーを見つけた時の処理
    private void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            Target = other.gameObject;
        }
    }

    // プレイヤーを見失った時の処理
    private void OnTriggerExit(Collider other){
        if(other.tag == "Player"){
            Target = null;
        }
    }
}