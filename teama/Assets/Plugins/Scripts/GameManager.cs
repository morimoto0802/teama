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
    [SerializeField] string ClipPath

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

    float PlayTime;
    float Distance;
    float During;
    bool isPlaying;
    int GoIndex;

    string Title;
    int BPM;
    List<GameObject> Notes;

    void OnEnable()
    {
        Distance = Math.Abs(BeatPointaka.position.y - SpawnPointaka.position.y);
        During = 2 * 1000;
        isPlaying = false;
        GoIndex = 0;

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
    }

    void loadChart()
    {
        Notes = new List<GameObject>();

        string jsonText = Resources.Load<TextAsset>(FilePath).ToString();

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
        }
    }

    void play()
    {
        PlayTime = Time.time * 1000;
        isPlaying = true;
        Debug.Log("Game Start!");
    }
}

