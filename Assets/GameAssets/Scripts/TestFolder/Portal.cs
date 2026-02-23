using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Portal : MonoBehaviour
{
    [SerializeField]
  //  LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (((other.gameObject.layer) & layerMask.value) == 0) { return; }
        Debug.Log("충돌 일어나냐 ?");
        LoadScene(1);
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
