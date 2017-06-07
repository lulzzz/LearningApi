﻿using System;
using LearningFoundation;
using NeuralNetworks.Core;
using NeuralNetworks.Core.Layers;
using NeuralNetworks.Core.Neurons;

namespace NeuralNet.RestrictedBoltmannMachine
{
    public class RestrictedBoltzmannMachine : ActivationNetwork
    {

        private StochasticLayer visible;
        private StochasticLayer hidden;
              
        ///   Gets the visible layer of the machine.
       
        public StochasticLayer Visible
        {
            get { return visible; }
        }

        
        ///   Gets the hidden layer of the machine.
        
        public StochasticLayer Hidden
        {
            get { return hidden; }
        }


       
        ///   Creates a new RBM Class with input number and hidden neuron number      
        /// <param name="inputsCount">The number of inputs for the machine.</param>
        /// <param name="hiddenNeurons">The number of hidden neurons in the machine.</param>
      
        public RestrictedBoltzmannMachine(int inputsCount, int hiddenNeurons)
            : this(new BernoulliFunction(alpha: 1), inputsCount, hiddenNeurons) { }

       
        ///   Creates a new RBM class with hiden layer and visible layer     
        /// 
        /// <param name="hidden">The hidden layer to be added in the machine.</param>
        /// <param name="visible">The visible layer to be added in the machine.</param>
        /// 
        public RestrictedBoltzmannMachine(StochasticLayer hidden, StochasticLayer visible)
            : base(null, hidden.InputsCount, 0)
        {
            this.hidden = hidden;
            this.visible = visible;
            base.layers[0] = hidden;
        }

       
        ///   Creates a new RBM class
        
        /// <param name="function">The activation function to use in the network neurons.</param>
        /// <param name="inputsCount">The number of inputs for the machine.</param>
        /// <param name="hiddenNeurons">The number of hidden neurons in the machine.</param>
        /// 
        public RestrictedBoltzmannMachine(IStochasticFunction function, int inputsCount, int hiddenNeurons)
            : base(function, inputsCount, 1)
        {
            this.visible = new StochasticLayer(function, inputsCount, hiddenNeurons);
            this.hidden = new StochasticLayer(function, hiddenNeurons, inputsCount);
            base.layers[0] = hidden;
        }
        
            ///Compute the network output vector using the input vector
        public override double[] Compute(double[] input)
        {
            return hidden.Compute(input);
        }
        
        /// Resconstruc an input vector using a given output
        public double[] Reconstruct(double[] output)
        {
            return visible.Compute(output);
        }
///   Samples an output vector from the network with an input vector.
        ///   Output is A possible output considering the stochastic activations of the network.
        public double[] GenerateOutput(double[] input)
        {
            return hidden.Generate(input);
        }

       
        ///   Samples an input vector from the network        ///   given an output vector.
        ///   Output is         ///   A possible reconstruction considering the        ///   stochastic activations of the network.
      
        public double[] GenerateInput(double[] output)
        {
            return visible.Generate(output);
        }


        /// <summary>
        ///   Constructs a Gaussian-Bernoulli network with 
        ///   visible Gaussian units and hidden Bernoulli units.
        /// </summary>
        /// 
        /// <param name="inputsCount">The number of inputs for the machine.</param>
        /// <param name="hiddenNeurons">The number of hidden neurons in the machine.</param>
        /// 
        /// <returns>A Gaussian-Bernoulli Restricted Boltzmann Machine</returns>
        /// 
        public static RestrictedBoltzmannMachine CreateGaussianBernoulli(int inputsCount, int hiddenNeurons)
        {
            RestrictedBoltzmannMachine network = new RestrictedBoltzmannMachine(inputsCount, hiddenNeurons);

            foreach (var neuron in network.Visible.Neurons)
                neuron.ActivationFunction = new GaussianFunction();

            return network;
        }

        /// <summary>
        ///   Creates a new <see cref="ActivationNetwork"/> from this instance.
        /// </summary>
        /// 
        /// <param name="outputs">The number of output neurons in the last layer.</param>
        /// 
        /// <returns>An <see cref="ActivationNetwork"/> containing this network.</returns>
        /// 
        public ActivationNetwork ToActivationNetwork(int outputs)
        {
            return ToActivationNetwork(new SigmoidFunction(alpha: 1), outputs);
        }

        /// <summary>
        ///   Creates a new <see cref="ActivationNetwork"/> from this instance.
        /// </summary>
        /// 
        /// <param name="outputs">The number of output neurons in the last layer.</param>
        /// <param name="function">The activation function to use in the last layer.</param>
        /// 
        /// <returns>An <see cref="ActivationNetwork"/> containing this network.</returns>
        /// 
        public ActivationNetwork ToActivationNetwork(IActivationFunction function, int outputs)
        {
            ActivationNetwork ann = new ActivationNetwork(function,
                inputsCount, hidden.Neurons.Length, outputs);

            // For each neuron
            for (int i = 0; i < hidden.Neurons.Length; i++)
            {
                ActivationNeuron aneuron = ann.Layers[0].Neurons[i] as ActivationNeuron;
                StochasticNeuron sneuron = hidden.Neurons[i];

                // For each weight
                for (int j = 0; j < sneuron.Weights.Length; j++)
                    aneuron.Weights[j] = sneuron.Weights[j];
                aneuron.Threshold = sneuron.Threshold;
                aneuron.ActivationFunction = sneuron.ActivationFunction;
            }

            return ann;
        }

        /// <summary>
        ///   Updates the weights of the visible layer by copying
        ///   the reverse of the weights in the hidden layer.
        /// </summary>
        /// 
        public void UpdateVisibleWeights()
        {
            Visible.CopyReversedWeightsFrom(Hidden);
        }
    }
}
