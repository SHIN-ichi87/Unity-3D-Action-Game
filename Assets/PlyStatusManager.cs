using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlyStatusManager : MonoBehaviour
{
    public GameObject Main;
    public int HP;
    public int MaxHP;
    public Image HPGage;
    public int MP;
    public int MaxMP;
    public Image MPGage;
    float TimeCount;

    public string TagName;
    
    private void Update(){
        // HPがなくなった時の死亡処理
        if(HP <= 0){
            HP = 0;
            var effect = Instantiate(Effect);
            effect.transform.position = transform.position;
            Destroy(Main);
            Destroy(effect, 5);
        }
        // 最大値を超えないように補正
        if(HP >= MaxHP)HP = MaxHP;
        if(MP >= MaxMP)MP = MaxMP;

        // UIゲージの更新
        float percentHP = (float)HP / MaxHP;
        HPGage.fillAmount = percentHP;
        float percentMP = (float)MP / MaxMP;
        MPGage.fillAmount = percentMP;
    }
    // Start is called before the first frame update
    public GameObject Effect;

    // 敵などに触れ続けている時の処理
    private void OnTriggerStay(Collider other){
        TimeCount += Time.deltaTime;
        // 指定タグに3秒以上触れ続けていたらダメージ
        if(other.tag == TagName && TimeCount > 3){
            DamageP();
            TimeCount = 0;
        }
    }

    // ダメージ処理
    void DamageP(){
        HP--;
    }
}