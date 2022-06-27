using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Messenger : MonoBehaviour
{
    public TMP_Text message1;
    public TMP_Text message2;
    public TMP_Text message3;
    public TMP_Text message4;

    Queue<string> MessageToProcess1 = new Queue<string>();
    Queue<string> MessageToProcess2 = new Queue<string>();
    Queue<string> MessageToProcess3 = new Queue<string>();
    Queue<string> MessageToProcess4 = new Queue<string>();

    void MessageRecieved1(string m)
    {
        MessageToProcess1.Enqueue(m);
    }

    void MessageRecieved2(string m)
    {
        MessageToProcess2.Enqueue(m);
    }

    void MessageRecieved3(string m)
    {
        MessageToProcess3.Enqueue(m);
    }

    void MessageRecieved4(string m)
    {
        MessageToProcess4.Enqueue(m);
    }
    private void Start()
    {
        Message.MessageSent1 += MessageRecieved1;
        Message.MessageSent2 += MessageRecieved2;
        Message.MessageSent3 += MessageRecieved3;
        Message.MessageSent4 += MessageRecieved4;
    }

    // Update is called once per frame
    void Update()
    {
        if(MessageToProcess1.Count > 0)
        {
            message1.text = MessageToProcess1.Dequeue();
        }
        if (MessageToProcess2.Count > 0)
        {
            message2.text = MessageToProcess2.Dequeue();
        }
        if (MessageToProcess3.Count > 0)
        {
            message3.text = MessageToProcess3.Dequeue();
        }
        if (MessageToProcess4.Count > 0)
        {
            message4.text = MessageToProcess4.Dequeue();
        }
    }
}
