using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource SurroundSound;


    public AudioClip TP_Pickup1;
    public AudioClip GoldTP_Pickup1;
    public AudioClip VirusHit1;
    public AudioClip HandSanitizer1;
    public AudioClip MaskEquipped1;
    public AudioClip MaskTakesDamage1;
    public AudioClip LysolSpray1;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }



    public void OnItemCollidedWithPlayer(GameObject item)
    {
        switch (item.gameObject.name)
        {
            case ("Virus(Clone)"):
                {
                    SurroundSound.PlayOneShot(VirusHit1);
                    break;
                }
            case ("Toilet Paper(Clone)"):
                {
                    SurroundSound.PlayOneShot(TP_Pickup1);
                    //playing sound
                    break;
                }
            case ("Golden Toilet Paper(Clone)"):
                {
                    SurroundSound.PlayOneShot(GoldTP_Pickup1);
                    break;
                }
            case ("Sanitizer(Clone)"):
                {
                    SurroundSound.PlayOneShot(HandSanitizer1);
                    break;
                }
            case ("Mask(Clone)"):
                {
                    SurroundSound.PlayOneShot(MaskEquipped1);
                    break;
                }
            case ("Lysol(Clone)"):
                {
                    SurroundSound.PlayOneShot(LysolSpray1);
                    break;
                }
            default:
                break;
        }

    }


    public void OnVirusCollidedWithMask()
    {
        SurroundSound.PlayOneShot(MaskTakesDamage1);
    }


}
