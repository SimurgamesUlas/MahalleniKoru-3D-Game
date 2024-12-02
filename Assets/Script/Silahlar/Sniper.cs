using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Sniper : MonoBehaviour
{

    Animator animatorum;
    [Header("Ayarlar")]
    public bool atesEdebilirmi;
    float İceridenAtesEtmeSikligi;
    public float disaridanAtesEtmeSikligi;
    public float menzil;
    public GameObject Cross;
    public GameObject Scope;

    [Header("Sesler")]

    public AudioSource AtesSesi;
    public AudioSource SarjorDegistirme;
    public AudioSource MermiBittiSes;
    public AudioSource MermiAlmaSesi;

    [Header("Efektler")]
    public ParticleSystem AtesEfekt;
    public ParticleSystem Mermiİzi;
    public ParticleSystem KanEfekti;

    [Header("Digerleri")]
    public Camera benimCam;
    float camFieldPov;
    float YaklasmaPov = 20;

    [Header("Silah Ayarları")]
    public int ToplamMermiSayisi;
    public int SarjorKapasitesi;
    public int KalanMermi;
    public string SilahinAdi;
    public TextMeshProUGUI ToplamMermi_Text;
    public TextMeshProUGUI KalanMermi_Text;
    public float DarbeGucu;

    public bool KovanCiksinmi;
    public GameObject KovanCikisNoktasi;
    public GameObject KovanObjesi;

    public MermiKutusuOlustur MermiKutusuOlusturYonetim;

    bool SarjorDegisiyorMu = false;
    void Start()
    {

        ToplamMermiSayisi = PlayerPrefs.GetInt(SilahinAdi + "ToplamMermi");
        KalanMermi = PlayerPrefs.GetInt(SilahinAdi + "KalanMermi");
        KovanCiksinmi = true;
        BaslangicMermiDoldur();
        SarjordoldurmaTeknikFonksiyon("NormalYaz");
        animatorum = GetComponent<Animator>();
        camFieldPov = benimCam.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Mouse0))
        {

            if (atesEdebilirmi && Time.time > İceridenAtesEtmeSikligi && KalanMermi != 0 && KalanMermi >= 0 && !SarjorDegisiyorMu)
            {
                if (!GameKontrolcu.OyunDurdumu)
                {
                    AtesEt();
                    İceridenAtesEtmeSikligi = Time.time + disaridanAtesEtmeSikligi;
                }
            }
            if (KalanMermi == 0)
            {
                MermiBittiSes.Play();
            }



        }
        if (Input.GetKey(KeyCode.R))
        {
            StartCoroutine(ReloadYap());

        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            MermiAl();
        }
        if(Input.GetKeyDown(KeyCode.Mouse1)) {
            KameraYaklastirVeZoomYap(true);
           
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            KameraYaklastirVeZoomYap(false);
            benimCam.fieldOfView = camFieldPov;
        }

    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Mermi"))
        {

            MermiKaydet(other.transform.gameObject.GetComponent<MermiKutusu>().OlusanSilahinTuru, other.transform.gameObject.GetComponent<MermiKutusu>().OlusanMermiSayisi);
            // MermiKutusuOlustur.MermiKutusuVarmi = false;

            MermiKutusuOlusturYonetim.NoktalariKaldirma(other.transform.gameObject.GetComponent<MermiKutusu>().Noktasi);
            Destroy(other.transform.parent.gameObject);
        }
        if (other.gameObject.CompareTag("CanKutusu"))
        {

            MermiKutusuOlusturYonetim.GetComponent<GameKontrolcu>().CanAl();
            HealthKutusuOlustur.HealthKutusuVarmi = false;
            Destroy(other.transform.gameObject);
        }
        if (other.gameObject.CompareTag("BombaKutusu"))
        {

            MermiKutusuOlusturYonetim.GetComponent<GameKontrolcu>().BombaAl();
            BombaKutusuOlustur.BombaKutusuVarmi = false;
            Destroy(other.transform.gameObject);
        }
    }
    IEnumerator ReloadYap()
    {
        if (KalanMermi < SarjorKapasitesi && ToplamMermiSayisi != 0)
        {
            
            animatorum.Play("SarjorDegistir");
            SarjorDegisiyorMu = true;
        }

        yield return new WaitForSeconds(2f);
       
        if (KalanMermi < SarjorKapasitesi && ToplamMermiSayisi != 0)
        {
            if (SarjorKapasitesi - KalanMermi <= ToplamMermiSayisi)
            {
              
                SarjordoldurmaTeknikFonksiyon("MermiVar");
                SarjorDegisiyorMu = false;
            }
            else
            {
                SarjorDegisiyorMu = false;
                SarjordoldurmaTeknikFonksiyon("MermiYok");
                SarjorDegisiyorMu = false;
            }

        }
    }
    void AtesEt()
    {
        if (KovanCiksinmi)
        {
            GameObject obje = Instantiate(KovanObjesi, KovanCikisNoktasi.transform.position, KovanCikisNoktasi.transform.rotation);
            Rigidbody rb = obje.GetComponent<Rigidbody>();
            rb.AddRelativeForce(new Vector3(-10f, 1, 0) * 60);
        }

        AtesSesi.Play();
        AtesEfekt.Play();
        animatorum.Play("AtesEt");
        KalanMermi--;
        PlayerPrefs.SetInt(SilahinAdi + "KalanMermi", KalanMermi);
        KalanMermi_Text.text = KalanMermi.ToString();

        RaycastHit hit;

        if (Physics.Raycast(benimCam.transform.position, benimCam.transform.forward, out hit, menzil))
        {

            if (hit.transform.gameObject.CompareTag("Dusman"))
            {
                Instantiate(KanEfekti, hit.point, Quaternion.LookRotation(hit.normal));

                hit.transform.gameObject.GetComponent<Dusman>().DarbeAl(DarbeGucu);
            }
            else if (hit.transform.gameObject.CompareTag("DevrilebilirObje"))
            {
                Rigidbody rg = hit.transform.gameObject.GetComponent<Rigidbody>();
                //rg.AddForce(new Vector3(4f, 2f, 3f) * 5f);
                //rg.AddForce(transform.forward * 50f);
                rg.AddForce(-hit.normal * 50f);
                Instantiate(Mermiİzi, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                Instantiate(Mermiİzi, hit.point, Quaternion.LookRotation(hit.normal));

            }


            AtesSesi.Play();
            AtesEfekt.Play();

        }
    }

    public void SarjordoldurmaTeknikFonksiyon(string tur)
    {
        switch (tur)
        {
            case "MermiVar":
                ToplamMermiSayisi -= SarjorKapasitesi - KalanMermi;
                KalanMermi += SarjorKapasitesi - KalanMermi;

                PlayerPrefs.SetInt(SilahinAdi + "ToplamMermi", ToplamMermiSayisi);
                PlayerPrefs.SetInt(SilahinAdi + "KalanMermi", KalanMermi);
                ToplamMermi_Text.text = ToplamMermiSayisi.ToString();
                KalanMermi_Text.text = KalanMermi.ToString();
                break;

            case "MermiYok":
                KalanMermi += ToplamMermiSayisi;
                ToplamMermiSayisi -= ToplamMermiSayisi;


                PlayerPrefs.SetInt(SilahinAdi + "ToplamMermi", ToplamMermiSayisi);
                PlayerPrefs.SetInt(SilahinAdi + "KalanMermi", KalanMermi);
                ToplamMermi_Text.text = ToplamMermiSayisi.ToString();
                KalanMermi_Text.text = KalanMermi.ToString();
                break;

            case "NormalYaz":
                ToplamMermi_Text.text = ToplamMermiSayisi.ToString();
                KalanMermi_Text.text = KalanMermi.ToString();
                break;
        }
    }
    void SarjorDegistir()
    {
        //int ToplamMermiİnt = Int32.Parse(ToplamMermi_Text.text);
        //int KalanMermiİnt = Int32.Parse(KalanMermi_Text.text);

        //int ReloadMermi = 30 - KalanMermiİnt;

        //if (ToplamMermiİnt - ReloadMermi >= 0)
        //{
        //    ToplamMermiİnt -= ReloadMermi;
        //    KalanMermiİnt += ReloadMermi;

        //}
        //else
        //{
        //    KalanMermiİnt += ToplamMermiİnt;
        //    ToplamMermiİnt -= ToplamMermiİnt;

        //}
        //KalanMermi = KalanMermiİnt;
        //ToplamMermiSayisi = ToplamMermiİnt;
        //KalanMermi_Text.text = KalanMermi.ToString();
        //ToplamMermi_Text.text = ToplamMermiSayisi.ToString();

        SarjorDegistirme.Play();
    }

    void MermiAl()
    {
        RaycastHit hit;

        if (Physics.Raycast(benimCam.transform.position, benimCam.transform.forward, out hit, 4))
        {
            if (hit.transform.gameObject.CompareTag("Mermi"))
            {

                MermiKaydet(hit.transform.gameObject.GetComponent<MermiKutusu>().OlusanSilahinTuru, hit.transform.gameObject.GetComponent<MermiKutusu>().OlusanMermiSayisi);
                //MermiKutusuOlustur.MermiKutusuVarmi = false;
                MermiKutusuOlusturYonetim.NoktalariKaldirma(hit.transform.gameObject.GetComponent<MermiKutusu>().Noktasi);
                Destroy(hit.transform.parent.gameObject);

            }
        }
    }

    public void BaslangicMermiDoldur()
    {
        if (PlayerPrefs.GetInt(SilahinAdi + "KalanMermi") > SarjorKapasitesi)
        {
            int kalan = KalanMermi - SarjorKapasitesi;
            ToplamMermiSayisi += kalan;
            KalanMermi  = SarjorKapasitesi;

            PlayerPrefs.SetInt(SilahinAdi + "ToplamMermi", ToplamMermiSayisi);
            PlayerPrefs.SetInt(SilahinAdi + "KalanMermi", KalanMermi);
            ToplamMermi_Text.text = ToplamMermiSayisi.ToString();
            KalanMermi_Text.text = KalanMermi.ToString();
        }
        if(SarjorKapasitesi > (KalanMermi + ToplamMermiSayisi))
        {
            KalanMermi += ToplamMermiSayisi;
            ToplamMermiSayisi -= ToplamMermiSayisi;
            PlayerPrefs.SetInt(SilahinAdi + "ToplamMermi", ToplamMermiSayisi);
            PlayerPrefs.SetInt(SilahinAdi + "KalanMermi", KalanMermi);
            ToplamMermi_Text.text = ToplamMermiSayisi.ToString();
            KalanMermi_Text.text = KalanMermi.ToString();
        }
        //if(SarjorKapasitesi > KalanMermi)
        //{
        //    ToplamMermiSayisi -= SarjorKapasitesi - KalanMermi;
        //    KalanMermi += SarjorKapasitesi - KalanMermi;

        //    PlayerPrefs.SetInt(SilahinAdi + "ToplamMermi", ToplamMermiSayisi);
        //    PlayerPrefs.SetInt(SilahinAdi + "KalanMermi", KalanMermi);
        //    ToplamMermi_Text.text = ToplamMermiSayisi.ToString();
        //    KalanMermi_Text.text = KalanMermi.ToString();
        //}

    }
    void MermiKaydet(string silahTuru, int MermiSayisi)
    {
        MermiAlmaSesi.Play();
        switch (silahTuru)
        {
            case "Taramali":
                PlayerPrefs.SetInt(SilahinAdi + "ToplamMermi", PlayerPrefs.GetInt(SilahinAdi + "ToplamMermi") + MermiSayisi);
                PlayerPrefs.SetInt(SilahinAdi + "KalanMermi", PlayerPrefs.GetInt(SilahinAdi + "KalanMermi"));
                break;
            case "Pompali":

                PlayerPrefs.SetInt(SilahinAdi + "ToplamMermi", PlayerPrefs.GetInt(SilahinAdi + "ToplamMermi") + MermiSayisi);
                PlayerPrefs.SetInt(SilahinAdi + "KalanMermi", PlayerPrefs.GetInt(SilahinAdi + "KalanMermi"));      
                break;
            case "Magnum":
                PlayerPrefs.SetInt("MagnumMermi", PlayerPrefs.GetInt("MagnumMermi") + MermiSayisi);
                break;
            case "Sniper":
                ToplamMermiSayisi += MermiSayisi;
                    PlayerPrefs.SetInt(SilahinAdi + "ToplamMermi", ToplamMermiSayisi);
                    PlayerPrefs.SetInt(SilahinAdi + "KalanMermi", KalanMermi);
                    SarjordoldurmaTeknikFonksiyon("NormalYaz");
                break;
        }
    }
    void KameraYaklastirVeZoomYap(bool durum)
    {
        if (durum)
        {
            Cross.SetActive(false);
            benimCam.cullingMask = ~(1 << 6);
            animatorum.SetBool("ZoomYap", durum);
            benimCam.fieldOfView = YaklasmaPov;
            Scope.SetActive(true);
        }
        else
        {
            Scope.SetActive(false);
            benimCam.cullingMask = -1;
            animatorum.SetBool("ZoomYap", durum);
            benimCam.fieldOfView = camFieldPov;
            Cross.SetActive(true);
        }
        
        
    }
}

