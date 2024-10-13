using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elmasKontrol : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //elmas çevrilecek
        //Frame-> Time domainine geçiş
        
        transform.Rotate(new Vector3(15, 30, 40)*Time.deltaTime);
    }
  
}
