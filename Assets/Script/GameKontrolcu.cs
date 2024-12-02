
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;


public class GameKontrolcu : MonoBehaviour
{

    [Header("Sağlık Ayarları")]
    float health = 100;
    public UnityEngine.UI.Image HealthBar;

    [Header("Silah Ayarları")]
    public GameObject[] silahlar;
    public AudioSource degisimSes;
    int AktifSira;
    int Sira;
    private keles kelesScript;
    private Sniper sniperScript;
    private Pompali pompaliScript;
    private Magnum magnumScript;
    public GameObject Bomba;
    public GameObject BombaPoint;
    public Camera BenimCam;


    [Header("Dusman Ayarları")]
    public GameObject[] dusmanlar;
    public GameObject[] cikisNoktalari;
    public GameObject[] hedefNoktalar;
    public int DusmanCikmaSuresi;
    public TextMeshProUGUI KalanDusmanText;
    [Header("Diğer Ayarlar")]
    public GameObject GameOverCanvas;
    public GameObject KazandinCanvas;
    public GameObject PauseCanvas;
    public int BaslangicDusmanSayisi;
    public static int KalanDusmanSayisi;
    public AudioSource OyunİciSeS;

    public TextMeshProUGUI SaglikSayisiText;
    public TextMeshProUGUI BombaSayisiText;
    public AudioSource Yok;
    public static bool OyunDurdumu;
   
     float bombaAtmaSuresi = 1f;
     float sonrakiBombaZamani = 0f;
    void Start()
    {



        AktifSira = 0;
       
        if (!PlayerPrefs.HasKey("OyunBasladimi"))
        {
            PlayerPrefs.SetInt("TaramaliToplamMermi", 50);
            PlayerPrefs.SetInt("TaramaliKalanMermi", 30);
            PlayerPrefs.SetInt("PompaliToplamMermi", 50);
            PlayerPrefs.SetInt("PompaliKalanMermi", 2);
            PlayerPrefs.SetInt("SniperToplamMermi", 50);
            PlayerPrefs.SetInt("SniperKalanMermi", 5);
            PlayerPrefs.SetInt("MagnumToplamMermi", 50);
            PlayerPrefs.SetInt("MagnumKalanMermi", 6);

            PlayerPrefs.SetInt("SaglikSayisi", 1);
            PlayerPrefs.SetInt("BombaSayisi", 5);
            PlayerPrefs.SetInt("OyunBasladimi", 1);
        }


        KalanDusmanSayisi = BaslangicDusmanSayisi;
        KalanDusmanText.text = BaslangicDusmanSayisi.ToString();
        SaglikSayisiText.text = PlayerPrefs.GetInt("SaglikSayisi").ToString();
        BombaSayisiText.text = PlayerPrefs.GetInt("BombaSayisi").ToString();
        AktifSira = 0;

        UpdateAmmoText();
        StartCoroutine(DusmanCikar());
        OyunİciSeS =  GetComponent<AudioSource>();
        OyunİciSeS.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && Sira != 1 && !OyunDurdumu)
        {
            Sira = 1;
            SilahDegistir(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && Sira != 2 && !OyunDurdumu)
        {
            Sira = 2;
            SilahDegistir(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && Sira != 3 && !OyunDurdumu)
        {
            Sira = 3;
            SilahDegistir(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && Sira != 4 && !OyunDurdumu) 
        {
            Sira = 4;
            SilahDegistir(3);
        }
        if (Input.GetKeyDown(KeyCode.Q) && !OyunDurdumu)
        {
            QtusuVersiyonu();
        }
        if (Input.GetKeyDown(KeyCode.G) && Time.time >= sonrakiBombaZamani && !OyunDurdumu)
        {
            
            BombaAt();
            sonrakiBombaZamani = Time.time + bombaAtmaSuresi;

        }
        if (Input.GetKeyDown(KeyCode.E) && health !=100 && !OyunDurdumu)
        {
            CanArttirma();

        }
        if (Input.GetKeyDown(KeyCode.Escape) && !OyunDurdumu)
        {
            Pause();

        }
    }

    void SilahDegistir(int siraNumarasi)
    {
        degisimSes.Play();
        foreach (GameObject silah in silahlar)
        {
            silah.SetActive(false);
        }
        AktifSira = siraNumarasi;
        silahlar[siraNumarasi].SetActive(true);

        kelesScript = silahlar[siraNumarasi].GetComponent<keles>();
        sniperScript = silahlar[siraNumarasi].GetComponent<Sniper>();
        pompaliScript = silahlar[siraNumarasi].GetComponent<Pompali>();
        magnumScript = silahlar[siraNumarasi].GetComponent<Magnum>();
        if (kelesScript != null)
        {
            kelesScript.SarjordoldurmaTeknikFonksiyon("NormalYaz");
        }
        if (sniperScript != null)
        {
            sniperScript.SarjordoldurmaTeknikFonksiyon("NormalYaz");
        }
        if (pompaliScript != null)
        {
            pompaliScript.SarjordoldurmaTeknikFonksiyon("NormalYaz");
        }
        if (magnumScript != null)
        {
            magnumScript.SarjordoldurmaTeknikFonksiyon("NormalYaz");
        }
        UpdateAmmoText();
    }
    IEnumerator DusmanCikar()
    {

        while (true)
        {
            yield return new WaitForSeconds(DusmanCikmaSuresi);

            if (BaslangicDusmanSayisi != 0)
            {
                int dusman = Random.Range(0, 5);
                int cikisNoktasi = Random.Range(0, 2);
                int hedefNoktasi = Random.Range(0, 2);


                // Instantiate işlemi
                GameObject obje = Instantiate(dusmanlar[dusman], cikisNoktalari[cikisNoktasi].transform.position, Quaternion.identity);

                // Hedef belirleme
                obje.GetComponent<Dusman>().HedefBelirle(hedefNoktalar[hedefNoktasi]);
                BaslangicDusmanSayisi--;

            }



        }
    }

    public void DusmanSayisiGuncelle()
    {
        KalanDusmanSayisi--;
        if (KalanDusmanSayisi <= 0)
        {
            PauseCanvas.SetActive(false);
            KazandinCanvas.SetActive(true);
            OyunDurdumu = true;


            Cursor.visible = true;
            GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = false;
            Cursor.lockState = CursorLockMode.None;


            KalanDusmanText.text = "0";
            Time.timeScale = 0;
        }
        else
        {
            KalanDusmanText.text = KalanDusmanSayisi.ToString();
        }
       
    }
    public void DarbeAl(float DarbeGucu)
    {
        health -= DarbeGucu;
        HealthBar.fillAmount = health / 100;
        if (health <= 0)
        {
            GameOver();
        }
    }

    public void CanArttirma()
    {
        if(PlayerPrefs.GetInt("SaglikSayisi")  != 0)
        {
            health = 100;
            HealthBar.fillAmount = health / 100;
            PlayerPrefs.SetInt("SaglikSayisi", PlayerPrefs.GetInt("SaglikSayisi") - 1);
            SaglikSayisiText.text = PlayerPrefs.GetInt("SaglikSayisi").ToString();
        }
        else
        {
            Yok.Play();
        }


    }
    public void CanAl()
    {
       
            PlayerPrefs.SetInt("SaglikSayisi", PlayerPrefs.GetInt("SaglikSayisi") + 1);
             SaglikSayisiText.text = PlayerPrefs.GetInt("SaglikSayisi").ToString();

    }
    public void BombaAt()
    {

        if (PlayerPrefs.GetInt("BombaSayisi") != 0)
        {
            GameObject obje = Instantiate(Bomba, BombaPoint.transform.position, BombaPoint.transform.rotation);
            Rigidbody rg = obje.GetComponent<Rigidbody>();
            Vector3 acimiz = Quaternion.AngleAxis(90, BenimCam.transform.forward) * BenimCam.transform.forward;
            rg.AddForce(acimiz * 250f);

            PlayerPrefs.SetInt("BombaSayisi", PlayerPrefs.GetInt("BombaSayisi") - 1);
            BombaSayisiText.text = PlayerPrefs.GetInt("BombaSayisi").ToString();
        }
        else
        {
            Yok.Play();
        }


    }
    public void BombaAl()
    {

        PlayerPrefs.SetInt("BombaSayisi", PlayerPrefs.GetInt("BombaSayisi") + 1);
        BombaSayisiText.text = PlayerPrefs.GetInt("BombaSayisi").ToString();

    }
    void GameOver()
    {
        GameOverCanvas.SetActive(true);
        Time.timeScale = 0;
        OyunDurdumu = true;
        Cursor.visible = true;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = false;
        Cursor.lockState = CursorLockMode.None;
    }
    void QtusuVersiyonu()
    {
        degisimSes.Play();
        foreach (GameObject silah in silahlar)
        {
            silah.SetActive(false);
        }
        if (AktifSira == silahlar.Length - 1)
        {
            AktifSira = 0;
        }
        else
        {
            AktifSira++;
        }
        silahlar[AktifSira].SetActive(true);

        kelesScript = silahlar[AktifSira].GetComponent<keles>();
        sniperScript = silahlar[AktifSira].GetComponent<Sniper>();
        pompaliScript = silahlar[AktifSira].GetComponent<Pompali>();
        magnumScript = silahlar[AktifSira].GetComponent<Magnum>();

        UpdateAmmoText();
    }

    void UpdateAmmoText()
    {
        if (kelesScript != null)
        {
            if (kelesScript.ToplamMermi_Text != null)
            {
                kelesScript.ToplamMermi_Text.text = kelesScript.ToplamMermiSayisi.ToString();
            }
            if (kelesScript.KalanMermi_Text != null)
            {
                kelesScript.KalanMermi_Text.text = kelesScript.KalanMermi.ToString();
            }
        }
        if (sniperScript != null)
        {
            if (sniperScript.ToplamMermi_Text != null)
            {
                sniperScript.ToplamMermi_Text.text = sniperScript.ToplamMermiSayisi.ToString();
            }
            if (sniperScript.KalanMermi_Text != null)
            {
                sniperScript.KalanMermi_Text.text = sniperScript.KalanMermi.ToString();
            }
        }
        if (pompaliScript != null)
        {
            if (pompaliScript.ToplamMermi_Text != null)
            {
                pompaliScript.ToplamMermi_Text.text = pompaliScript.ToplamMermiSayisi.ToString();
            }
            if (pompaliScript.KalanMermi_Text != null)
            {
                pompaliScript.KalanMermi_Text.text = pompaliScript.KalanMermi.ToString();
            }
        }
        if (magnumScript != null)
        {
            if (magnumScript.ToplamMermi_Text != null)
            {
                magnumScript.ToplamMermi_Text.text = magnumScript.ToplamMermiSayisi.ToString();
            }
            if (magnumScript.KalanMermi_Text != null)
            {
                magnumScript.KalanMermi_Text.text = magnumScript.KalanMermi.ToString();
            }
        }
    }

    public void BastanBasla()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        OyunDurdumu = false;
       

        Time.timeScale = 1;
        Cursor.visible = false;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = true;
        Cursor.lockState = CursorLockMode.Locked;

    }
    public void Pause()
    {
        PauseCanvas.SetActive(true);
        Time.timeScale = 0;
        OyunDurdumu = true;
        Cursor.visible = true;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = false;
        Cursor.lockState = CursorLockMode.None;
    }
    public void DevamEt()
    {
        PauseCanvas.SetActive(false);
        Time.timeScale = 1;
        OyunDurdumu = false;
        Cursor.visible = false;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void AnaMenu()
    {
        SceneManager.LoadScene(0);
    }
}

