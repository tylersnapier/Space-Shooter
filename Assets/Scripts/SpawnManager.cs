using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _enemyVariant;
    [SerializeField]
    private GameObject[] powerups;
    [SerializeField] 
    private GameObject[] debuffs;

    private bool _stopSpawning = false;
    

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

   

    IEnumerator SpawnEnemyRoutine()
    {   
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning==false)
        {
            int _randomEnemy = Random.Range(0, 2);
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
          GameObject newEnemy = Instantiate(_enemyVariant[_randomEnemy], posToSpawn, Quaternion.identity);
          newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
       
    }

    IEnumerator SpawnPowerupRoutine()
    {
       yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 postToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, 6);
            Instantiate(powerups[randomPowerUp], postToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }



    IEnumerator SpawnDebuffRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 postToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomDebuff = Random.Range(0, 1);
            Instantiate(debuffs[randomDebuff], postToSpawn, Quaternion.identity); 
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
