using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MermiKutusu : MonoBehaviour
{
    string[] silahlar =
       {
            "Magnum",
            "Pompali",
            "Sniper",
            "Taramali"
        };

    int[] mermiSayisi =
    {
           10,
           20,
           5,
           30,
    };

    public List<Sprite> SilahResimleri = new List<Sprite>();
    public Image SilahinResmi;



    public string OlusanSilahinTuru;
    public int OlusanMermiSayisi;
    public int Noktasi;


    void Start()
    {

        int gelenAnahtar = Random.Range(0, silahlar.Length);

        OlusanSilahinTuru = silahlar[gelenAnahtar];
        OlusanMermiSayisi = mermiSayisi[Random.Range(0, mermiSayisi.Length)];
        SilahinResmi.sprite = SilahResimleri[gelenAnahtar];
        /*OlusanSilahinTuru = "Taramali";
        OlusanMermiSayisi = 10;*/
    }
}
