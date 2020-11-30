using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
	public static Pause Instance { get; private set; }
	public GameObject screens;
	public GameObject loreReader;
	public GameObject loreList;
	public Button escapeReaderButton;
	public Button initialButton;
	public int lorePage;
	public int activeScreen = 0;
	private Dictionary<int, string> loreBook = new Dictionary<int, string>();

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}

		loreBook.Add(0, "Did not assign this collectible's ID");
		loreBook.Add(1, "Hello, this is the journal of Zebrille, head researcher on the Florauna 2210 project. If this is found, please give it to Clorva, my sponsor, for study. It is the start of our second day on the asteroid GG707 and the first free moment I’ve had. The entire crew arrived yesterday, I met the physician Yenn, and my research assistants Brendle and Kolb. We set up our spaces, got to know each other, and set up the specimens for growth. "
			+ "\nIn just one week we will be able to do our control tests on the 100’s of A through R. I know we have a lot of work to do before we can even think of beginning mutations, but our excitement was palpable as we talked last night about what we will accomplish here over the next 3 years. I’m glad we get along. I knew we’d work well together, based on our backgrounds. But liking each other, that’s a bonus I couldn’t be sure about.");
		loreBook.Add(2, "Tests today continued the positive trend in humidity responsiveness. The F300s’ moisture adaptation is remarkable. They need a few tweaks for temperature susceptibility, but are already more resilient than any recorded plant species! I knew we’d be testing the boundaries of the possible here, but this is beyond what I’d hoped we could accomplish. Brendle finished prepping the F400s’ splicing today and we can grow them tomorrow, with testing starting a few days after."
			+ "\nIf our theory is correct, by combining what we’ve grown here with the R800s’ enhanced logical reasoning and the B400s’ physical malleability, the F400 could be the first batch of the last plants we ever need to create. A new species that self-preserves, evolving in real-time to aid itself, and the environment around it."
			+ "\nThis is why I came here. This is why I had to come. We will literally change the face of the planet! Of course, as Kolb keeps saying, the real work will begin once the development is complete, and we have to control what is done with it. But I will allow myself to relish this delight now. We’ve earned it.");
		loreBook.Add(3, "I know I shouldn’t pick favorites. Forgive the informal language here, reviewing my entries lately, I see my passion has taken over while writing in here, and I won’t be able to publish this. Which is fine, I need somewhere to record my thoughts anyway, unfiltered."
			+ "\nF402 is… remarkable. A flora on a level of mental acuity even I didn’t think could be reached. Yes, mental acuity, at this point there is no denying that there is a conscious process in that body delivering messages, making choices, possibly even having thoughts? Brendle is attempting to communicate with it. I’ve allowed it, out of curiosity. They are attempting to teach the plant language through sensory stimulation. Brendle admits that they do not have a specific hypothesis or plan for how to capitalize on this experiment, just optimism. That’s why I love them."
			+ "\nOh, wow. Huh, I was going to erase it, but… that looks nice. I love them.");
	}

	void Start () {
		Time.timeScale = 0;
		ActivatePause();
		ScreenSwitch(0);
		foreach (int co in PersistentManager.Instance.Collectibles)
		{
			loreList.transform.GetChild(co - 1).gameObject.SetActive(true);
		}
	}

	void Update ()
	{
		//uses the p button to pause and unpause the game
		if (Input.GetKeyDown(PersistentManager.Instance.PauseKey))
		{
			Debug.Log("Time scale is "+Time.timeScale);
			ActivatePause();
		}
	}

	public void ScreenSwitch(int changeValue)
	{
		int screenCount = screens.transform.childCount;
		int newScreen = activeScreen + changeValue;
		if (newScreen >= screenCount)
		{
			newScreen = newScreen - screenCount;
		}
		if (newScreen < 0)
		{
			newScreen = newScreen + screenCount;
		}
		screens.transform.GetChild(activeScreen).gameObject.SetActive(false);
		screens.transform.GetChild(newScreen).gameObject.SetActive(true);
		activeScreen = newScreen;
	}

	//shows objects with ShowOnPause tag
	public void ActivatePause()
	{
		bool paused = Time.timeScale==0;
		if (!paused)
		{
			CloseLore();
			initialButton.Select();
			initialButton.OnSelect(null);
			Time.timeScale = 0;
			PersistentManager.Instance.musicPlayer.volume = PersistentManager.Instance.quietMusicVolume;
		}
		else
		{
			Time.timeScale = 1;
			StartCoroutine(PersistentManager.Instance.LerpVolume(1f, PersistentManager.Instance.standardMusicVolume));
		}

		for (int childIndex = 0; childIndex < this.transform.childCount; childIndex++)
		{
			this.transform.GetChild(childIndex).gameObject.SetActive(!paused);
		}
	}

	public void OpenLore(int entry)
	{
		Debug.Log("opening "+entry);
		loreReader.transform.GetChild(0).GetComponent<Text>().text = loreBook[entry];
		loreReader.SetActive(true);
		escapeReaderButton.Select();
		escapeReaderButton.OnSelect(null);
	}

	public void CloseLore()
	{
		loreReader.SetActive(false);
		initialButton.Select();
		initialButton.OnSelect(null);
	}
}
