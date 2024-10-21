using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerControllerClient : MonoBehaviour
{
    public int movSpeed = 100;
    public GameObject consoleUDP;
    public GameObject consoleTCP;
    public GameObject waitingRoomTcp;
    public TMP_InputField chatBox;

    Animator animator;

    private bool nearUDP = false;
    private bool nearTCP = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 speed = new Vector3(0, 0, 0);
        if (Input.GetKey("d") && !chatBox.isFocused)
        {
            // speed += Vector3.Cross(new Vector3(1, 0, 0), this.transform.forward) * movSpeed;
            speed += new Vector3(1, 0, 0) * movSpeed;
        }
        if (Input.GetKey("a") && !chatBox.isFocused)
        {
            //speed += Vector3.Cross(new Vector3(-1, 0, 0), this.transform.forward) * movSpeed;
            speed += new Vector3(-1, 0, 0) * movSpeed;

        }
        if (Input.GetKey("w") && !chatBox.isFocused)
        {
           // speed += Vector3.Cross(new Vector3(0, 0, 1), this.transform.forward) * movSpeed;
            speed += new Vector3(0, 0, 1) * movSpeed;

        }
        if (Input.GetKey("s") && !chatBox.isFocused)
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

        if (nearUDP && Input.GetKeyDown("e") && !chatBox.isFocused)
        {
            consoleUDP.SetActive(!consoleUDP.activeSelf);

        }
        if (nearTCP && Input.GetKeyDown("e") && !chatBox.isFocused)
        {
            consoleTCP.SetActive(!consoleTCP.activeSelf);

        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("ComputerTCP"))
        {
            nearTCP = true;
        }
        if (other.gameObject.CompareTag("ComputerUDP"))
        {
            nearUDP = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("ComputerTCP"))
        {
            nearTCP = false;
            consoleTCP.SetActive(false);
        }
        if (other.gameObject.CompareTag("ComputerUDP"))
        {
            nearUDP = false;
            consoleUDP.SetActive(false);
        }
    }
}


