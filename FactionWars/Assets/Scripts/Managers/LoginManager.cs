using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LoginManager : MonoBehaviour
{
	public GameObject sysMsg;
	public Text sysMsgText;

	[SerializeField]
	private List<GameObject> menus = new List <GameObject>();

	[SerializeField]
	private InputField loginUsername;
	[SerializeField]
	private InputField loginPassword;

	[SerializeField]
	private InputField registerUsername;
	[SerializeField]
	private InputField registerEmail;
	[SerializeField]
	private InputField registerPassword;
	[SerializeField]
	private InputField registerConfirmPassword;


	private void Update()
	{
		if (sysMsg.activeInHierarchy) // This is not working. Change it for canvas if need.
		{
			if (Input.GetMouseButtonDown(0))
			{
			//	Debug.Log("mouse cliced");
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out RaycastHit hit))
				{
					Debug.Log("hit:" + hit.transform.name);
					if(hit.transform.gameObject.CompareTag("message"))
					{
				//		Debug.Log("mouse cliced message");
						sysMsg.SetActive(false);
					}
				}
			}
		}
	}

	public void Login()
	{
		if (loginUsername.text.Length < 1)
			SystemMessage("Insert username");
		if (loginPassword.text.Length < 1)
			SystemMessage("Insert password");


		AccountInfo.Login(loginUsername.text, loginPassword.text);
	}

	public void QuickLogin()
	{
		AccountInfo.Login("Helsinki", "Helsinki");
	}
	public void QuickLogin1()
	{
		AccountInfo.Login("Stockholm", "Stockholm");
	}
	public void QuickLogin2()
	{
		AccountInfo.Login("Riodejaneiro1", "Riodejaneiro");
	}

	public void QuickLogin3()
	{
		AccountInfo.Login("barcelona", "barcelona");
	}


	public void Register()
	{
		Debug.Log("username len" + registerUsername.text.Length + " pw len:" + registerPassword.text.Length);
		
		if(registerPassword.text.Length < 6)
			SystemMessage("Password has to be at least 6 characters");
		if (registerUsername.text.Length < 3)
			SystemMessage("Username has to be at least 3 characters");

		if (registerPassword.text == registerConfirmPassword.text)
			AccountInfo.Register(registerUsername.text, registerEmail.text, registerPassword.text);
		else 
		{
			SystemMessage("password do not match");
			Debug.LogError("password do not match");
		}
	}

	public void ChangeMenu(int i)
	{
		GameFunctions.ChangeMenu(menus.ToArray(), i);
	}

	public void SystemMessage(string message)
	{
		sysMsgText.text = message;
		sysMsg.SetActive(true);
	}

	public void CloseMessage()
	{
		sysMsg.SetActive(false);
	}
}
