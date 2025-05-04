using UnityEngine;
using System.Collections.Generic;

public class SystemMusic : MonoBehaviour
{
    [Header("再生するMP3のリスト")]
    public List<AudioClip> musicClips = new List<AudioClip>();

    [Header("次の再生までの待ち時間（秒）")]
    public float minDelay = 5f;
    public float maxDelay = 15f;

    public AudioSource audioSource;
    private float delayTimer = 0f;
    private float nextDelayTime = 0f;
    private bool isWaiting = false;

    void Start()
    {
        // AudioSource をこの GameObject に追加
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        SetNextDelay();
    }

    void Update()
    {
        if (musicClips.Count == 0) return;

        // 再生中なら何もしない
        if (audioSource.isPlaying) return;

        // 待ち時間をカウント
        if (!isWaiting)
        {
            isWaiting = true;
            delayTimer = 0f;
            nextDelayTime = Random.Range(minDelay, maxDelay);
        }

        delayTimer += Time.deltaTime;

        if (delayTimer >= nextDelayTime)
        {
            PlayRandomClip();
            isWaiting = false;
        }
    }

    void PlayRandomClip()
    {
        int index = Random.Range(0, musicClips.Count);
        AudioClip selectedClip = musicClips[index];
        audioSource.clip = selectedClip;
        audioSource.Play();

        Debug.Log($"[SystemMusic] 再生: {selectedClip.name}");
    }

    void SetNextDelay()
    {
        nextDelayTime = Random.Range(minDelay, maxDelay);
    }
}
