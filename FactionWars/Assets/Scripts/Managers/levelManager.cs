using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class levelManager : MonoBehaviour
{
	private static levelManager instance;
	[SerializeField]
	private Image loadingBar;
	[SerializeField]
	private static int index;

	public static int Index
	{
		get { return index; }
		set { index = value; }
	}
	public Image LoadingBar
	{
		get { return loadingBar; }
		set { loadingBar = value; }
	}

	public static levelManager Instance
	{
		get { return instance; }
	}

	AsyncOperation async = null;

	private void Awake()
	{
		if(instance != this)
		{
			instance = this;
		}
	}

	private void Start()
	{
		loadingBar.fillAmount = 0;
		StartCoroutine(LoadingLevel(index));
	}

	private void Update()
	{
		if(async != null)
		{
			loadingBar.fillAmount = async.progress;
		}
	}

	IEnumerator LoadingLevel(int i)
	{
		async = SceneManager.LoadSceneAsync(i);
		yield return null;
	}
	 
	public static void LoadLevel(int i)
	{
		Index = i;
		SceneManager.LoadScene(GameConstants.LOADING_SCENE);

	}
}
