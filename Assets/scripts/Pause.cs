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
	public GameObject menuButtons;
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

		loreBook.Add(0, "Did not assign this collectible's ID"
			);
		loreBook.Add(1, "Hello, this is the journal of Zebrille, head researcher on the Florauna 2210 project. If this is found, please give it to Clorva, my sponsor, for study. It is the start of our second day on the asteroid GG707 and the first free moment I’ve had. The entire crew arrived yesterday, I met the physician Yenn, and my research assistants Brendle and Kolb. We set up our spaces, got to know each other, and set up the specimens for growth. "
			+ "\nIn just one week we will be able to do our control tests on the flora strains A through R. We are growing 20 specimens of each strain (101 - 120), for a total of 360 specimens. I know we have a lot of work to do before we can even think of beginning mutations, but our excitement was palpable as we talked last night about what we will accomplish here over the next 3 years. I’m glad we get along. I knew we’d work well together, based on our backgrounds. But liking each other, that’s a bonus I couldn’t be sure about."
			);
		loreBook.Add(2, "Already two weeks in and the pace is showing no signs of slowing down. The humidity and airflow stress tests have been going smoothly. Kolb already created impressive visualizations of the data we’ve collected, they are adamant they will find any irregularities as soon as they appear."
			+ "\nBut so far, the data matches the sets we were provided for the plant specimens. The environment appears safe to work in."
			+ "\nSpeaking of, on a personal note I am still getting used to living here. The decor is… lacking. The food is equally bland. The gravity is… close, but not quite right. A surprising positive, however, has been my conversations with Brendle. They are sharp and inquisitive in a way that I have not seen before. Others may mark it as youthful energy, but I see a passion in them that will not extinguish. I wouldn’t be surprised if they are able to take increasing control over the direction of the project if their writing is as articulate and stimulating as their loquation."
			);
		loreBook.Add(3, "");
		loreBook.Add(4, "The moisture adaptations of F301-308 are remarkable. They need a few tweaks for temperature susceptibility, but are already more resilient than any recorded plant species! I knew we’d be testing the boundaries of the possible here, but this is beyond what I’d hoped we could accomplish. Brendle finished prepping the F400s’ splicing today and we can grow them tomorrow, with testing starting a few days after."
			+ "\nIf our theory is correct, by combining what we’ve grown here with the R800s’ enhanced logical reasoning and the B400s’ physical malleability, the F400 could be the first batch of the last plants we ever need to create. A new species that self-preserves, evolving in real-time to aid itself, and the environment around it."
			+ "\nThis is why I came here. This is why I had to come. We will literally change the face of the planet! Of course, as Kolb keeps saying, the real work will begin once the development is complete, and we have to control what is done with it. But I will allow myself to relish this delight now. We’ve earned it."
			);
		loreBook.Add(5, ""); //"I know I shouldn’t pick favorites. Forgive the informal language here, reviewing my entries lately, I see my passion has taken over while writing in here, and I won’t be able to publish this. Which is fine, I need somewhere to record my thoughts anyway, unfiltered."
		// 	+ "\nF402 is… remarkable. A flora on a level of mental acuity even I didn’t think could be reached. Yes, mental acuity, at this point there is no denying that there is a conscious process in that body delivering messages, making choices, possibly even having thoughts? Brendle is attempting to communicate with it. I’ve allowed it, out of curiosity. They are attempting to teach the plant how to conceptualize language, through sensory stimulation. Brendle admits that they do not have a specific hypothesis or plan for how to capitalize on this experiment, just optimism. That’s why I love them."
		// 	+ "\nOh, wow. Hmm, I was going to erase it, but… it feels nice to read. To see it declared. Yes, I love them."
		// 	);
		loreBook.Add(6, "");
		loreBook.Add(7, "Mixed success this week. No I haven’t written recently, it’s become taxing. The writing here. When I need to be monitoring all of the F specimens. When this won’t be published anyway. When the progress is not so much progress anymore, but developments."
			+ "\nA development today, the water supply got cut off from the stress test chamber of the humidity wing. Yenn opened up the floor and we found roots that had broken the pipe. Roots wrapped around the pipe, crushing it open, and shaped to re-direct the water elsewhere. Deeper."
			+ "\nWell, a mixture of roots and vines. With so many novel discoveries, it’s frustratingly difficult to maintain thorough comprehension of everything happening. In my own lab."
			+ "\nNormally, I would talk to Brendle, but they are increasingly distant. Their demeanor is changed. Maybe the others can’t see, but I know. They won’t open up to me, and I can’t open up to them. And now I’m writing here."
			+ "\nWe cut off the water supply for the wing. We took biopsies of the roots/vines. We’re learning, we’re making plans. But, it feel like we’re not the only ones."
			);
		loreBook.Add(8, "I can no longer hide behind denial. I am afraid. I know I should not be, but I am afraid of F402. It is a plant, we are it’s creators, I know this. But…"
			+ "\nI haven’t seen Brendle in days. I miss them. I can’t concentrate without them."
			+ "\nRoots are popping up. Roots, flowers, pods. All over the place. Maybe there’s a pattern, but not one I can discern. And F402 continues to grow. The way it moves. Like it knows something. Like it wants something. Kolb calls it Pando but I won’t give it the power of its own moniker. It is still F402, one of many specimens in our research project. Yes, it is exhibiting… remarkable behavior. That is not a reason to poison it, as Kolb claims. All the more reason to study it, learn from it."
			+ "\nI will write here that I am afraid, but nobody else will know. I will not say it aloud. It is a plant, and nothing more. I am in control. I maintain control. This is my lab."
			);
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
		loreReader.transform.GetChild(0).GetComponent<Text>().text = loreBook[entry];
		loreReader.SetActive(true);
		escapeReaderButton.Select();
		escapeReaderButton.OnSelect(null);
		menuButtons.SetActive(false);
	}

	public void CloseLore()
	{
		loreReader.SetActive(false);
		initialButton.Select();
		initialButton.OnSelect(null);
		menuButtons.SetActive(true);
	}
}
