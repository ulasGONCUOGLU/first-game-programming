using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class oyuncuKontrol : MonoBehaviour
{

    /// /Ses efekti eklemek için gerekli 
    AudioSource sesKaynagi;
    public AudioClip tebrik;
    public AudioClip yuvarlanma;
    public AudioClip carpma;
    public AudioClip uyari;

    //


    //2 tane materyal
    //
    public Renderer rend;
   public Material m1; //ilk materyalin ref
   public Material m2; //ikinci mat ref
    bool degis = true;
    //mouse tıklandığında mat switch et
    private void OnMouseDown()
    {
        if(degis)
        {
            rend.material = m1;
            degis = false;
        }
        else
        {
            rend.material = m2;
            degis = true;
        }
    }
    // Start is called before the first frame update
    private Rigidbody rb;
    [Tooltip("hız değerini belirler")]
    public float hiz=10;
    int skor = 0;
    //sahnedeki skortext için bir değişken
    public Text  skorTXT;
    //uyarı ve tebrik mesajları için iki text değişken
    public Text uyariTxt;
    public Text tebrikTxt;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sesKaynagi = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        //eğer oyuncu hareket ediyorsa yuvarlanma sesini oynat, hareket etmiyorsa 
        //ya da belli hız değerinin altında sesi durdur
        if(rb.velocity.magnitude>0.2f && sesKaynagi.isPlaying==false)
        {
            sesKaynagi.PlayOneShot(yuvarlanma);
        }
        //if(rb.velocity.magnitude<0.3f)
        //{
        //    sesKaynagi.Stop();
        //}

        
        //uygulamadan çıkış: Escape çıkış
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit(-1);
            Debug.Log("Çıkış yapıldı");
        }

        //zıplama için sadece y ekseni değeri verilir
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 mv = new Vector3(0f, 10f, 0f);
            rb.AddForce(mv * hiz);
            sesKaynagi.PlayOneShot(uyari);
           
        }

        //kuvveti oluştur
        //kullanıcının seçtiği yönde ve bizim belirlediğimiz büyüklükte 
        //kuvvet oluşturulacak
        float movHor;
        float movVer;
        //platform seç girişi uygula 
        if (Application.platform == RuntimePlatform.Android)
        {
            //android ise ivmeölçer/gyro  kullan
            movHor = Input.acceleration.x;
            movVer = Input.acceleration.y;
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            //Klavye bilgisini alır
            movHor = Input.GetAxis("Horizontal");
            movVer = Input.GetAxis("Vertical");
        }
        else
        {
            //Klavye bilgisini alır
            movHor = Input.GetAxis("Horizontal");
            movVer = Input.GetAxis("Vertical");
        }


            //Vector3 movement = new Vector3(x, y, z);
        // x: yatay
        // y: 0.0f yukarı hareket yok
        // z: dikey
        
        //oluşturulan kuvvet oyuncuya uygulanacak
        //AddForce rigidbody'e kuvveti uygulayacak..
        Vector3 movement = new Vector3(movHor, 0.0f, movVer);
        rb.AddForce(movement * hiz);
        

    }

    //çarpışma anında tetikleme yapılacak fonk
    //collider alanını kullanarak
    private void OnTriggerEnter(Collider other)
    {
        //çarpılan nesne eğer "elmas" ise nesneyi yok et/gizle, puanı arttır
        // SetActive : gizle/göster
        // Destroy : nesneyi yok eder
      
        //Collider için isTrigger açık olmalı
        if (other.gameObject.CompareTag("elmaslar"))
        {
            other.gameObject.SetActive(false); //gizler
                                               // Destroy(other.gameObject); //yok etme
            rend.material = other.gameObject.GetComponent<Renderer>().material;
            skor += 10;
            skorTXT.text = "Skor:"+skor.ToString();
            sesKaynagi.PlayOneShot(carpma);
        }
        //son Elmas için şart; 90 puan ve sonElmas etiketi olmalı
        else if(other.gameObject.CompareTag("sonElmas") && skor==90)
        {
            //son elması yok et
            other.gameObject.SetActive(false);

            //skor'u arttır
            skor += 10;
            skorTXT.text = "Skor:"+skor.ToString();

            //tebrikler kazandınız mesajı..3 sn ekranda duracak
            tebrikTxt.text = "Tebrikler.Kazandınız!!!!";
            sesKaynagi.PlayOneShot(tebrik);
            //3 sn sonra sonraki sahneye geç..seviye atlayacak
            //SceneManager sınıfı..LoadScene isimli metot ile sonraki seviye yüklenir
            StartCoroutine(Beklet());
        }
        else
        {
            //UYARI: bu elması en son toplayınız!!!
            //bu uyarı 3 sn ekranda durup gidecek..
            //WAITFORSECONDS komutu!! IEnumarator sınıfından bir metot gerekli, co-routine çalışacak

            uyariTxt.text = "UYARI: Bu elmas en son toplanacak!!!";
            StartCoroutine(Uyari());
            sesKaynagi.PlayOneShot(uyari);
        }

    }//ontrigger blok sonu

    IEnumerator Beklet()
    {
        yield return new WaitForSeconds(3); //3 sn kazandıracak
        SceneManager.LoadScene("cikisSahnesi"); //index yada name

    }
    //IEnumerator tipinde metot
    IEnumerator Uyari()
    {
       yield return new WaitForSeconds(3); //3 sn kazandıracak
        uyariTxt.text = "";

    }

    //private void OnCollisionEnter(Collision nesne)
    //{
    //    // Collision için isTrigger kapalı olmalı
    //  if (nesne.gameObject.CompareTag("elmaslar"))
    //    {
    //        Destroy(nesne.gameObject);//nesne sahneden siliniyor
    //        Debug.Log("Carpma koorinatlari:"+nesne.contacts[0].point);
    //        Debug.Log("Carpilan nesne:" + nesne.gameObject.name);
    //        Debug.Log("Carpan nesne:" + gameObject.name);
    //    }
    //}
}

