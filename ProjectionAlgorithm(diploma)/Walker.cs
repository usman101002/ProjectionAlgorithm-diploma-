using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonteKarloMatrixVectorProduct
{
    internal class Walker
    {
        private readonly List<int> _alias;
        /// <summary>
        /// элементы стохастического вектора
        /// </summary>
        private readonly List<double> _vectorProbabilities;
        private readonly List<double> _probabilities;
        private readonly int[] _values;
        private readonly Random _random = new Random();
        private int _weightLength;
        private List<int> _underFull;
        private List<int> _overFull;

        public Walker(List<double> vectorProbabilities)
        {
            this._probabilities = new List<double>();
            this._alias = new List<int>();
            this._vectorProbabilities = vectorProbabilities;

            this._values = new int[vectorProbabilities.Count];
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = i;
            }

            this._weightLength = _values.Length;

            this._underFull = new List<int>();
            this._overFull = new List<int>();

            PopulateProbabilityAndAlias();
            PopulateOverAndUnderfull();
            ProcessOverAndUnderfull();
        }

        /// <summary>
        /// заполняем массив, где каждый элемент это n * p[i], где p[i] это компонента вектора стохастического
        /// </summary>
        private void PopulateProbabilityAndAlias()
        {
            foreach (var prob in _vectorProbabilities)
            {
                _alias.Add(-1);
                var probability = _weightLength * prob;
                _probabilities.Add(probability);
            }
        }

        private void PopulateOverAndUnderfull()
        {
            for (var i = 0; i < _probabilities.Count(); i++)
            {
                if (_probabilities[i] < 1)
                    _underFull.Add(i);
                if (_probabilities[i] > 1)
                    _overFull.Add(i);
            }
        }

        private void ProcessOverAndUnderfull()
        {
            while (_underFull.Any() && _overFull.Any())
            {
                var currentUnder = _underFull.Pop();
                var currentOver = _overFull.Last();

                _alias[currentUnder] = currentOver;
                _probabilities[currentOver] -= (1 - _probabilities[currentUnder]);

                if (_probabilities[currentOver] < 1)
                {
                    _underFull.Add(currentOver);
                    _overFull.Pop();
                }
            }
        }

        private int GetBiasedRandom(int min, int max)
        {
            return Convert.ToInt32(Math.Floor(_random.NextDouble() * (max - min + 1)) + min);
        }

        /// <summary>
        /// метод выбирает случайный индекс из того массива, который мы передали 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public int GetSelection()
        {
            if (!_probabilities.Any() || !_alias.Any())
            {
                throw new InvalidOperationException("Weights have not been set. Build method must be called before a selection is made.");
            }

            var fairDiceRoll = _random.NextDouble();
            var biasedCoin = GetBiasedRandom(0, _weightLength - 1);
            var selectionIndex = _probabilities[biasedCoin] >= fairDiceRoll ? biasedCoin : _alias[biasedCoin];
            return selectionIndex;
        }


    }
}
