using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // 出現させる敵のPrefab
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;
    public GameObject Enemy4;
    public GameObject Enemy5;
    public GameObject Enemy6a;
    public GameObject Enemy6b;

    // 出現位置
    public Transform EnemyPlace1;
    public Transform EnemyPlace2;
    public Transform EnemyPlace3;
    public Transform EnemyPlace4;
    public Transform EnemyPlace5;
    public Transform EnemyPlace6;

    float TimeCount;

    public int Count;
    public int MaxCount; // 敵の最大出現数

    // Start is called before the first frame update
    void Start()
    {
        // ゲーム開始時に初期配置する敵
        Instantiate(Enemy3, EnemyPlace3.position, Quaternion.identity);
        Instantiate(Enemy4, EnemyPlace4.position, Quaternion.identity);
        Instantiate(Enemy5, EnemyPlace5.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        // 最大数を超えていたら生成しない
        if(MaxCount <= Count){
            return;
        }

        TimeCount += Time.deltaTime;
        // 1秒ごとに敵をセットで生成
        if(TimeCount > 1){
            Instantiate(Enemy1, EnemyPlace1.position, Quaternion.identity);
            Count++;
            Instantiate(Enemy2, EnemyPlace2.position, Quaternion.identity);
            Count++;
            Instantiate(Enemy6a, EnemyPlace6.position, Quaternion.identity);
            Count++;
            Instantiate(Enemy6b, EnemyPlace6.position, Quaternion.identity);
            Count++;
            TimeCount = 0;
        }
    }
}