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

    string Title;
    int BPM;
    List<GameObject> Notes;

    void OneEnable()
    {
        Play.onClick
           .AsObservable()
           .Subscribe(_ => play());

        SetChart.onClick
            .AsObservable()
            .Subscribe(_ => loadChart());
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
            else
            {
                Note = Instantiate(murasaki, SpawnPointmurasaki.position, Quaternion.identity);
            }

            Notes.Add(Note);
        }
    }

    void play()
    {
        Debug.Log("Game Start!");
    }
}

