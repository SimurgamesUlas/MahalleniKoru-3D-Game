using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombaKutusuOlustur : MonoBehaviour
{
    public List<GameObject> BombaKutusuPoint = new List<GameObject>();
    public GameObject BombaKutusuKendisi;

    public static bool BombaKutusuVarmi;
    public float KutuCikmaSuresi;
    int randomsayi;
   
    void Start()
    {
        BombaKutusuVarmi = false;
        StartCoroutine(BombaKutusuYap());
    }

    
    IEnumerator BombaKutusuYap()
    {
        while(true)
        {
            yield return new WaitForSeconds(KutuCikmaSuresi);
       
           
             if(!BombaKutusuVarmi)
            {
                randomsayi = Random.Range(0, 5);
                Instantiate(BombaKutusuKendisi, BombaKutusuPoint[randomsayi].transform.position, BombaKutusuPoint[randomsayi].transform.rotation);
                BombaKutusuVarmi = true;
            }           
          
         
        }
    }

}
