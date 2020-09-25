using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] bool looping = false;

    int startingWave = 0;
    [SerializeField] int totalEnemies = 0;

    [SerializeField] bool finishSpawn = false;
    bool called = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        } while (looping);
    }

    private void Update()
    {
        DefeatedLevel();
    }

    private void DefeatedLevel()
    {
        if (totalEnemies <= 0 && finishSpawn && !called)
        {
            FindObjectOfType<Cutscene>().SetRun();
            StartCoroutine(SpawnBossWave());
            called = true;
        }
    }

    public void ResetBools()
    {
        called = false;
        finishSpawn = false;
    }

    public IEnumerator SpawnAllWaves()
    {
        for (int i = 0; i < waveConfigs.Count - 1; i++)
        {
            var currentWave = waveConfigs[i];
            yield return new WaitForSeconds(currentWave.GetSpawnTime());
            StartCoroutine(SpawnAllEnemiesInWave(currentWave));
            
        }
        finishSpawn = true;
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig currentWave)
    {

        for (int i = 0; i < currentWave.GetNumberOfEnemies(); i++)
        {
            totalEnemies += 1;
            var newEnemy = Instantiate(currentWave.GetEnemyPrefab(),
                currentWave.GetWaypoints()[startingWave].transform.position,
                Quaternion.identity);
            newEnemy.GetComponent<EnemyPathing>().setWaveConfig(currentWave);
            yield return new WaitForSeconds(currentWave.GetTimeBetweenSpawns());
        }
    }

    public IEnumerator SpawnBossWave()
    {
        var currentWave = waveConfigs[waveConfigs.Count - 1];
        yield return new WaitForSeconds(currentWave.GetSpawnTime());
        StartCoroutine(SpawnAllEnemiesInWave(currentWave));
    }

    public void EnemyDown()
    {
        totalEnemies--;
    }

}
