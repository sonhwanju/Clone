using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRootGame : MonoBehaviour
{
    [SerializeField]
    private Button nextWaveBtn;

    public GameObject inGameImg;
    public GameObject waveImg;

    public Image hpBar;
    public Text hpText;

    public Text waveText;

    public Button skillBtn1;
    public Button skillBtn2;

    private Image skillBtn1Fill;
    private Image skillBtn2Fill;

    float maxCool = 5f;
    float curSkill1 = 5f;
    float curSkill2 = 5f;

    void Awake()
    {
        GameSceneClass.gUiRootGame = this;
    }

    private void Start()
    {
        skillBtn1Fill = skillBtn1.GetComponent<Image>();
        skillBtn2Fill = skillBtn2.GetComponent<Image>();

        nextWaveBtn.onClick.AddListener(() =>
        {
            ChangeImg(false);
            curSkill1 = curSkill2 = maxCool;
            StartCoroutine(GameSceneClass.gMGGame.WaveStart());
        });

        skillBtn1.onClick.AddListener(() =>
        {
            if(curSkill1 >= maxCool)
            {
                curSkill1 = 0f;
                GameSceneClass.gMGGame.girl.Back();
            }
        });

        skillBtn2.onClick.AddListener(() =>
        {
            if (curSkill2 >= maxCool)
            {
                curSkill2 = 0f;
                GameSceneClass.gMGGame.man.Back();
            }
        });
    }

    public void SetImageFill(Image fillImg,float value)
    {
        fillImg.fillAmount = value;
    }

    public void SetHpBar(float value,string s)
    {
        hpBar.fillAmount = value;
        hpText.text = s;
    }

    public void SetWaveText(string s)
    {
        waveText.text = s;
    }

    public void ChangeImg(bool on)
    {
        waveImg.SetActive(!on);
        inGameImg.SetActive(on);
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //List<string> keyList = new List<string>(Global.spritesDic.Keys);
            //int randomIdx = Random.Range(0, keyList.Count - 1);
            
            //testImage.sprite = Global.spritesDic[keyList[randomIdx]];
            //testImage.SetNativeSize();
        }

        if(curSkill1 <= maxCool)
        {
            curSkill1 += Time.deltaTime;
            SetImageFill(skillBtn1Fill, curSkill1 / maxCool);
        }

        if(curSkill2 <= maxCool)
        {
            curSkill2 += Time.deltaTime;
            SetImageFill(skillBtn2Fill, curSkill2 / maxCool);
        }
    }

    public void TestFunc()
    {
        print("call UIRootGame");
    }
}
