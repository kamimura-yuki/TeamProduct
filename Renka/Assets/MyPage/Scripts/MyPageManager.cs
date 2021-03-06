﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MyPageManager : MonoBehaviour
{

    [System.Serializable]
    public struct CharacterVisualVariation_
    {
        //体の画像
        //public Texture bodyTex;

        //表情画像の配列
        public Texture[] faceTexs;

        //服装画像の配列
        //public Texture[] clothesTexs;
    }

    [SerializeField, Tooltip("コメントを表示させるためのタップ範囲のオブジェ")]
    Image commentArea;

    [SerializeField, Tooltip("上のアルファ値")]
    float commentAreaAlpha;

    [System.Serializable]
    public struct characterComments
    {
        //表情画像の配列
        public likeComments[] likeComment;
    }

    [System.Serializable]
    public struct likeComments
    {
        //表情画像の配列
        public string[] comment;
        public string[] voiceName;
    }

    [SerializeField, Tooltip("キャラクターのコメント")]
    characterComments[] comments;

    [SerializeField, Tooltip("コメントを表示するText")]
    Text commentText;

    [SerializeField, Tooltip("攻略中のキャラクターのテクスチャ")]
    Texture texture;

    [SerializeField, Tooltip("キャラクターを描画するイメージの参照")]
    RawImage image;

    [SerializeField, Tooltip("キャラクターの顔を描画するイメージの参照")]
    RawImage faceImage;

    [SerializeField, Tooltip("キャラクターの服装を描画するイメージの参照")]
    RawImage clothesImage;

    [SerializeField, Tooltip("表示するキャラのトランスフォーム")]
    RectTransform rectTrans;

    [SerializeField]
    Button storyButton;

    //前のコメントの配列番号
    int commentPrevLine;

    [SerializeField, Tooltip("好感度ゲージ")]
    Slider gauge;
    [SerializeField, Tooltip("現在の好感度")]
    Text likeNum;

    [SerializeField, Tooltip("好感度の最大値")]
    int likeMax;

    [SerializeField, Tooltip("描画するかもしれないキャラのデータ")]
    CharacterVisualVariation_[] charVariations;

    [SerializeField, Tooltip("キャラクターの後ろの背景")]
    RawImage back;

    [SerializeField, Tooltip("使う背景の一覧")]
    Texture[] backs;
    [SerializeField]
    GameObject[] testCanvas;

    [SerializeField, Tooltip("BGM")]
    AudioClip bgm;

    int texID = 0;
    int canvasID = 0;
    int likeID = 0;

    void Start()
    {
        //Debug.Log(SceneChanger.GetBeforeSceneName());
        Fade.Instance.FadeOut(0.5f, null);

        if (DataManager.Instance.isEndChapter() && DataManager.Instance.isEndStory())
        {
            storyButton.interactable = false;
        }

        SceneChanger.ResetBeforeScene();

        //コメントエリアのアルファ値を0して、描画をしなくする
        //ボタン判定は残す
        var col = commentArea.color;
        col.a = commentAreaAlpha;
        commentArea.color = col;

        //0だと配列の０番目と重なるので
        commentPrevLine = -1;

        //好感度の最大値と現在の好感度からゲージの割合を決める
        gauge.value = (float)DataManager.Instance.masteringData.likeabillity / (float)likeMax;

        likeNum.text = DataManager.Instance.masteringData.likeabillity.ToString();

        likeID = Mathf.Min(DataManager.Instance.masteringData.readChapterID / 4, 2);

        //キャラクターの生成
        var i = DataManager.Instance.masteringData.masteringCharacterID;
        if (i >= 0 && i < 4)
        {
            //キャラのセット
            //image.texture = charVariations[i].bodyTex;
            faceImage.gameObject.SetActive(false);
            //clothesImage.texture = charVariations[i].clothesTexs[0];
            rectTrans.anchoredPosition = new Vector3(0, -900, 0);
            image.texture = charVariations[i].faceTexs[0];
        }
        else
        {
            //辰己
            faceImage.gameObject.SetActive(false);
            rectTrans.anchoredPosition = new Vector3(0, -900, 0);
            image.texture = charVariations[0].faceTexs[0];
        }

        if (i>=0 && comments.Length >= i)
        {
            var line = Random.Range(0, comments[i].likeComment[likeID].comment.Length);
            commentText.text = comments[i].likeComment[likeID].comment[line];
        }

        //if(SoundManager.Instance.GetNowPlayBGMName() != bgm.name)
        SoundManager.Instance.PlayBGM(bgm.name);
    }
    
    //全部デバッグ用
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            testCanvas[0].SetActive(!testCanvas[0].activeInHierarchy);
            testCanvas[1].SetActive(!testCanvas[1].activeInHierarchy);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            texID++;
            if(texID >= backs.Length) { texID = 0;}
            back.texture = backs[texID];
        }
    }

    /// <summary>
    /// Storyボタンが押されたときに呼ばれる
    /// </summary>
    public void OnClickStory()
    {
        //Debug.Log(DataManager.Instance.endLine);
        SoundManager.Instance.PlaySE("main botan");
        if (DataManager.Instance.masteringData.masteringCharacterID == 0)
        {
            SoundManager.Instance.PlayVoice("tatsumi_2");
        }
        if (DataManager.Instance.masteringData.masteringCharacterID == 1)
        {
            SoundManager.Instance.PlayVoice("yusuke_2");
        }
        Fade.Instance.FadeIn(0.5f, () => { SceneChanger.LoadScene("ADV"); });

        //SceneChanger.LoadScene("ADV");
    }

    /// <summary>
    /// MiniGameボタンが押されたときに呼ばれる
    /// </summary>
    public void OnClickMiniGame()
    {
        SoundManager.Instance.PlaySE("main botan");
        if (DataManager.Instance.masteringData.masteringCharacterID == 0)
        {
            SoundManager.Instance.PlayVoice("tatsumi_3");
        }
        if (DataManager.Instance.masteringData.masteringCharacterID == 1)
        {
            SoundManager.Instance.PlayVoice("yusuke_3");
        }
        Fade.Instance.FadeIn(0.5f, () => { SceneChanger.LoadScene("MiniGameDifficultySelect", true); });

        //SceneChanger.LoadScene("MiniGame",true);
    }

    /// <summary>
    /// Galleryボタンが押されたときに呼ばれる
    /// </summary>
    public void OnClickGallery()
    {
        //SaveData.ResetMasteringData();
        //DataManager.Instance.Init();
        SoundManager.Instance.PlaySE("main botan");
        if (DataManager.Instance.masteringData.masteringCharacterID == 0)
        {
            SoundManager.Instance.PlayVoice("tatsumi_" + 5);
        }
        if (DataManager.Instance.masteringData.masteringCharacterID == 1)
        {
            SoundManager.Instance.PlayVoice("yusuke_" + 4);
        }
        Fade.Instance.FadeIn(0.5f, () => { SceneChanger.LoadScene("Menu"); });
        //SceneChanger.LoadScene("Menu");
	}

    /// <summary>
    /// Configボタンが押されたときに呼ばれる
    /// </summary>
    public void OnClickConfig()
    {
        Debug.Log("Click Config");
    }

    /// <summary>
    /// CommentAreaが押されたときに呼ばれる
    /// </summary>
    public void OnClickCommentArea()
    {
        var i = DataManager.Instance.masteringData.masteringCharacterID;

        if (i >= 0 && comments.Length >= i)
        {

            //ランダムにコメントを変える
            var line = Random.Range(0, comments[i].likeComment[likeID].comment.Length);
            //var line = Random.Range(0, comments.Length);

            //同じコメントを呼ばない
            if (comments[i].likeComment[likeID].comment.Length != 0 && comments[i].likeComment[likeID].comment.Length != 1)
            {
                while (line == commentPrevLine)
                {
                    line = Random.Range(0, comments[i].likeComment[likeID].comment.Length);
                }
                commentPrevLine = line;
            }

            //コメントを表示テキストに上書きする
            commentText.text = comments[i].likeComment[likeID].comment[line];
            SoundManager.Instance.PlayVoice(comments[i].likeComment[likeID].voiceName[line]);
        }
        //commentText.text = comments[line];
    }
}
