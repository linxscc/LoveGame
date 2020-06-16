using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DownloadObbExample : MonoBehaviour {
    
    private string expPath;

    void OnGUI()
	{
		if (!GooglePlayDownloader.RunningOnAndroid())
		{
			GUI.Label(new Rect(10, 10, Screen.width-10, 20), "Use GooglePlayDownloader only on Android device!");
			return;
		}

	    expPath = GooglePlayDownloader.GetExpansionFilePath();
	    if (expPath == null)
		{
				GUI.Label(new Rect(10, 10, Screen.width-10, 20), "External storage is not available!");
		}
		else
		{
			string mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
			string patchPath = GooglePlayDownloader.GetPatchOBBPath(expPath);

		    if (mainPath == null)
		    {
                Debug.LogWarning("DownloadObbExample==>FetchOBB-------1");
		        GooglePlayDownloader.FetchOBB();
		        Debug.LogWarning("DownloadObbExample==>FetchOBB-------2");
		    }

            StartCoroutine(loadLevel());

//			GUI.Label(new Rect(10, 10, Screen.width-10, 20), "Main = ..."  + ( mainPath == null ? " NOT AVAILABLE" :  mainPath.Substring(expPath.Length)));
//			GUI.Label(new Rect(10, 25, Screen.width-10, 20), "Patch = ..." + (patchPath == null ? " NOT AVAILABLE" : patchPath.Substring(expPath.Length)));
//			if (mainPath == null || patchPath == null)
//				if (GUI.Button(new Rect(10, 100, 100, 100), "Fetch OBBs"))
//					GooglePlayDownloader.FetchOBB();
		}

	}

    protected IEnumerator loadLevel()
    {
        string mainPath;
        do
        {
            yield return new WaitForSeconds(0.5f);
            mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
            Debug.LogWarning("waiting mainPath " + mainPath);
        }
        while (mainPath == null);

        SceneManager.LoadScene("LoadingPage");

//        if (downloadStarted == false)
//        {
//            downloadStarted = true;
//
//            string uri = "file://" + mainPath;
//            log("downloading " + uri);
//            WWW www = WWW.LoadFromCacheOrDownload(uri, 0);
//
//            // Wait for download to complete
//            yield return www;
//
//            if (www.error != null)
//            {
//                log("wwww error " + www.error);
//            }
//            else
//            {
//                Application.LoadLevel(nextScene);
//            }
//        }
    }
}
