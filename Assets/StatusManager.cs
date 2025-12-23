using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    public GameObject Main;
    public int HP;
    public int MaxHP;
    public int MP;
    public Image HPGage;

    public string TagName;
    
    private void Update(){
        // HPが0以下になったら死亡処理
        if(HP <= 0){
            HP = 0;
            // 死亡エフェクトの生成
            var effect = Instantiate(Effect);
            effect.transform.position = transform.position;
            // 倒したときにプレイヤーのMPを回復させる
            GameObject.Find("PlyStatusManager").GetComponent<PlyStatusManager>().MP += MP;
            
            // 本体を削除
            Destroy(Main);
            Destroy(effect, 5);
        }

        // HPゲージの表示更新
        float percentHP = (float)HP / MaxHP;
        HPGage.fillAmount = percentHP;
    }
    // Start is called before the first frame update
    public GameObject Effect;

    // 攻撃判定（Collider）が当たった時の処理
    private void OnTriggerEnter(Collider other){
        // 指定したタグ（攻撃など）ならダメージを受ける
        if(other.tag == TagName){
            Damage();
        }
    }

    // ダメージ計算処理
    void Damage(){
        // プレイヤーの攻撃タイプ(e)を取得
        int a = GameObject.Find("unitychan").GetComponent<PlayerController>().e;
        
        // 強攻撃の場合
        if(a == 10){
            HP -= GameObject.Find("unitychan").GetComponent<PlayerController>().HighDamage;
            return;
        }
        // 中攻撃の場合
        if(a == 5){
            HP -= GameObject.Find("unitychan").GetComponent<PlayerController>().MidDamage;
            return;
        }
        // 通常攻撃
        HP--;
    }
}