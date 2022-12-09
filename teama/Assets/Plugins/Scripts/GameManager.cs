using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class GameManager : MonoBehaviour
{
    [SerializeField] string FilePath;
    [SerializeField] string ClipPath;

    [SerializeField] Button Play;
    [SerializeField] Button SetChart;


    [SerializeField] GameObject aka;
    [SerializeField] GameObject ki;
    [SerializeField] GameObject ao;
    [SerializeField] GameObject midori;
    [SerializeField] GameObject murasaki;

    [SerializeField] Transform SpawnPointaka;
    [SerializeField] Transform SpawnPointki;
    [SerializeField] Transform SpawnPointao;
    [SerializeField] Transform SpawnPointmidori;
    [SerializeField] Transform SpawnPointmurasaki;
    [SerializeField] Transform BeatPointaka;
    [SerializeField] Transform BeatPointki;
    [SerializeField] Transform BeatPointao;
    [SerializeField] Transform BeatPointmidori;
    [SerializeField] Transform BeatPointmurasaki;

    AudioSource Music;

    float PlayTime;
    float Distance;
    float During;
    bool isPlaying;
    int GoIndex;

    float CheckRange;
    float BeatRange;
    List<float> NoteTimings;



    string Title;
    int BPM;
    List<GameObject> Notes;

    Subject<string> SoundEffectSubject = new Subject<string>();

    public IObservable<string> OnSoundEffect
    {
        get { return SoundEffectSubject; }
    }

    Subject<string> MessageEffectSubject = new Subject<string>();

    public IObservable<string> OnMessageEffect
    {
        get { return MessageEffectSubject; }
    }

    void OnEnable()
    {
        Music = this.GetComponent<AudioSource>();
        Distance = Math.Abs(BeatPointaka.position.y - SpawnPointaka.position.y);
        During = 2 * 1000;
        isPlaying = false;
        GoIndex = 0;

        CheckRange = 120;
        BeatRange = 80;

        Debug.Log(Distance);

        Play.onClick
           .AsObservable()
           .Subscribe(_ => play());

        SetChart.onClick
            .AsObservable()
            .Subscribe(_ => loadChart());

        this.UpdateAsObservable()
            .Where(_ => isPlaying)
            .Where(_ => Notes.Count > GoIndex)
            .Where(_ => Notes[GoIndex].GetComponent<NoteController>().getTiming() <= ((Time.time * 1000 - PlayTime) + During))
            .Subscribe(_ =>
            {
                Notes[GoIndex].GetComponent<NoteController>().go(Distance, During);
                GoIndex++;
            });


        this.UpdateAsObservable()
            .Where(_ => isPlaying)
            .Where(_ => Input.GetKeyDown(KeyCode.B))
            .Subscribe(_ =>
            {
                beat("ki", Time.time * 1000 - PlayTime);
                SoundEffectSubject.OnNext("ki");
            });

        this.UpdateAsObservable()
            .Where(_ => isPlaying)
            .Where(_ => Input.GetKeyDown(KeyCode.H))
            .Subscribe(_ =>
            {
                beat("midori", Time.time * 1000 - PlayTime);
                SoundEffectSubject.OnNext("midori");
            });

        this.UpdateAsObservable()
            .Where(_ => isPlaying)
            .Where(_ => Input.GetKeyDown(KeyCode.J))
            .Subscribe(_ =>
            {
                beat("ao", Time.time * 1000 - PlayTime);
                SoundEffectSubject.OnNext("ao");
            });

        this.UpdateAsObservable()
            .Where(_ => isPlaying)
            .Where(_ => Input.GetKeyDown(KeyCode.K))
            .Subscribe(_ =>
            {
                beat("murasaki", Time.time * 1000 - PlayTime);
                SoundEffectSubject.OnNext("murasaki");
            });

        this.UpdateAsObservable()
            .Where(_ => isPlaying)
            .Where(_ => Input.GetKeyDown(KeyCode.L))
            .Subscribe(_ =>
            {
                beat("aka", Time.time * 1000 - PlayTime);
                SoundEffectSubject.OnNext("aka");
            });
    }

    void loadChart()
    {
        Notes = new List<GameObject>();
        NoteTimings = new List<float>();

        string jsonText = Resources.Load<TextAsset>(FilePath).ToString();
        Music.clip = (AudioClip)Resources.Load(ClipPath);

        JsonNode json = JsonNode.Parse(jsonText);
        Title = json["title"].Get<string>();
        BPM = int.Parse(json["bpm"].Get<string>());

        foreach (var note in json["notes"])
        {
            string type = note["type"].Get<string>();
            float timing = float.Parse(note["timing"].Get<string>());
            Debug.Log("TEST");
            GameObject Note;
            if (type == "aka")
            {
                Note = Instantiate(aka, SpawnPointaka.position, Quaternion.identity);
            }
            else if (type == "ki")
            {
                Note = Instantiate(ki, SpawnPointki.position, Quaternion.identity);
            }
            else if (type == "ao")
            {
                Note = Instantiate(ao, SpawnPointao.position, Quaternion.identity);
            }
            else if (type == "midori")
            {
                Note = Instantiate(midori, SpawnPointmidori.position, Quaternion.identity);
            }
            else if (type == "murasaki")
            {
                Note = Instantiate(murasaki, SpawnPointmurasaki.position, Quaternion.identity);
            }
            else
            {
                Note = Instantiate(aka, SpawnPointaka.position, Quaternion.identity);
            }

            Note.GetComponent<NoteController>().setParameter(type, timing);

            Notes.Add(Note);
            Debug.Log("notekazu" + Notes.Count);
            NoteTimings.Add(timing);
        }
        Debug.Log("notekazu" + Notes.Count);
    }


    void play()
    {
        Music.Stop();
        Music.Play();
        PlayTime = Time.time * 1000;
        isPlaying = true;
        Debug.Log("Game Start!");
    }

    void beat(string type, float timing)
    {
        float minDiff = -1;
        int minDiffIndex = -1;

        for (int i = 0; i < NoteTimings.Count; i++)
        {
            if (NoteTimings[i] > 0)
            {
                float diff = Math.Abs(NoteTimings[i] - timing);
                if (minDiff == -1 || minDiff > diff)
                {
                    minDiff = diff;
                    minDiffIndex = i;
                }
            }
        }

        if (minDiff != -1 & minDiff < CheckRange)
        {
            if (minDiff < BeatRange & Notes[minDiffIndex].GetComponent<NoteController>().getType() == type)
            {
                NoteTimings[minDiffIndex] = -1;
                Notes[minDiffIndex].SetActive(false);

                MessageEffectSubject.OnNext("good");
                Debug.Log("beat" + type + "success.");
            }
            else
            {
                NoteTimings[minDiffIndex] = -1;
                Notes[minDiffIndex].SetActive(false);

                MessageEffectSubject.OnNext("failure");
                Debug.Log("beat" + type + "failure.");
            }
        }
        else
        {
            Debug.Log("though");
        }
    }
}

