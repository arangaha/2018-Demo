using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class bodypart : MonoBehaviour {
    public Sprite front;
    public Sprite back;
    public string partback;
    public string partfront;
    protected bool flipped = false;
    protected string currentEquip = "_borak heavy";
    GameObject EffectOverlay;
    GameObject EffectOverlayInstance;
    public bool reloaded = false;
    // Use this for initialization
    protected virtual void Awake()
    {
        EffectOverlay = Resources.Load("FX Overlays/EffectOverlay") as GameObject;
        //GetComponent<SpriteRenderer>().sprite = front;
        //GetComponent<SpriteMask>().sprite = front;

    }
	// Update is called once per frame
	void Update () {
        if(!reloaded)
        {
            reloadSprite();
            reloaded = true;
        }
        if(GetComponent<SpriteRenderer>().sprite != null) 
        {
            reloadSprite();
            GetComponent<SpriteRenderer>().sprite = front;
        }
        if(GetComponent<SpriteMask>().sprite != null)
        {
            reloadSprite();
            GetComponent<SpriteMask>().sprite = front;
        }
	}
    public void flip()
    {
        if (flipped)
        {
            flipped = false;
            GetComponent<SpriteRenderer>().sprite = front;
            GetComponent<SpriteMask>().sprite = front;
        }
        else
        {
            flipped = true;
            GetComponent<SpriteRenderer>().sprite = back;
            GetComponent<SpriteMask>().sprite = back;

        }

    }
    public void reset()
    {
        flipped = false;
        GetComponent<SpriteRenderer>().sprite = front;
        GetComponent<SpriteMask>().sprite = front;
    }
    public void changeEquip(string e)
    {
        front = Resources.Load<Sprite>(e + "/" + partfront);
        back = Resources.Load<Sprite>(e + "/" + partback);
    }

    public void applyOverlay(Sprite sprite)
    {
        
        bool duplicate = false;
        List<GameObject> effectOverlayList = getCurrentOverlays();
        for (int i = 0; i < effectOverlayList.Count; i++)
        {
            if (effectOverlayList[i].GetComponent<SpriteRenderer>().sprite.Equals(sprite))
            {
                duplicate = true;
            }
        }
        if (!duplicate)
        {
            float x;
            float y;
            EffectOverlayInstance = Instantiate(EffectOverlay, transform.position, transform.rotation);
            EffectOverlayInstance.transform.parent = gameObject.transform;
            EffectOverlayInstance.gameObject.layer = gameObject.layer;
            EffectOverlayInstance.GetComponent<EffectOverlayScript>().changeSprite(sprite);
            x = transform.GetComponent<SpriteRenderer>().sprite.rect.xMax/ EffectOverlayInstance.GetComponent<SpriteRenderer>().sprite.rect.xMax;
            y = transform.GetComponent<SpriteRenderer>().sprite.rect.yMax / EffectOverlayInstance.GetComponent<SpriteRenderer>().sprite.rect.yMax;
            //GetComponent<Transform>().localEulerAngles = transform.GetComponent<Transform>().localEulerAngles;
            EffectOverlayInstance.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder+1;
            EffectOverlayInstance.GetComponent<Transform>().localScale = new Vector3(x, y, 1);

        }
    }
    public void removeOverlay(Sprite sprite)
    {
        List<GameObject> effectOverlayList = getCurrentOverlays();
        for(int i = 0;i< effectOverlayList.Count;i++)
        {
            if(effectOverlayList[i].GetComponent<SpriteRenderer>().sprite.Equals(sprite))
            {
                if(sprite.Equals("freeze_overlay")|| sprite.Equals("chill_overlay"))
                {
                    GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/Default");
                }
                Destroy(effectOverlayList[i]);
                i = effectOverlayList.Count;
            }
        }
    }
    public List<GameObject> getCurrentOverlays()
    {
        List<GameObject> effectOverlayList = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.tag == "EffectOverlay")
            {
                effectOverlayList.Add(child.gameObject);
            }
        }
        return effectOverlayList;
    }
    public void AddFlames()
    {
        List<GameObject> effectOverlayList = getCurrentOverlays();
        for (int i = 0; i < effectOverlayList.Count; i++)
        {
            if (effectOverlayList[i].GetComponent<SpriteRenderer>().sprite.Equals(Resources.Load<Sprite>("FX Overlays/ignite_overlay")))
            {
                effectOverlayList[i].GetComponent<EffectOverlayScript>().addFlames(Mathf.RoundToInt(transform.GetComponent<SpriteRenderer>().sprite.rect.xMax), Mathf.RoundToInt(transform.GetComponent<SpriteRenderer>().sprite.rect.yMax));
            }
        }
    }
    public void setEquip(string equip)
    {
        currentEquip = equip;
        //reloadSprite();

       // front = LoadSprite("body_parts/" + currentEquip + "/" + partfront);
        //back = LoadSprite("body_parts/" + currentEquip + "/" + partback);

        //back = Resources.Load("body_parts/" + currentEquip + "/" + partback, typeof(Sprite)) as Sprite;
        //front = Resources.Load("body_parts/" + currentEquip + "/" + partfront) as Sprite;
        //back = Resources.Load("body_parts/" + currentEquip + "/" + partback)as Sprite;
    }
    public void reloadSprite()
    {
        front = Resources.Load<Sprite>("body_parts/" + currentEquip + "/" + partfront);
        back = Resources.Load<Sprite>("body_parts/" + currentEquip + "/" + partback);
    }
    public Sprite LoadSprite (string s)
    {
        Texture2D img = Resources.Load<Texture2D>(s);
        Sprite imgSprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height), Vector2.zero);
        Debug.Log(imgSprite);
        return imgSprite;
    }

    public void ChangeOpacity (float opacity)
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, opacity);
    }
    public void chill()
    {
        GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/Chill");
    }
}
