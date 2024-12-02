
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pompali : MonoBehaviour
{

    Animator animatorum;
    [Header("Ayarlar")]
    public bool atesEdebilirmi;
    float İceridenAtesEtmeSikligi;
    public float disaridanAtesEtmeSikligi;
    public float menzil;


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
    float YaklasmaPov = 40;


    [Header("Silah Ayarları")]
    public int ToplamMermiSayisi;
    public int SarjorKapasitesi;
    public int KalanMermi;
    public string SilahinAdi;
    public TextMeshProUGUI ToplamMermi_Text;
    public TextMeshProUGUI KalanMermi_Text;
    public bool KovanCiksinmi;
    public bool cameraTitresinMi;
    public GameObject KovanCikisNoktasi;
    public GameObject KovanObjesi;
    public MermiKutusuOlustur MermiKutusuOlusturYonetim;
    public float DarbeGucu;

    public GameObject MermiCikisNoktasi1;
    public GameObject MermiCikisNoktasi2;
    public GameObject Mermi;

    bool SarjorDegisiyorMu = false;

    bool zoomVarmi;
    void Start()
    {

        ToplamMermiSayisi = PlayerPrefs.GetInt(SilahinAdi + "ToplamMermi");
        KalanMermi = PlayerPrefs.GetInt(SilahinAdi + "KalanMermi");
        KovanCiksinmi = false;
        BaslangicMermiDoldur();
        SarjordoldurmaTeknikFonksiyon("NormalYaz");
        animatorum = GetComponent<Animator>();
        camFieldPov = benimCam.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Mouse1))
        {

            if (atesEdebilirmi && Time.time > İceridenAtesEtmeSikligi && KalanMermi != 0 && KalanMermi >= 0 && !SarjorDegisiyorMu)
            {
                if (!GameKontrolcu.OyunDurdumu)
                {
                    AtesEt(false);
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
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            zoomVarmi = true;
            KameraYaklastirVeZoomYap(true);

        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            zoomVarmi = false;
            KameraYaklastirVeZoomYap(false);
            benimCam.fieldOfView = camFieldPov;
        }
        if (zoomVarmi)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {

                if (atesEdebilirmi && Time.time > İceridenAtesEtmeSikligi && KalanMermi != 0 && KalanMermi >= 0)
                {
                    AtesEt(true);
                    İceridenAtesEtmeSikligi = Time.time + disaridanAtesEtmeSikligi;
                }
                if (KalanMermi == 0)
                {
                    MermiBittiSes.Play();
                }



            }
        }
    }
    void KameraYaklastirVeZoomYap(bool durum)
    {
        if (durum)
        {


            animatorum.SetBool("ZoomYap", durum);
            benimCam.fieldOfView = YaklasmaPov;

        }
        else
        {


            animatorum.SetBool("ZoomYap", durum);
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

        yield return new WaitForSeconds(2.3f);
       
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
    void AtesEt(bool yakinlasmaVarmi)
    {
        AtesEtmeTeknikİslemler(yakinlasmaVarmi);

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
    IEnumerator CameraTitre(float titremeSuresi, float magnitude)
    {
        Vector3 orjinalPozisyon = benimCam.transform.localPosition;

        if (cameraTitresinMi)
        {
            float gecensure = 0.0f;
            while (gecensure < titremeSuresi)
            {
                float x = Random.Range(-1f, 1) * magnitude;

                benimCam.transform.localPosition = new Vector3(x, orjinalPozisyon.y, orjinalPozisyon.x);
                gecensure += Time.deltaTime;
                yield return null;
            }
            benimCam.transform.localPosition = orjinalPozisyon;
        }
    }
    void AtesEtmeTeknikİslemler(bool yakinlasmaVarmi)
    {
        if (KovanCiksinmi)
        {
            GameObject obje = Instantiate(KovanObjesi, KovanCikisNoktasi.transform.position, KovanCikisNoktasi.transform.rotation);
            Rigidbody rb = obje.GetComponent<Rigidbody>();
            rb.AddRelativeForce(new Vector3(-10f, 1, 0) * 60);
        }
        Instantiate(Mermi, MermiCikisNoktasi1.transform.position, MermiCikisNoktasi1.transform.rotation);
        Instantiate(Mermi, MermiCikisNoktasi2.transform.position, MermiCikisNoktasi2.transform.rotation);
        StartCoroutine(CameraTitre(.10f, .2f));
        AtesSesi.Play();
        AtesEfekt.Play();

        if (!yakinlasmaVarmi)
        {
            animatorum.Play("AtesEt");

        }
        else
        {
            animatorum.Play("ZoomAtes");
        }

        KalanMermi--;
        PlayerPrefs.SetInt(SilahinAdi + "KalanMermi", KalanMermi);
        KalanMermi_Text.text = KalanMermi.ToString();
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

    void BaslangicMermiDoldur()
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
                ToplamMermiSayisi += MermiSayisi;
                PlayerPrefs.SetInt(SilahinAdi + "ToplamMermi", ToplamMermiSayisi);
                PlayerPrefs.SetInt(SilahinAdi + "KalanMermi", KalanMermi);
                SarjordoldurmaTeknikFonksiyon("NormalYaz");
                break;
            case "Magnum":
                PlayerPrefs.SetInt("MagnumMermi", PlayerPrefs.GetInt("MagnumMermi") + MermiSayisi);
                break;
            case "Sniper":
                PlayerPrefs.SetInt("SniperMermi", PlayerPrefs.GetInt("SniperMermi") + MermiSayisi);
                break;
        }
    }
}

