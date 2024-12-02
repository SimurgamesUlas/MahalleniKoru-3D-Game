using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomba : MonoBehaviour
{
    public float guc;
    public float menzil;
    public float yukariguc;
    public ParticleSystem PatlamaEfekt;
    public Vector3 baslangicKuvveti; 
    private Rigidbody rb;
    AudioSource PatlamaSesi;

    void Start()
    {
        PatlamaSesi = GetComponent<AudioSource>();
        
    }

    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            Destroy(gameObject,.5f);
            Patlama();
        }
    }

    void Patlama()
    {
        Vector3 PatlamaPozisyonu = transform.position;
        Instantiate(PatlamaEfekt, transform.position, transform.rotation);
        if (PatlamaSesi != null && !PatlamaSesi.isPlaying)
        {
            PatlamaSesi.Play();
        }
        Collider[] colliders = Physics.OverlapSphere(PatlamaPozisyonu, menzil);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (hit != null && rb)
            {
                if (hit.gameObject.CompareTag("Dusman"))
                {
                    hit.transform.gameObject.GetComponent<Dusman>().Oldun();
                }
                rb.AddExplosionForce(guc, PatlamaPozisyonu, menzil, 1, ForceMode.Impulse);
            }
        }
    }
}
