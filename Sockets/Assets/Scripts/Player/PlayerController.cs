using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int movSpeed = 100;
    public GameObject udpConsole;
    public GameObject tcpConsole;
    public GameObject tcpWaitingRoom;
    public TMP_InputField chatBox;

    Animator animator;

    private bool newarCPU = false;
    private bool nearTCP1CPU = false;
    private bool nearTCP2CPU = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 speed = new Vector3(0, this.gameObject.GetComponent<Rigidbody>().velocity.y, 0);
        if (Input.GetKey("d") && udpConsole.activeSelf == false && tcpConsole.activeSelf == false)
        {
           
            // speed += Vector3.Cross(new Vector3(1, 0, 0), this.transform.forward) * movSpeed;
            speed += new Vector3(1, 0, 0) * movSpeed;
        }
        if (Input.GetKey("a") && udpConsole.activeSelf == false && tcpConsole.activeSelf == false)
        {
            //speed += Vector3.Cross(new Vector3(-1, 0, 0), this.transform.forward) * movSpeed;
            speed += new Vector3(-1, 0, 0) * movSpeed;

        }
        if (Input.GetKey("w") && udpConsole.activeSelf == false && tcpConsole.activeSelf == false)
        {
           // speed += Vector3.Cross(new Vector3(0, 0, 1), this.transform.forward) * movSpeed;
            speed += new Vector3(0, 0, 1) * movSpeed;

        }
        if (Input.GetKey("s") && udpConsole.activeSelf == false && tcpConsole.activeSelf == false  )
        {
            //speed += Vector3.Cross(new Vector3(0, 0, -1), this.transform.forward) * movSpeed;
            speed += new Vector3(0, 0, -1) * movSpeed;

        }


        this.gameObject.GetComponent<Rigidbody>().velocity = speed;

        if (speed != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(this.gameObject.GetComponent<Rigidbody>().velocity);
            animator.SetBool("isWalking", true);
        }
        else { animator.SetBool("isWalking", false); }

        if (newarCPU && Input.GetKeyDown(KeyCode.U) && !chatBox.isFocused)
        {
            udpConsole.SetActive(!udpConsole.activeSelf);

        }
        if (newarCPU && Input.GetKeyDown(KeyCode.T) && !chatBox.isFocused)
        {
            tcpConsole.SetActive(!tcpConsole.activeSelf);

        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Computer1"))
        {
            newarCPU = true;
        }
     
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Computer1"))
        {
            newarCPU = false;
            udpConsole.SetActive(false);
            tcpConsole.SetActive(false);
        }

    }
}


