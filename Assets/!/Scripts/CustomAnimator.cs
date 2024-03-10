using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CustomAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    const float FRAME_BUFFER = 0.4f;
    public static CustomAnimator Instance { get; private set; }

    SpriteRenderer marioSpriteRenderer;
    Dictionary<int, SpriteRenderer> goombasSpriteRenderers = new();
    public List<Sprite> marioSprites = new();
    public List<Sprite> goombaSprites = new();
    public List<Sprite> koopaSprites = new();
    public Dictionary<string, Sprite> marioSpritesMap = new();
    public Dictionary<string, Sprite> goombaSpritesMap = new();
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        foreach (Sprite sprite in marioSprites)
        {
            marioSpritesMap.Add(sprite.name, sprite);
        }

        goombaSpritesMap = new Dictionary<string, Sprite>
        {
            { "Alive", goombaSprites[0] },
            { "Dead", goombaSprites[1] },
        };

        marioSpriteRenderer = GameObject.FindGameObjectWithTag("Mario").GetComponent<SpriteRenderer>();

        var goombas = GameObject.FindGameObjectsWithTag("Goomba");
        foreach (GameObject goomba in goombas)
        {
            goombasSpriteRenderers.Add(goomba.GetInstanceID(), goomba.GetComponent<SpriteRenderer>());
        }
    }

    private string GetSmallOrBig(bool isBig)
    {
        return isBig ? "Small" : "Big";
    }
    public bool IsMarioDead()
    {
        return marioSpriteRenderer.sprite.name == "Mario_Small_Death";
    }


    public void AnimateMarioJump()
    {
        marioSpriteRenderer.sprite = marioSpritesMap["Mario_Small_Jump"];
    }
    public void AnimateMarioJump(bool isBig)
    {
        string smallOrBig = GetSmallOrBig(isBig);
        marioSpriteRenderer.sprite = marioSpritesMap[$"Mario_{smallOrBig}_Jump"];
    }

    public void AnimateMarioIdle()
    {
        marioSpriteRenderer.sprite = marioSpritesMap["Mario_Small_Idle"];
    }

    public void AnimateMarioIdle(bool isBig)
    {
        string smallOrBig = GetSmallOrBig(isBig);
        marioSpriteRenderer.sprite = marioSpritesMap[$"Mario_{smallOrBig}_Idle"];
    }

    public void AnimateMarioDead()
    {
        marioSpriteRenderer.sprite = marioSpritesMap["Mario_Small_Death"];
    }

    public IEnumerator AnimateMarioRunning()
    {
        int i = 1;
        while (true)
        {
            marioSpriteRenderer.sprite = marioSpritesMap["Mario_Small_Run" + i.ToString()];
            yield return new WaitForSeconds(0.2f);
            i = (i % 3) + 1;
        }
    }

    public IEnumerator AnimateMarioRunning(bool isBig)
    {
        int i = 1;
        string smallOrBig = GetSmallOrBig(isBig);
        while (true)
        {
            marioSpriteRenderer.sprite = marioSpritesMap[$"Mario_{smallOrBig}_Run" + i.ToString()];
            yield return new WaitForSeconds(FRAME_BUFFER);
            i = (i % 3) + 1;
        }
    }

    public void AnimateKillGoomba(GameObject goomba)
    {
        var id = goomba.GetInstanceID();
        SpriteRenderer renderer = goombasSpriteRenderers[id];
        renderer.sprite = goombaSpritesMap["Dead"];
    }
}

