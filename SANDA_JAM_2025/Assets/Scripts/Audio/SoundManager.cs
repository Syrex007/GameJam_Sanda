using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance { get; private set; }

    [SerializeField] private AudioSource soundFXPrefab;
    [SerializeField] private AudioClip[] soundFXClips;

    private Dictionary<string, AudioClip> clipLookup;
    private Dictionary<string, Queue<AudioSource>> audioSourcePool;
    private Dictionary<string, List<AudioSource>> activeSources;
    private Transform soundParent;

    public AudioMixerGroup mixerMusic;
    public AudioMixerGroup mixerSFX;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeClipLookup();
            audioSourcePool = new Dictionary<string, Queue<AudioSource>>();
            activeSources = new Dictionary<string, List<AudioSource>>();

            // Create a parent object for all audio sources
            soundParent = new GameObject("SoundFX_Pool").transform;
            soundParent.SetParent(transform);
            AudioMixer mixer = Resources.Load<AudioMixer>("Sounds/Mixer");
            mixerMusic = mixer.FindMatchingGroups("Master/Song")[0];
            mixerSFX = mixer.FindMatchingGroups("Master/SFX")[0];
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeClipLookup()
    {
        clipLookup = new Dictionary<string, AudioClip>();
        foreach (var clip in soundFXClips)
        {
            if (clip != null && !clipLookup.ContainsKey(clip.name))
            {
                clipLookup.Add(clip.name, clip);
            }
        }
    }

    public void PlaySoundByName(string clipName, Transform spawnTransform, float volume = 1f, float pitch = 1f, bool loop = false, bool fgSFX = true)
    {
        if (!clipLookup.TryGetValue(clipName, out AudioClip clipToPlay))
        {
            Debug.LogWarning($"SoundFXManager: AudioClip '{clipName}' not found.");
            return;
        }

        AudioSource source = GetAudioSourceFromPool(clipName, spawnTransform.position);
        if (source == null) return;

        source.clip = clipToPlay;
        source.outputAudioMixerGroup = fgSFX ? mixerSFX : mixerMusic;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
        source.Play();

        if (!activeSources.ContainsKey(clipName))
            activeSources[clipName] = new List<AudioSource>();

        activeSources[clipName].Add(source);

        if (!loop)
        {
            StartCoroutine(ReturnToPoolAfterPlay(source, clipName, clipToPlay.length));
        }
    }

    private AudioSource GetAudioSourceFromPool(string clipName, Vector3 position)
    {
        // Clean up null references in pool first
        CleanPool(clipName);

        if (!audioSourcePool.TryGetValue(clipName, out Queue<AudioSource> pool))
        {
            pool = new Queue<AudioSource>();
            audioSourcePool[clipName] = pool;
        }

        AudioSource source = null;

        // Try to get from pool
        while (pool.Count > 0 && source == null)
        {
            source = pool.Dequeue();
            if (source == null) continue; // Skip destroyed objects
        }

        // Create new if needed
        if (source == null)
        {
            GameObject newObj = Instantiate(soundFXPrefab.gameObject, position, Quaternion.identity, soundParent);
            source = newObj.GetComponent<AudioSource>();
            if (source == null)
            {
                Debug.LogError("SoundFXPrefab is missing AudioSource component!");
                return null;
            }
        }

        source.transform.position = position;
        source.gameObject.SetActive(true);
        return source;
    }

    private void CleanPool(string clipName)
    {
        if (audioSourcePool.TryGetValue(clipName, out Queue<AudioSource> pool))
        {
            // Remove any null references
            var tempList = new List<AudioSource>(pool);
            tempList.RemoveAll(x => x == null);
            pool.Clear();
            foreach (var source in tempList) pool.Enqueue(source);
        }

        if (activeSources.TryGetValue(clipName, out List<AudioSource> activeList))
        {
            activeList.RemoveAll(x => x == null);
        }
    }

    private IEnumerator ReturnToPoolAfterPlay(AudioSource source, string clipName, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Check if source was destroyed while waiting
        if (source == null || !source.gameObject) yield break;

        if (activeSources.ContainsKey(clipName))
            activeSources[clipName].Remove(source);

        if (source != null)
        {
            source.Stop();
            source.loop = false;
            source.gameObject.SetActive(false);

            if (!audioSourcePool.ContainsKey(clipName))
                audioSourcePool[clipName] = new Queue<AudioSource>();

            audioSourcePool[clipName].Enqueue(source);
        }
    }

    public void StopSoundByName(string clipName)
    {
        if (!activeSources.ContainsKey(clipName)) return;

        foreach (var source in activeSources[clipName])
        {
            if (source != null && source.gameObject && source.isPlaying)
            {
                source.Stop();
                source.loop = false;
                source.gameObject.SetActive(false);

                if (!audioSourcePool.ContainsKey(clipName))
                    audioSourcePool[clipName] = new Queue<AudioSource>();

                audioSourcePool[clipName].Enqueue(source);
            }
        }

        activeSources[clipName].Clear();
    }

    public bool IsSoundPlaying(string clipName)
    {
        if (activeSources.TryGetValue(clipName, out List<AudioSource> sources))
        {
            foreach (var source in sources)
            {
                if (source != null && source.gameObject && source.isPlaying)
                    return true;
            }
        }
        return false;
    }

    public void StopAllSounds()
{
    // Recorremos todos los clips activos y detenemos sus audios
    foreach (var kvp in activeSources)
    {
        string clipName = kvp.Key;
        List<AudioSource> sources = kvp.Value;

        for (int i = sources.Count - 1; i >= 0; i--)
        {
            AudioSource source = sources[i];
            if (source != null && source.gameObject && source.outputAudioMixerGroup.name == "SFX")
            {
                source.Stop();
                source.loop = false;
                source.gameObject.SetActive(false);

                // Enviamos el AudioSource de vuelta al pool
                if (!audioSourcePool.ContainsKey(clipName))
                    audioSourcePool[clipName] = new Queue<AudioSource>();

                audioSourcePool[clipName].Enqueue(source);
            }
        }

        // Limpiamos la lista de fuentes activas
        sources.Clear();
    }

    // Aseguramos que no quede nada sonando
    activeSources.Clear();
}

    public void StopAllSoundsMusicEffect()
    {
        print("Entra a stopear todo");

        foreach (var kvp in activeSources)
        {
            print("z1");
            string clipName = kvp.Key;
            List<AudioSource> sources = kvp.Value;
            
            for (int i = sources.Count - 1; i >= 0; i--)
            {
                print("z2");
                AudioSource source = sources[i];
                if (source != null && source.gameObject)
                {
                    print("z3");
                    print($"Deteniendo {source.clip?.name}");

                    //  Detenemos la musica correctamente
                    source.loop = false;
                    source.Stop();
                    source.clip = null; //  Liberamos el clip para que Unity no intente seguirlo

                    // Desactivamos el objeto del pool
                    source.gameObject.SetActive(false);

                    // Volvemos a ponerlo en el pool
                    if (!audioSourcePool.ContainsKey(clipName))
                        audioSourcePool[clipName] = new Queue<AudioSource>();

                    audioSourcePool[clipName].Enqueue(source);
                }
            }
            print("z4");
            // Limpiamos la lista de fuentes activas
            sources.Clear();
        }
        print("z5");
        // Por ultimo, vaciamos el diccionario de activos
        activeSources.Clear();

        print("StopAllSoundsMusicEffect COMPLETADO");
    }

}
