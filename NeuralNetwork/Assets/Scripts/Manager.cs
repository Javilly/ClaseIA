using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] private GameObject bichinPrefab;
    [SerializeField] private GameObject target;
    [SerializeField] private Text genCounter;

    private List<NeuralNetwork> brains;
    private List<Bichin> bichinList = null;

    private int[] layers = new int[] { 2, 10, 10, 2 };
    [SerializeField] private int populationSize = 20;
    [SerializeField] private float genLifespan = 15;
    private int currentGen = 0;
    private float genTimer;

    private bool training;

    void Awake()
    {
        genTimer = genLifespan;
    }

    void Update()
    {
        if (!training)
        {
            if (currentGen == 0)
            {
                InitializeShipBrains();
            }
            else
            {
                brains.Sort();

                for (int i = 0; i < populationSize / 2; i++)
                {
                    brains[i] = new NeuralNetwork(brains[i + (populationSize / 2)]);
                    brains[i].Mutate();
                    brains[i + (populationSize / 2)] = new NeuralNetwork(brains[i + (populationSize / 2)]);
                }

                for (int i = 0; i < populationSize; i++)
                {
                    brains[i].SetFitness(0f);
                }
            }

            currentGen++;
            genCounter.text = "Current gen: " + currentGen;
            training = true;
            genTimer = Time.time + genLifespan;
            //target.transform.position = new Vector3(UnityEngine.Random.Range(-47f, 47f), UnityEngine.Random.Range(-26f, 26f), -2);
            CreateShipBodies();
        }
        else
        {
            if (Time.time > genTimer)
            {
                training = false;
            }
        }     
    }

    private void CreateShipBodies()
    {
        if (bichinList != null)
        {
            for (int i = 0; i < bichinList.Count; i++)
            {
                GameObject.Destroy(bichinList[i].gameObject);
            }
        }

        bichinList = new List<Bichin>();

        for (int i = 0; i < populationSize; i++)
        {
            Bichin newBichin = ((GameObject)Instantiate(bichinPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation)).GetComponent<Bichin>();
            newBichin.InitializeBichin(brains[i], target.transform);
            bichinList.Add(newBichin);
        }

    }

    private void InitializeShipBrains()
    {
        if (populationSize % 2 != 0)
        {
            populationSize--;
        }

        brains = new List<NeuralNetwork>();

        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork brain = new NeuralNetwork(layers);
            brain.Mutate();
            brains.Add(brain);
        }
    }
}