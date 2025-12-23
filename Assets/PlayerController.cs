using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform Camera;
    public float PlayerSpeed;
    public float RotationSpeed;
    public int HPUP;
    public int MPUP;
    public int MidClassMP; // 中級魔法の消費MP
    public int HighClassMP; // 上級魔法の消費MP
    public int MidDamage;
    public int HighDamage;

    Vector3 speed = Vector3.zero;
    Vector3 rot = Vector3.zero;

    public Animator PlayerAnimator;
    bool isRun;

    public Collider WeaponCollider;
    public Collider WeaponCollider2;
    bool canMove = true; // 行動可能フラグ

    // 各種エフェクトのPrefab
    public GameObject EffectSPACE;
    public GameObject EffectENTER;
    public GameObject EffectENTER2;
    public GameObject EffectMP;
    public GameObject EffectHP;
    public GameObject EffectEXHP;
    public GameObject EffectON; // 現在セットされているエフェクト
    
    // エフェクトの発生位置
    public Transform EffectPlace;
    public Transform EffectPlace2;
    public Transform SteEffectPlace;
    
    public int e; // 攻撃や魔法の種類ID
    public int eP = 0; // エフェクト発生位置の切り替え用

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();      // 移動
        Rotation();  // 回転
        KeyTouch();  // 攻撃・魔法入力
        Camera.transform.position = transform.position; // カメラ追従
    }

    // 移動処理
    void Move(){
        if(canMove == false)return; // 攻撃中などは動けない
        speed = Vector3.zero;
        rot = Vector3.zero;
        rot.y = 180;
        isRun = false;

        // WASDキー入力で移動方向と向きを設定
        if(Input.GetKey(KeyCode.W)){
            rot.y = 180;
            MoveSet();
        }
        if(Input.GetKey(KeyCode.S)){
            rot.y = 0;
            MoveSet();
        }
        if(Input.GetKey(KeyCode.A)){
            rot.y = 90;
            MoveSet();
        }
        if(Input.GetKey(KeyCode.D)){
            rot.y = -90;
            MoveSet();
        }
        transform.Translate(speed);
        PlayerAnimator.SetBool("run", isRun); // 走りアニメーション
    }

    // 移動の共通設定
    void MoveSet(){
        speed.z = PlayerSpeed;
        transform.eulerAngles = Camera.transform.eulerAngles + rot; // カメラの向きに合わせて移動
        isRun = true;
        /*TimeCount += Time.deltaTime;
        if(TimeCount > 5){
            audioSource.PlayOneShot(RunSE);
            TimeCount = 0;
        }*/
    }

    // 回転処理（カメラ操作）
    void Rotation(){
       var speed = Vector3.zero;
        if(Input.GetKey(KeyCode.LeftArrow)){
            speed.y = -RotationSpeed;
        }
        if(Input.GetKey(KeyCode.RightArrow)){
            speed.y = RotationSpeed;
        } 

        Camera.transform.eulerAngles += speed;
    }

    // キー入力による攻撃・魔法の発動
    void KeyTouch(){
        if(canMove == false)return;

        // Space: 通常攻撃
        if(Input.GetKeyDown(KeyCode.Space)){
            EffectON = EffectSPACE;
            PlayerAnimator.SetBool("attack", true);
            canMove = false;
        }
        // Enter: 強攻撃（MP消費あり）
        if(Input.GetKeyDown(KeyCode.Return)
         && GameObject.Find("PlyStatusManager").GetComponent<PlyStatusManager>().MP >= HighClassMP){
            e = 10;
            EffectON = EffectENTER;
            PlayerAnimator.SetBool("HighAttack", true);
            canMove = false;
        }
        // 下キー: MP回復魔法
        if(Input.GetKeyDown(KeyCode.DownArrow)){
            e = 0;
            EffectON = EffectMP;
            PlayerAnimator.SetBool("StatusMagic", true);
            canMove = false;
        }
        // 上キー: HP回復魔法
        if(Input.GetKeyDown(KeyCode.UpArrow)){
            e = 1;
            EffectON = EffectHP;
            PlayerAnimator.SetBool("StatusMagic", true);
            canMove = false;
        }
        // 右Shift: 特殊回復（HP大回復・MP消費）
        if(Input.GetKeyDown(KeyCode.RightShift)
         && GameObject.Find("PlyStatusManager").GetComponent<PlyStatusManager>().MP >= MidClassMP){
            e = 2;
            EffectON = EffectEXHP;
            PlayerAnimator.SetBool("StatusMagic", true);
            canMove = false;
        }
        // Slash(/): 中攻撃（MP消費あり）
        if(Input.GetKeyDown(KeyCode.Slash)
         && GameObject.Find("PlyStatusManager").GetComponent<PlyStatusManager>().MP >= MidClassMP){
            e = 5; eP=2;
            EffectON = EffectENTER2;
            PlayerAnimator.SetBool("HighAttack", true);
            canMove = false;
        }
    }

    // アニメーションイベントから呼ばれる：武器の当たり判定ONとエフェクト発生
    void WeaponON(){
        var effect = Instantiate(EffectON);
        // エフェクトの発生位置調整
        if(eP == 0)effect.transform.position = EffectPlace.transform.position;
        if(eP == 2)effect.transform.position = EffectPlace2.transform.position;
        
        // MP消費
        if(e == 10)GameObject.Find("PlyStatusManager").GetComponent<PlyStatusManager>().MP -= HighClassMP;
        if(e == 5)GameObject.Find("PlyStatusManager").GetComponent<PlyStatusManager>().MP -= MidClassMP;
        
        Destroy(effect, 10);
        
        // 攻撃の種類に応じてColliderを有効化
        if(e == 10 || e == 5)WeaponCollider2.enabled = true;
        else WeaponCollider.enabled = true;
    }

    // アニメーションイベントから呼ばれる：武器の当たり判定OFF
    void WeaponOFF(){
        
        if(e == 10 || e == 5){
            PlayerAnimator.SetBool("HighAttack", false);
            WeaponCollider2.enabled = false;
            e = 0; eP=0;
            return;
        }
        WeaponCollider.enabled = false;
        PlayerAnimator.SetBool("attack", false);
    }

    // アニメーション終了後に動けるようにする
    void CanMove(){
        canMove = true;
    }

    // アニメーションイベントから呼ばれる：ステータス変化の実処理
    void StatusUP(){
        var effect = Instantiate(EffectON);
        effect.transform.position = SteEffectPlace.transform.position;
        
        if(e == 0)GameObject.Find("PlyStatusManager").GetComponent<PlyStatusManager>().MP += MPUP; // MP回復
        if(e == 1)GameObject.Find("PlyStatusManager").GetComponent<PlyStatusManager>().HP += HPUP; // HP回復
        if(e == 2){ // HP大回復
            GameObject.Find("PlyStatusManager").GetComponent<PlyStatusManager>().HP += 100;
            GameObject.Find("PlyStatusManager").GetComponent<PlyStatusManager>().MP -= MidClassMP;
        }
        Destroy(effect, 10);
    }

    // ステータス魔法アニメーションの終了処理
    void StatusUPOFF(){
        PlayerAnimator.SetBool("StatusMagic", false);
    }
}