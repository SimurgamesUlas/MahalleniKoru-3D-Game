using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKutusuOlustur : MonoBehaviour
{
    public List<GameObject> HealthKutusuPoint = new List<GameObject>();
    public GameObject HealthKutusuKendisi;

    public static bool HealthKutusuVarmi;
    public float KutuCikmaSuresi;
    int randomsayi;
   
    void Start()
    {
        HealthKutusuVarmi = false;
        StartCoroutine(HealthKutusuYap());
    }

    
    IEnumerator HealthKutusuYap()
    {
        while(true)
        {
            yield return new WaitForSeconds(KutuCikmaSuresi);
       
           
                if(!HealthKutusuVarmi)
            {
                randomsayi = Random.Range(0, 8);
                Instantiate(HealthKutusuKendisi, HealthKutusuPoint[randomsayi].transform.position, HealthKutusuPoint[randomsayi].transform.rotation);
                HealthKutusuVarmi = true;
            }           
          
         
        }
    }

}
