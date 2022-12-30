using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] string FilePath;
    [SerializeField] string ClipPath;

    [SerializeField] Button Play;
    [SerializeField] Button SetChart;
    [SerializeField] TextMeshProUGUI ScoreText; //追加
    [SerializeField] TextMeshProUGUI ComboText; //追加
    [SerializeField] TextMeshProUGUI TitleText; //追加

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

    float ComboCount; //追加
    float Score; //追加
    float ScoreFirstTerm; //追加
    float ScoreTorerance; //追加
    float ScoreCeilingPoint = 1050000; //追加
    int CheckTimingIndex = 0; //追加

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

        //追加
        this.UpdateAsObservable()
            .Where(_ => isPlaying)
            .Where(_ => Notes.Count > CheckTimingIndex)
            .Where(_ => NoteTimings[CheckTimingIndex] == -1)
            .Subscribe(_ => CheckTimingIndex++);

        //追加
        this.UpdateAsObservable()
            .Where(_ => isPlaying)
            .Where(_ => Notes.Count > CheckTimingIndex)
            .Where(_ => NoteTimings[CheckTimingIndex] != -1)
            .Where(_ => NoteTimings[CheckTimingIndex] < ((PlayTime * 1000 - PlayTime) - CheckRange / 2))
            .Subscribe(_ =>
            {
                updateScore("failure");
                CheckTimingIndex++;
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
        Debug.Log("notekazu"+ Notes.Count) ;

        TitleText.text = Title; //追加

        //追加
        if(Notes.Count < 10)
        {
            ScoreFirstTerm = (float)Math.Round(ScoreCeilingPoint / Notes.Count);
            ScoreTorerance = 0;
        }else if(10 <= Notes.Count && Notes.Count < 30)
        {
            ScoreFirstTerm = 300;
            ScoreTorerance = (float)Math.Floor((ScoreCeilingPoint - ScoreFirstTerm * Notes.Count) / (Notes.Count - 9));
        }else if(30 <= Notes.Count && Notes.Count < 50)
        {
            ScoreFirstTerm = 300;
            ScoreTorerance = (float)Math.Floor((ScoreCeilingPoint - ScoreFirstTerm * Notes.Count) / (2 * (Notes.Count - 19)));
        }else if(50 <= Notes.Count && Notes.Count < 100)
        {
            ScoreFirstTerm = 300;
            ScoreTorerance = (float)Math.Floor((ScoreCeilingPoint - ScoreFirstTerm * Notes.Count) / (4 * (Notes.Count - 39)));
        }
        else
        {
            ScoreFirstTerm = 300;
            ScoreTorerance = (float)Math.Floor((ScoreCeilingPoint - ScoreFirstTerm * Notes.Count) / (4 * (3 * Notes.Count - 232)));
        }
    }　//ここまでインデントは合っているはず

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
                if(minDiff == -1 || minDiff > diff)
                {
                    minDiff = diff;
                    minDiffIndex = i;
                }
            }
        }

        if(minDiff != -1 & minDiff < CheckRange)
        {
            if(minDiff < BeatRange & Notes[minDiffIndex].GetComponent<NoteController>().getType() == type)
            {
                NoteTimings[minDiffIndex] = -1;
                Notes[minDiffIndex].SetActive(false);

                MessageEffectSubject.OnNext("good");
                updateScore("good"); //追加
                Debug.Log("beat" + type + "success.");
            }
            else
            {
                NoteTimings[minDiffIndex] = -1;
                Notes[minDiffIndex].SetActive(false);

                MessageEffectSubject.OnNext("failure");
                updateScore("false"); //追加
                Debug.Log("beat" + type + "failure.");
            }
        }
        else
        {
            Debug.Log("though");
        }
    }

    //追加
    void updateScore(string result)
    {
        if(result == "good")
        {
            ComboCount++;

            float plusScore;
            if (ComboCount < 10)
            {
                plusScore = ScoreFirstTerm;
            }
            else if (10 <= ComboCount && ComboCount < 30)
            {
                plusScore = ScoreFirstTerm + ScoreTorerance;
            }
            else if (30 <= ComboCount && ComboCount < 50)
            {
                plusScore = ScoreFirstTerm + ScoreTorerance * 2;
            }
            else if (50 <= ComboCount && ComboCount < 100)
            {
                plusScore = ScoreFirstTerm + ScoreTorerance * 4;
            }
            else
            {
                plusScore = ScoreFirstTerm + ScoreTorerance * 8;
            }

            Score += plusScore;
        }
        else if (result == "failure")
        {
            ComboCount = 0;
        }
        else
        {
            ComboCount = 0;
        }

        ComboText.text = ComboCount.ToString();
        ScoreText.text = Score.ToString();
    } 
}

