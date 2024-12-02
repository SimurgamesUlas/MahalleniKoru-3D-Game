using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dusman : MonoBehaviour
{
    NavMeshAgent ajan;
    GameObject Hedef;
    public float health;
    public float DusmanDarbeGucu;
    GameKontrolcu gameKontrol;
    GameObject anaKontrolcum;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        ajan = GetComponent<NavMeshAgent>();

        anaKontrolcum = GameObject.FindWithTag("AnaKontrolcum");
      
    }

    public void HedefBelirle(GameObject objem)
    {
        Hedef = objem;
    }
    // Update is called once per frame
    void Update()
    {
        if (Hedef != null)
        {
            ajan.SetDestination(Hedef.transform.position);
        }
        else
        {
            Debug.LogWarning("Hedef nesnesi henüz belirlenmemiş!");
        }
    }
    public void DarbeAl(float darbeGucu)
    {
        health -= darbeGucu;
        if (health <= 0)
        {
            Oldun();
            gameObject.tag = "Untagged";
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("korumamGerekli"))
        {
            anaKontrolcum.GetComponent<GameKontrolcu>().DarbeAl(DusmanDarbeGucu);
            OldunKutu();
            gameObject.tag = "Untagged";
        }
    }
    public void Oldun()
    {
        anaKontrolcum.GetComponent<GameKontrolcu>().DusmanSayisiGuncelle();
        animator.SetTrigger("Olme");
        Destroy(gameObject,2f);
    }
    void OldunKutu()
    {
        anaKontrolcum.GetComponent<GameKontrolcu>().DusmanSayisiGuncelle();
        Destroy(gameObject);
    }
}
