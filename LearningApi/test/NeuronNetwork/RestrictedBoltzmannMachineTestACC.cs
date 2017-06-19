﻿using NeuralNet.RestrictedBoltzmannMachine;
using NeuralNetworks.Core.ActivationFunctions;
using NUnit.Framework;

//using Xunit;

namespace test.NeuronNetwork
{
    [TestFixture]
    public class RestrictedBoltzmannTestACC
    {
        [Test]
        //[Theory]
        public void NetworkTestRBM()
        {
            // Create some sample inputs and outputs. Note that the
            // first four vectors belong to one class, and the other
            // four belong to another (you should see that the 1s
            // accumulate on the beginning for the first four vectors
            // and on the end for the second four).

            double[][] inputs =
            {
                new double[] { 1,1,1,0,0,0 }, // class a
                new double[] { 1,0,1,0,0,0 }, // class a
                new double[] { 1,1,1,0,0,0 }, // class a
                new double[] { 0,0,1,1,1,0 }, // class b
                new double[] { 0,0,1,1,0,0 }, // class b
                new double[] { 0,0,1,1,1,0 }, // class b
            };

            // Create a Bernoulli activation function
            var function = new BernoulliFunction( alpha: 0.5 );

            // Create a Restricted Boltzmann Machine for 6 inputs and with 1 hidden neuron
            var rbm = new RestrictedBoltzmannMachine( function, inputsCount: 6, hiddenNeurons: 2 );

            // Create the learning algorithm for RBMs
            var teacher = new ContrastiveDivergenceLearning( rbm, 5000, inputs, 0.1 )           
            {
                Momentum = 0,
               // LearningRate = 0.1,
                Decay = 0
            };

            // learn 5000 iterations
            //for (int i = 0; i < 5000; i++)
            //    teacher.RunEpoch( inputs );


            // Compute the machine answers for the given inputs:
            double[] a = rbm.Compute( new double[] { 1, 1, 1, 0, 0, 0 } ); // { 0.99, 0.00 }
            double[] b = rbm.Compute( new double[] { 0, 0, 0, 1, 1, 1 } ); // { 0.00, 0.99 }
            double[] c = rbm.Compute( new double[] { 1, 0, 1, 0, 0, 0 } );
            double[] d = rbm.Compute( new double[] { 1, 0, 1, 1, 0, 0 } );
            double[] e = rbm.Compute( new double[] { 1, 0, 1, 0, 1, 0 } );
            double[] f = rbm.Compute( new double[] { 1, 0, 1, 0, 0, 1 } );
            double[] g = rbm.Compute( new double[] { 0, 1, 0, 1, 0, 1 } );

            // As we can see, the first neuron responds to vectors belonging
            // to the first class, firing 0.99 when we feed vectors which 
            // have 1s at the beginning. Likewise, the second neuron fires 
            // when the vector belongs to the second class.

            // We can also generate input vectors given the classes:
            double[] xa = rbm.GenerateInput( new double[] { 1, 0 } ); // { 1, 1, 1, 0, 0, 0 }
            double[] xb = rbm.GenerateInput( new double[] { 0, 1 } ); // { 0, 0, 1, 1, 1, 0 }

            // As we can see, if we feed an output pattern where the first neuron
            // is firing and the second isn't, the network generates an example of
            // a vector belonging to the first class. The same goes for the second
            // neuron and the second class.

            Assert.IsTrue( ((a[0] > a[1]) && (b[0] < b[1]))
                      ^ ((a[0] < a[1]) && (b[0] > b[1])) );

        }

    }
}