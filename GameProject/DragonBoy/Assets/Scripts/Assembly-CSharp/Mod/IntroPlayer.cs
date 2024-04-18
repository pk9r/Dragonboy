using System.IO;
using Mod;
using Mod.ModHelper.Menu;
using Mod.R;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Threading;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE
using SFB;
#elif UNITY_ANDROID
using EHVN;
#endif

public class IntroPlayer : MonoBehaviour
{
    class IntroPlayerChatable : IChatable
    {
        public void onChatFromMe(string text, string to)
        {
            if (string.IsNullOrEmpty(text))
            {
                onCancelChat();
                return;
            }
            if (ChatTextField.gI().strChat == Strings.introInputVolume)
            {
                if (int.TryParse(text, out int result))
                {
                    if (result < 0 || result > 100)
                        GameCanvas.startOKDlg(string.Format(Strings.inputNumberOutOfRange, 0, 100) + '!');
                    else
                    {
                        volume = result / 100f;
                        Utils.SaveData("intro_volume", volume);
                        GameScr.info1.addInfo(string.Format(Strings.valueChanged, Strings.setIntroVolumeTitle, result) + '!', 0);
                    }
                }
                else
                    GameCanvas.startOKDlg(Strings.invalidValue + '!');
            }
            onCancelChat();
        }

        public void onCancelChat() => ChatTextField.gI().ResetTF();
    }

    internal static bool isEnabled;
    internal static string path;
    internal static float volume = 1f;

    static VideoPlayer videoPlayer;
    static bool isPlaying;

    void Awake()
    {
        videoPlayer = GameObject.Find("Main Camera").GetComponent<VideoPlayer>();
        Utils.TryLoadDataBool("intro_enabled", out isEnabled);
        Utils.TryLoadDataString("intro_path", out path);
        if (Utils.TryLoadDataInt("intro_volume", out int vol))
            volume = vol / 100f;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.OnGameStart();
        if (!File.Exists(path) || !isEnabled)
        {
            SceneManager.LoadScene("NROL");
            return;
        }
        videoPlayer.url = path;
        if (volume == 0)
            videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
        else 
            videoPlayer.SetDirectAudioVolume(0, volume);
        videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
        videoPlayer.Prepare();
    }

    void VideoPlayer_prepareCompleted(VideoPlayer source)
    {
        videoPlayer.Play();
        isPlaying = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!videoPlayer.isPlaying && isPlaying)
            SceneManager.LoadScene("NROL");
    }

    void OnGUI()
    {
        if (videoPlayer.isPlaying && isPlaying)
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), videoPlayer.texture, ScaleMode.ScaleToFit);
        if (Input.GetMouseButtonDown(0))
        {
            videoPlayer.Stop();
            SceneManager.LoadScene("NROL");
        }    
    }

    internal static void ShowMenu()
    {
        MenuBuilder menuBuilder = new MenuBuilder()
            .setChatPopup(Strings.introCurrentPath + ": " + path)
            .addItem(Strings.introChangeVideoPath, new MenuAction(SelectVideo))
            .addItem(Strings.setIntroVolumeTitle, new MenuAction(() =>
            {
                ChatTextField.gI().strChat = Strings.introInputVolume;
                ChatTextField.gI().tfChat.name = Strings.introInputVolumeHint;
                ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                ChatTextField.gI().startChat2(new IntroPlayerChatable(), string.Empty);
                ChatTextField.gI().tfChat.setText((volume * 100).ToString());
            }));
        if (string.IsNullOrEmpty(path))
            menuBuilder.setChatPopup(Strings.introNoVideo + '.');
        menuBuilder.start();
    }

    internal static void SelectVideo()
    {
        string[] paths = null;
        new Thread(delegate ()
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE
            ExtensionFilter[] extensions = new[]
            {
                new ExtensionFilter(Strings.videoFile, "mp4" ),
                new ExtensionFilter(Strings.allFileTypes, "*" ),
            };
            paths = StandaloneFileBrowser.OpenFilePanel(Strings.introSelectFile, "", extensions, false);
#elif UNITY_ANDROID
            paths = FileChooser.Open(new string[] { "video/mp4" });
#endif
            if (paths.Length == 0)
                return;
            path = paths[0];
            Utils.SaveData("intro_path", path);
        })
        { IsBackground = true }.Start();
    }

}
