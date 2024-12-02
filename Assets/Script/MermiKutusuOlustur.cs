using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MermiKutusuOlustur : MonoBehaviour
{
    public List<GameObject> MermiKutusuPoint = new List<GameObject>();
    public GameObject MermiKutusuKendisi;

    public static bool MermiKutusuVarmi;
    public float KutuCikmaSuresi;
    int randomsayi;
    List<int> noktalar = new List<int>();
    void Start()
    {
        MermiKutusuVarmi = false;
        StartCoroutine(MermiKutusuYap());
    }

    
    IEnumerator MermiKutusuYap()
    {
        while(true)
        {
            yield return new WaitForSeconds(KutuCikmaSuresi);
            randomsayi = Random.Range(0, 5);
           
            if (!noktalar.Contains(randomsayi))
            {
                noktalar.Add(randomsayi);
            }
            else
            {
                randomsayi = Random.Range(0, 5);
                continue;
            }
           
            GameObject objem = Instantiate(MermiKutusuKendisi, MermiKutusuPoint[randomsayi].transform.position, MermiKutusuPoint[randomsayi].transform.rotation);
            objem.transform.gameObject.GetComponentInChildren<MermiKutusu>().Noktasi = randomsayi;
         
        }






        //while(true)
        //{
        //    yield return null;
        //    if (!MermiKutusuVarmi)
        //    {
        //        yield return new WaitForSeconds(KutuCikmaSuresi);
           
        //            int randomsayim = Random.Range(0, 4);
        //            Instantiate(MermiKutusuKendisi, MermiKutusuPoint[randomsayim].transform.position, MermiKutusuPoint[randomsayim].transform.rotation);
        //            MermiKutusuVarmi = true;
              
        //    }
         
        //}      
    }
    public void NoktalariKaldirma(int deger)
    {
        noktalar.Remove(deger);

    }
}
