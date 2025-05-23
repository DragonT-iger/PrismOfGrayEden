//using UnityEngine;
//using UnityEngine.UI;

//public class TitleLoopImages : MonoBehaviour
//{
//    public Sprite[] arrTitleLoop;
//    private Image image;
//    private int spriteCount = 0;
//    private float time = 0;
//    public float checkTime = 0f;
//    public bool isClicked = false;
//    void Start()
//    {
//        image = GetComponent<Image>();
//    }
     
//    void Update()
//    {
//        if (Input.anyKeyDown) isClicked = true;

//        if (isClicked)
//        {
//            if (time < checkTime)
//            {
//                time += Time.deltaTime;
//            }
//            else
//            {
                
//                spriteCount = spriteCount % (arrTitleLoop.Length);
//                image.sprite = arrTitleLoop[spriteCount++];
//                time -= checkTime;
                
//            }
//        }
//    }

//}
