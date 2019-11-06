using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NeuralNetwork
{
    int[] neuronsPerLayer;
    float[][] neurons;
    float[][][] weights;
    float[][][] previousWeights;

    float fitness = 0;


    public NeuralNetwork(int[] _neuronsPerLayer)
    {

        neuronsPerLayer = new int[_neuronsPerLayer.Length];

        for (int i = 0; i < _neuronsPerLayer.Length; i++)
        {
            neuronsPerLayer[i] = _neuronsPerLayer[i];
        }

        InitializeNeurons();
        InitializeWeights();
    }

    void InitializeNeurons()
    {
        neurons = new float[neuronsPerLayer.Length][];

        for (int i = 0; i < neuronsPerLayer.Length; i++)
        {
            neurons[i] = new float[neuronsPerLayer[i]];
        }
    }

    void InitializeWeights()
    {
        List<float[][]> dummyWeights = new List<float[][]>();

        for (int i = 1; i < neuronsPerLayer.Length; i++)
        {
            List<float[]> currentLayerWeights = new List<float[]>();
            //int neuronsInPreviousLayer = neuronsPerLayer[i - 1];

            for (int j = 0; j < neurons[i].Length; j++)
            {
                if (i < neuronsPerLayer.Length - 1)
                {
                    if (j < neurons[i].Length - 1)
                    {
                        float[] weightsFound = new float[neuronsPerLayer[i - 1]];

                        for (int k = 0; k < neurons[i - 1].Length; k++)
                        {
                            float initialWeight = UnityEngine.Random.Range(-0.5f, 0.5f);
                            weightsFound[k] = initialWeight;
                        }

                        currentLayerWeights.Add(weightsFound);
                    }
                }
                else
                {
                    float[] weightsFound = new float[neuronsPerLayer[i - 1]];

                    for (int k = 0; k < neurons[i - 1].Length; k++)
                    {
                        float initialWeight = UnityEngine.Random.Range(-0.5f, 0.5f);
                        weightsFound[k] = initialWeight;
                    }

                    currentLayerWeights.Add(weightsFound);
                }
            }

            dummyWeights.Add(currentLayerWeights.ToArray());
        }

        weights = dummyWeights.ToArray();
    }

    private void CopyWeights()
    {
        List<float[][]> weightsCopy = new List<float[][]>();

        for (int i = 0; i < weights.Length; i++)
        {
            List<float[]> currentLayerWeights = new List<float[]>();

            for (int j = 0; j < weights[i].Length; j++)
            {
                float[] weightsFound = new float[weights[i].Length];

                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weightsFound[i] = weights[i][j][k];
                }

                currentLayerWeights.Add(weightsFound);
            }

            weightsCopy.Add(currentLayerWeights.ToArray());
        }

        previousWeights = weightsCopy.ToArray();
    }


    //Feed() Primera instancia antes de la primera layer
    //recibo un array de floats y devuelvo array de floats
    //recorre primera layer de neuronas y uno a uno pone la info que rec bo por parametro
    public float[] Feed(float[] input)
    {
        for (int i = 0; i < neurons[0].Length; i++)
        {
            neurons[0][i] = input[i];
        }
        ActivateNeuron();

        return neurons[neuronsPerLayer.Length - 1];
    }


    //ActivateNeuron
    //Iterar todos los pesos (empezar desde la segunda layer)
    //J va a recorrer todas las neuronas de nuestra layer actual agarrando los datos de la layer anterior
    //math.tanh
    void ActivateNeuron()
    {
        float value = 0;

        for (int i = 1; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    value += weights[i][j][k];
                }
                neurons[i + 1][j] = (float)Math.Tanh(value);
                value = 0;
            }
        }
    }

    public void Mutate()
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    float currentWeight = weights[i][j][k];
                    float rmd = UnityEngine.Random.Range(0f, 100f);

                    if (rmd <= 2)
                    {
                        currentWeight *= -1;
                    }
                    else if (rmd <= 4)
                    {
                        currentWeight = UnityEngine.Random.Range(-0.5f, 0.5f);
                    }
                    else if (rmd <= 6)
                    {
                        float increase = UnityEngine.Random.Range(0f, 1f) + 1f;
                        currentWeight *= increase;
                    }
                    else if (rmd <= 8)
                    {
                        float decrease = UnityEngine.Random.Range(0f, 1f);
                        currentWeight *= decrease;
                    }

                    weights[i][j][k] = currentWeight;
                }
            }
        }
    }

    public void SetFitness(float amount)
    {
        fitness = amount;
    }

    public void AddFitness(float amount)
    {
        fitness += amount;
    }

    public float GetFitness()
    {
        return fitness;
    }
}
