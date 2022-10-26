using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager Instance { get; private set; }

    public GameObject fxprfeab;

    public bool _fadeAS;
    public AudioSource _musicAS;
    public bool _fadeCM;
    public AudioSource m_CurrentMusic;

    // Diccionario de FX
    public List<AudioClip> _fx;
    public Dictionary<string, int> _fxDic;

    // Diccionario de Musica
    public List<AudioClip> _music;
    public Dictionary<string, int> _musicDic;

    // Unity
    void Awake()
    {
        Instance = this;
        _fx = new List<AudioClip>();
        _fxDic = new Dictionary<string, int>();

        _music = new List<AudioClip>();
        _musicDic = new Dictionary<string, int>();

    }

    public void Update()
    {

        if (_fadeAS) _musicAS.volume -= 0.01f;
        else _musicAS.volume += 0.01f;
        _musicAS.volume = Mathf.Clamp(_musicAS.volume, 0.0f, 0.6f);

        if (_fadeCM) m_CurrentMusic.volume -= 0.01f;
        else m_CurrentMusic.volume += 0.01f;
        m_CurrentMusic.volume = Mathf.Clamp(m_CurrentMusic.volume, 0.0f, 0.6f);

    }

    // MusicPlayer.cs <Load>
    private void Load(List<AudioClip> list, Dictionary<string, int> dic, string path, string audio)
    {
        list.Add(Resources.Load<AudioClip>("Audio/" + path + "/" + audio));
        dic.Add(audio, list.Count - 1);
    }

    private void LoadFX(string audio)
    {
        Load(_fx, _fxDic, "FX", audio);
    }

    private void LoadMusic(string audio)
    {
        Load(_music, _musicDic, "Music", audio);
    }

    // MusicPlayer.cs <Exists>
    private bool Exists(Dictionary<string, int> dic, string key)
    {
        return dic.ContainsKey(key);
    }

    private bool ExistsFX(string audio)
    {
        return Exists(_fxDic, audio);
    }

    private bool ExistsMusic(string audio)
    {
        return Exists(_musicDic, audio);
    }

    // MusicPlayer.cs <Get>
    private AudioClip GetFX(string audio)
    {
        return _fx[_fxDic[audio]];
    }

    private AudioClip GetMusic(string audio)
    {
        return _music[_musicDic[audio]];
    }

    // MusicPlayer.cs <Play>
    public void PlayMe(AudioSource audio)
    {
        audio.PlayOneShot(audio.clip);
    }

    public void PlayFX(string audio, float volume = 1)
    {
        if (!ExistsFX(audio)) LoadFX(audio);

        // Play del FX con el mixer del FX
        GameObject g = Instantiate(fxprfeab, transform);
        g.GetComponent<AudioSource>().PlayOneShot(GetFX(audio), volume);
        Destroy(g, GetFX(audio).length * 1.2f);
    }

    public void PlayMusic(string audio, float volume = 1, bool repeat = false)
    {
        if (!ExistsMusic(audio)) LoadMusic(audio);

        // Play de la Music con el mixer de la Music
        if (m_CurrentMusic.isPlaying)
        {
            _musicAS.clip = GetMusic(audio);
            _musicAS.Play();
            _musicAS.loop = repeat;
            _musicAS.volume = 0.0f;
            _fadeCM = true; _fadeAS = false;
        }
        else if (_musicAS.isPlaying)
        {
            m_CurrentMusic.clip = GetMusic(audio);
            m_CurrentMusic.Play();
            m_CurrentMusic.loop = repeat;
            m_CurrentMusic.volume = 0.0f;
            _fadeAS = true; _fadeCM = false;
        }
        else
        {
            m_CurrentMusic.clip = GetMusic(audio);
            m_CurrentMusic.Play();
            m_CurrentMusic.loop = repeat;
            m_CurrentMusic.volume = 0.0f;
            _fadeAS = false;
        }
    }

    // MusicPlayer.cs <Control>
    public bool IsPlaying(string audio)
    {
        return false;
    }
}
