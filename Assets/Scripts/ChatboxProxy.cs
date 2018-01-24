using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatboxProxy : MonoBehaviour {
	public void Show(string content)
	{
		Chatbox.Show(content);
	}
}
