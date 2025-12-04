using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class ReplayManager : MonoBehaviour
{
    public static ReplayManager Instance;

    public ReplayManager GetIntance()
    {
        return Instance;
    }

    private void SetInstance(ReplayManager instance)
    {
        Instance = instance;
    }

    [SerializeField] private int bufferSize = 250;
    [SerializeField] private float replayDuration = 4f;

    private List<ReplayRecord> replayRecords = new List<ReplayRecord>();
    private int currentIndex = 0;
    private bool isReplaying = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    public void RegisterReplay(ReplayRecord record)
    {
        if (!replayRecords.Contains(record))
        {
            replayRecords.Add(record);
            record.Initialize(bufferSize);
        }
    }

    private void FixedUpdate()
    {
        if (isReplaying) return;

        for (int i = 0; i < replayRecords.Count; i++)
        {
            replayRecords[i].RecordSample(currentIndex);
        }

        currentIndex++;
        if (currentIndex >= bufferSize)
        {
            currentIndex = 0;
        }
    }

    public IEnumerator PlayReplay()
    {
        isReplaying = true;

        foreach (var record in replayRecords)
        {
            record.SetReplay(true);
        }

        int framesToReplay = Mathf.Min(bufferSize, Mathf.RoundToInt(replayDuration / Time.fixedDeltaTime));

        int start = currentIndex - framesToReplay;
        if (start < 0) start += bufferSize;

        int index = start;

        for (int f = 0; f < framesToReplay; f++)
        {
            foreach (var record in replayRecords)
            {
                record.PlaySample(index);
            }

            index++;
            if (index >= bufferSize)
            {
                index = 0;
            }

            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }

        foreach (var record in replayRecords)
        {
            record.SetReplay(false);
        }

        isReplaying = false;
    }
}
